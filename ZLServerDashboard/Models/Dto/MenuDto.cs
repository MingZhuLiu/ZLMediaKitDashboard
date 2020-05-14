using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Models.Dto
{
    public class MenuDto
    {
        public MenuDto()
        {
            Children = new List<MenuDto>();
        }

        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public DateTime CreateTs { get; set; }
        public MenuStatus Status { get; set; }

        public List<MenuDto> Children { get; set; }
    }
}
