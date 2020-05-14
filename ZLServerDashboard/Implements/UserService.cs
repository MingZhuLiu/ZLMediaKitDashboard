using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhiHuReptile.Web.Commons;
using ZLServerDashboard.Commons;
using ZLServerDashboard.DataBase;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;
using ZLServerDashboard.Models.ViewDto;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Implements
{
    public class UserService : IUserService
    {

        private readonly MediaPlatContext dbContext;
        private IMapper mapper;
        private IRoleService roleService;
        private IMenuService menuService;
        public UserService(MediaPlatContext dbContext, IMapper mapper, IRoleService roleService, IMenuService menuService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.roleService = roleService;
            this.menuService = menuService;
        }

        public UserDto FindUserByLoginName(string loginName)
        {
            var dto = RedisHelper.Instance.GetHash<UserDto>(loginName);
            if (dto == null)
            {
                var dbModel = dbContext.TbUser.Include(p => p.TbUserRole).Where(p => p.LoginName == loginName).FirstOrDefault();
                if (dbModel != null)
                {
                    dto = mapper.Map<UserDto>(dbModel);
                    //dto.Roles = dbContext.TbRole.Where(p => dbModel.TbUserRole.Select(p => p.RoleId).ToList().Contains(p.Id)).Select(p => mapper.Map<RoleDto>(p)).ToList();// dbModel.TbUserRole.Select(p => p.Role).Select(p => mapper.Map<RoleDto>(p)).ToList();
                    RedisHelper.Instance.SetHash<UserDto>(loginName, dto);
                }
            }

            return dto;
        }

        public LoginResultDto<UserDto> LoginCheck(string account, string passwd, Enums.LoginPlatform loginPlatform)
        {
            LoginResultDto<UserDto> result = new LoginResultDto<UserDto>();


            try
            {
                if (String.IsNullOrWhiteSpace(account) || String.IsNullOrWhiteSpace(passwd))
                {
                    result.Failed("用户名/密码不能为空!");
                    return result;
                }


                //passwd = RSAHelper.Instance.Encrypt(account+passwd);
                var user = FindUserByLoginName(account);
                if (user == null || user.State == Enums.UserStatus.Deleted || user == null || user.State == Enums.UserStatus.Deleted || user.LoginPasswd != passwd)
                {
                    result.Failed("用户名/密码错误!");
                    return result;
                }
                if (user.State == Enums.UserStatus.Forbid)
                {
                    result.Failed("您的账号已被停用,请联系单位管理员!");
                    return result;
                }


                //走到这里说明验证通过

                if (loginPlatform == Enums.LoginPlatform.Web)
                {
                    var token = Tools.CreateWebToken(user.Id, user.LoginName, Enums.LoginPlatform.Web);
                    RedisHelper.Instance.SetHash(RedisCacheTables.WebTokenDto, user.Id, token);
                    result.Success("登录成功!");
                    result.Data = user;
                    result.Token = token;
                }
                else if (loginPlatform == Enums.LoginPlatform.Android)
                {

                }
                else if (loginPlatform == Enums.LoginPlatform.IOS)
                {

                }
                else if (loginPlatform == Enums.LoginPlatform.WeiXinProgram)
                {

                }
            }
            catch (Exception ex)
            {
                result.Failed(ex);
            }


            return result;
        }

        public bool ValidateToken(string token, LoginPlatform loginPlatform)
        {
            return Tools.ValidateToken(token, loginPlatform);
        }



        public List<MenuDto> FindUserMenus(UserDto user, bool allMenu = true)
        {
            List<MenuDto> menus = new List<MenuDto>();
            var roles = roleService.FindUserRoles(user);

            foreach (var item in roles)
            {
                var roleMenus = menuService.FindMenusByRole(item);
                foreach (var menu in roleMenus)
                {
                    if (!menus.Where(p => p.Id == menu.Id).Any())
                        if (allMenu == true || (allMenu == false && menu.Status == MenuStatus.Normal))
                            menus.Add(menu);
                }
            }
            return menus;
        }

        public List<MenuDto> FindUserMenuTree(UserDto user, bool allMenu = true)
        {
            var menus = FindUserMenus(user, allMenu);

            menus = menus.ToTree<MenuDto>(
                    (r, c) => { return c.ParentId==0||!c.ParentId.HasValue; },
                    (r, c) =>
                    {
                        return r.Id == c.ParentId;
                    },
                    (r, datalist) =>
                    {
                        r.Children.AddRange(datalist);
                    }
                    );
            return menus;

        }

        public bool LoginOut(string token, LoginPlatform loginPlatform)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(token))
                    return false;

                var webToken = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenDto>(RSAHelper.Instance.Decrypt(token));
                if (webToken != null)
                {
                    string lastToken;
                    bool result = false;
                    switch (loginPlatform)
                    {
                        case LoginPlatform.Web:
                            lastToken = RedisHelper.Instance.GetHash<String>(RedisCacheTables.WebTokenDto, webToken.UserId);
                            if (lastToken != null && lastToken.Equals(webToken))
                                result = RedisHelper.Instance.DeleteHash(RedisCacheTables.WebTokenDto, webToken.UserId);
                            else
                                result = true;
                            break;
                            //case LoginPlatform.DownloadClient:
                            //    lastToken = RedisHelper.Instance.GetHash<String>(RedisCacheTables.DownloadClientTokenDto, webToken.UserId);
                            //    if (lastToken != null && lastToken.Equals(webToken))
                            //        result = RedisHelper.Instance.DeleteHash(RedisCacheTables.DownloadClientTokenDto, webToken.UserId);
                            //    else
                            //        result = true;
                            //    break;
                            //case LoginPlatform.BusinessClient:
                            //    lastToken = RedisHelper.Instance.GetHash<String>(RedisCacheTables.BusinessClientTokenDto, webToken.UserId);
                            //    if (lastToken != null && lastToken.Equals(webToken))
                            //        result = RedisHelper.Instance.DeleteHash(RedisCacheTables.BusinessClientTokenDto, webToken.UserId);
                            //    else
                            //        result = true;
                            //    break;
                    }
                    return result;
                }
                else
                {
                    return false;
                }
                //var array = desToken.Split("&nbspqxsd;");
                //if (array.Length != 2)
                //{
                //    return false;
                //}
                //else
                //{


                //    var webotken = RedisHelper.Instance.DeleteKey(array[0]);
                //    if (webotken == null || webotken.Token != token)
                //        return false;
                //    else
                //    {
                //        return RedisHelper.Instance.DeleteHash<WebTokenDto>(webotken.Account);
                //    }
                //}
            }
            catch
            {
                return false;
            }
        }

        public TableQueryModel<UserDto> GetUserList(QueryModel queryModel)
        {
            TableQueryModel<UserDto> tableQueryModel = new TableQueryModel<UserDto>();
            try
            {
                List<UserDto> menus = new List<UserDto>();
                var query = dbContext.TbUser.Where(p => p.State != (int)UserStatus.Deleted);
                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.Name.Contains(queryModel.keyword)
                    || p.Address.Contains(queryModel.keyword)
                    || p.Tel.Contains(queryModel.keyword)
                    || p.LoginName.Contains(queryModel.keyword)
                    ).AsQueryable();
                }


                tableQueryModel.count = query.Count();
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                query = query.Take(queryModel.limit);

                var list = query.ToList();
                list.ForEach(p => { var dto = mapper.Map<UserDto>(p); menus.Add(mapper.Map<UserDto>(dto)); });

                tableQueryModel.code = 0;
                tableQueryModel.data = menus;
            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }

        public BaseModel<String> AddUser(UserDto dto, UserDto operaUser)
        {
            BaseModel<String> result = new BaseModel<string>();
            if (String.IsNullOrWhiteSpace(dto.Name))
                return result.Failed("姓名不能为空!");

            if (String.IsNullOrWhiteSpace(dto.LoginName))
                return result.Failed("账号不能为空!");
            if (String.IsNullOrWhiteSpace(dto.LoginPasswd))
            {
                return result.Failed("密码不能为空!");
            }
            else
            {
                dto.LoginPasswd = RSAHelper.Instance.Encrypt(dto.LoginPasswd);
            }


            if (dbContext.TbUser.Where(p => p.LoginName == dto.LoginName).Any())
                return result.Failed("用户名已存在!");

            if (dto.TempRoleId == 0)
            {
                //用户的角色设置的是空,不允许
                return result.Failed("角色错误!");
            }
            else
            {
                //检查是否超越操作者的权限
                var operaRole = roleService.GetCanPowerRoles(operaUser).Select(p => p.Id).ToArray();
                if (!operaRole.Contains(dto.TempRoleId.Value))
                {
                    return result.Failed("角色错误,禁止越权操作!");
                }
            }

          
            TbUser sysUser = new TbUser();
            sysUser = mapper.Map<TbUser>(dto);
            sysUser.Id = Tools.NewID;
            sysUser.CreateTs = DateTime.Now;
            dbContext.TbUser.Add(sysUser);
            var flag = dbContext.SaveChanges() > 0 ? true : false;

            if (flag)
            {
                //dto = mapper.Map<UserDto>(sysUser);
                roleService.SetUserRole(sysUser.Id, new long[] { dto.TempRoleId.Value });
                result.Success("保存成功!");
            }
            else
            {
                result.Success("保存失败!");
            }

            return result;
        }


        public BaseModel<String> EditUser(UserDto dto, UserDto operaUser)
        {
            BaseModel<String> result = new BaseModel<string>();
            if (String.IsNullOrWhiteSpace(dto.Name))
                return result.Failed("姓名不能为空!");
            if (String.IsNullOrWhiteSpace(dto.LoginName))
                return result.Failed("账号不能为空!");
            var dbModel = dbContext.TbUser.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(dto.LoginPasswd))
            {

            }
            else
            {
                dbModel.LoginPasswd = dto.LoginPasswd;
            }

            if (dbContext.TbUser.Where(p => p.LoginName == dto.LoginName && p.Id != dto.Id).Any())
                return result.Failed("用户名已存在!");
            if (dto.TempRoleId==0)
            {
                //用户的角色设置的是空,不允许
                return result.Failed("角色错误!");
            }
            else
            {
                //检查是否超越操作者的权限
                var operaRole = roleService.GetCanPowerRoles(operaUser).Select(p => p.Id).ToArray();
                if (!operaRole.Contains(dto.TempRoleId.Value))
                {
                    return result.Failed("角色错误,禁止越权操作!");
                }
            }

            dbModel.LoginName = dto.LoginName;
            dbModel.Address = dto.Address;
            dbModel.Name = dto.Name;
            dbModel.Sex = (int)dto.Sex;
            dbModel.State = (int)dto.State;
            dbModel.Tel = dto.Tel;

            var flag = dbContext.SaveChanges() > 0 ? true : false;
            var flag1 = roleService.SetUserRole(dbModel.Id, new long[] { dto.TempRoleId.Value });
            RedisHelper.Instance.DeleteHash<UserDto>(dbModel.LoginName);
            if (flag || flag1)
            {

                result.Success("保存成功!");
            }
            else
            {
                result.Success("保存失败!");
            }

            return result;
        }

        public UserDto FindUserById(long id)
        {
            var dbModel = dbContext.TbUser.Where(p => p.Id == id).FirstOrDefault();
            if (dbModel != null)
                return mapper.Map<UserDto>(dbModel);
            else
                return null;
        }

        public BaseModel<string> DeleteUsers(long[] ids, UserDto operaUser)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (ids == null || ids.Length == 0)
                return result.Failed("参数错误!");

            var users = dbContext.TbUser.Where(p => ids.Contains(p.Id)).ToList();
            foreach (var item in users)
            {
                item.State = (int)UserStatus.Deleted;
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                foreach (var item in users)
                {
                    RedisHelper.Instance.DeleteHash<UserDto>(item.LoginName);
                }
                result.Success("删除成功!");
            }
            else
            {
                result.Failed("删除失败!");
            }
            return result;

        }

        public BaseModel<String> ResetAdminPassword()
        {
            BaseModel<String> result = new BaseModel<string>();
            try
            {
                var dbModel = dbContext.TbUser.Where(p => p.LoginName == "admin").FirstOrDefault();
                dbModel.LoginPasswd = RSAHelper.Instance.Encrypt("1");
                var flag = dbContext.SaveChanges() > 0 ? true : false;
                if (flag)
                {
                    RedisHelper.Instance.DeleteHash<UserDto>(dbModel.LoginName);
                    result.Success("重置成功!");
                }
                else
                {
                    result.Failed("重置失败!");
                }
            }
            catch (Exception ex)
            {
                result.Failed(ex);
            }
            return result;
        }

      
    }
}
