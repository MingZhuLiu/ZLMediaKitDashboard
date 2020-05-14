using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbUserRole
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public virtual TbRole Role { get; set; }
        public virtual TbUser User { get; set; }
    }
}
