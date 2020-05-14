using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLServerDashboard.Commons;
using ZLServerDashboard.DataBase;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Implements
{
    public class RoleService : IRoleService
    {
        private readonly MediaPlatContext dbContext;
        private IMapper mapper;
        public RoleService(MediaPlatContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }



        public List<RoleDto> FindUserRoles(UserDto user)
        {
            var roles = RedisHelper.Instance.GetHash<List<RoleDto>>(Models.Enums.RedisCacheTables.User_Roles, user.Id);
            if (roles == null || roles.Count == 0)
            {
                var userroles = dbContext.TbUserRole.Where(p => p.UserId == user.Id).Select(p => p.RoleId).ToList();
                roles = dbContext.TbRole.Where(p => userroles.Contains(p.Id)).Select(p => mapper.Map<RoleDto>(p)).ToList();
                RedisHelper.Instance.SetHash(RedisCacheTables.User_Roles, user.Id, roles);
            }
            return roles;
        }

        public TableQueryModel<RoleDto> QueryRoleList(QueryModel queryModel)
        {
            TableQueryModel<RoleDto> tableQueryModel = new TableQueryModel<RoleDto>();
            try
            {
                List<RoleDto> roles = new List<RoleDto>();
                var query = dbContext.TbRole.Where(p => p.Status != (int)RoleStatus.Deleted);
              

                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.Name.Contains(queryModel.keyword)).AsQueryable();
                }
                tableQueryModel.count = query.Count();
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                query = query.Take(queryModel.limit);

                var list = query.ToList();
                list.ForEach(p => roles.Add(mapper.Map<RoleDto>(p)));

                tableQueryModel.code = 0;
                tableQueryModel.data = roles;

            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }

        public bool AddRole(RoleDto dto, UserDto user)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name))
                return false;
            if (dto.Name.Contains("管理员"))
                return false;
            TbRole role = new TbRole();
            role.Id = Tools.NewID;
            role.CreateTs = DateTime.Now;
            role.Description = dto.Description;
            role.Name = dto.Name;
            role.Status = (int)dto.Status;
           
            dbContext.TbRole.Add(role);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            return flag;
        }

        public RoleDto FindRole(long id)
        {
            var dbModel = dbContext.TbRole.Where(p => p.Id == id).FirstOrDefault();
            if (dbModel != null)
                return mapper.Map<RoleDto>(dbModel);
            else
                return null;
        }

        public bool EditRole(RoleDto dto, UserDto user)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name) )
                return false;
            if (dto.Name.Contains("管理员"))
                return false;
            var dbModel = dbContext.TbRole.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (dbModel == null)
                return false;

       

            dbModel.Name = dto.Name;
            dbModel.Description = dto.Description;
            dbModel.Status = (int)dto.Status;
            
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                //删除缓存
                RedisHelper.Instance.DeleteHash(RedisCacheTables.Role_Menus, dto.Id);
            }
            return flag;
        }


        public bool DeleteRole(long[] ids, UserDto user)
        {
            if (ids == null)
                return false;
            var roles = dbContext.TbRole.Where(p => ids.Contains(p.Id)).ToList();
            foreach (var item in roles)
            {
               
                item.Status = (int)RoleStatus.Deleted;
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                //删除缓存
                foreach (var item in roles)
                {
                    RedisHelper.Instance.DeleteHash(RedisCacheTables.Role_Menus, item.Id);
                }
            }
            return flag;
        }

        public bool RolePower(long roleId, long[] menusIds)
        {
            var list = dbContext.TbMenuRole.Where(p => p.RoleId == roleId).ToList();
            if (list != null && list.Count != 0)
                dbContext.TbMenuRole.RemoveRange(list);
            dbContext.SaveChanges();
            if (menusIds != null)
            {
                foreach (var item in menusIds)
                {
                    TbMenuRole mr = new TbMenuRole();
                    mr.MenuId = item;
                    mr.RoleId = roleId;
                    dbContext.TbMenuRole.Add(mr);
                }
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                //删除缓存
                RedisHelper.Instance.DeleteHash(RedisCacheTables.Role_Menus, roleId);
            }
            return flag;
        }

        public bool SetUserRole(long userId, long[] roleIds)
        {
            var removes = dbContext.TbUserRole.Where(p => p.UserId == userId).ToList();
            foreach (var item in removes)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            foreach (var item in roleIds)
            {
                TbUserRole role = new TbUserRole();
                role.UserId = userId;
                role.RoleId = item;
                dbContext.TbUserRole.Add(role);
            }
            dbContext.SaveChanges();
            RedisHelper.Instance.DeleteHash(Models.Enums.RedisCacheTables.User_Roles, userId);
            return true;
        }

        public List<RoleDto> GetCanPowerRoles(UserDto user)
        {
            var hasRoles = FindUserRoles(user);
            if (hasRoles.Where(p => p.Name == "超级管理员").Any())
            {
                //顶级超级管理员
                return QueryRoleList(new QueryModel() { page = 1, limit = 999999999 }).data;
            }
            else
            {
                //普通用户,将自己的权限返回回去
                return FindUserRoles(user);
            }
        }
    }
}
