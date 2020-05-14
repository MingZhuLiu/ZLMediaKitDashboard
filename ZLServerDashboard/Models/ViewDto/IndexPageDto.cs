using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLServerDashboard.Models.Dto;

namespace ZLServerDashboard.Models.ViewDto
{
    public class IndexPageDto
    {
        public string Title { get; set; }

        public UserDto User { get; set; }
        public List<MenuDto> Menus { get; set; }
    }
}
