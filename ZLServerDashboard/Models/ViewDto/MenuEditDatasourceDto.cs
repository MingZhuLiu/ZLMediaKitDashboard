using System.Collections.Generic;
using ZLServerDashboard.Models.Dto;

namespace ZLServerDashboard.Models.ViewDto
{
     public class MenuEditDatasourceDto
    {
        public MenuDto Menu { get; set; }
        public List<MenuDto> Menus { get; set; }
    }
}