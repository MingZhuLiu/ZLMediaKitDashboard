using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbMenuRole
    {
        public long MenuId { get; set; }
        public long RoleId { get; set; }

        public virtual TbMenu Menu { get; set; }
        public virtual TbRole Role { get; set; }
    }
}
