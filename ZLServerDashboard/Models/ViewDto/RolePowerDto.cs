using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLServerDashboard.Models.ViewDto
{
    public class RolePowerDto
    {
        public long RoleId { get; set; }
        public List<LayuiTreeDto> Menus { get; set; }
    }
}
