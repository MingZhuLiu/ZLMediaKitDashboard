using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Models.Dto
{
    public class ApplicationDto
    {
        public long Id { get; set; }
        public string App { get; set; }
        public string Remark { get; set; }
        public ApplicationState Status { get; set; }
        public DateTime CreateTs { get; set; }
        public long CreateBy { get; set; }
        public DateTime UpdateTs { get; set; }
        public long UpdateBy { get; set; }

        public virtual UserDto CreateByNavigation { get; set; }
        public virtual UserDto UpdateByNavigation { get; set; }

        public String CreateByName => CreateByNavigation?.Name;
        public String UpdateByName => UpdateByNavigation?.Name;
    }
}
