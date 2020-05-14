using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;

namespace ZLServerDashboard.Interface
{
    public interface IMenuService
    {
        public List<MenuDto> FindMenusByRole(RoleDto role);

        public TableQueryModel<MenuDto> GetMenuList(QueryModel query);

        public MenuDto FindMenu(long id);
        public bool AddMenu(MenuDto dto);
        public bool EditMenu(MenuDto dto);

        public bool DeleteMenu(long[] ids);

    }
}
