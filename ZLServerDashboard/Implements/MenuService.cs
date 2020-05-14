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
    public class MenuService : IMenuService
    {
        private readonly MediaPlatContext dbContext;
        private IMapper mapper;
        public MenuService(MediaPlatContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public List<MenuDto> FindMenusByRole(RoleDto role)
        {
            var menus = RedisHelper.Instance.GetHash<List<MenuDto>>(Models.Enums.RedisCacheTables.Role_Menus, role.Id);
            if (menus == null || menus.Count == 0)
            {
                var dbMenuRoles = dbContext.TbMenuRole.Where(p => p.RoleId == role.Id).Select(p => mapper.Map<MenuRoleDto>(p)).ToList();
                menus = dbContext.TbMenu.Where(p => dbMenuRoles.Select(q => q.MenuId).ToList().Contains(p.Id) && p.Status != (int)MenuStatus.Deleted).Select(p => mapper.Map<MenuDto>(p)).ToList();
                RedisHelper.Instance.SetHash(Models.Enums.RedisCacheTables.Role_Menus, role.Id, menus);
            }
            return menus;
        }

        public TableQueryModel<MenuDto> GetMenuList(QueryModel queryModel)
        {
            TableQueryModel<MenuDto> tableQueryModel = new TableQueryModel<MenuDto>();
            try
            {
                List<MenuDto> menus = new List<MenuDto>();
                var query = dbContext.TbMenu.Where(p => p.Status != (int)MenuStatus.Deleted);
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
                list.ForEach(p => menus.Add(mapper.Map<MenuDto>(p)));

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


        public MenuDto FindMenu(long id)
        {
            var dbModel = dbContext.TbMenu.Where(p => p.Id == id).FirstOrDefault();
            if (dbModel != null)
                return mapper.Map<MenuDto>(dbModel);
            else
                return null;
        }


        public bool AddMenu(MenuDto dto)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name))
                return false;
            dto.Id = Tools.NewID;
            dto.CreateTs = DateTime.Now;
            var dbModel = mapper.Map<TbMenu>(dto);
            dbContext.TbMenu.Add(dbModel);
            return dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool EditMenu(MenuDto dto)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name))
                return false;
            var dbModel = dbContext.TbMenu.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (dbModel == null)
                return false;
            dbModel.Icon = dto.Icon;
            dbModel.Name = dto.Name;
            dbModel.Order = dto.Order;
            dbModel.Status = (int)dto.Status;
            dbModel.ParentId = dto.ParentId;
            dbModel.Url = dto.Url;
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                //删除缓存
                RedisHelper.Instance.DeleteKey(RedisCacheTables.Role_Menus);
            }
            return flag;
        }

        public bool DeleteMenu(long[] ids)
        {
            if (ids == null)
                return false;
            var menus = dbContext.TbMenu.Where(p => ids.Contains(p.Id)).ToList();
            foreach (var item in menus)
            {
                item.Status = (int)MenuStatus.Deleted;
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                //删除缓存
                RedisHelper.Instance.DeleteKey(RedisCacheTables.Role_Menus);
            }
            return flag;
        }
    }
}
