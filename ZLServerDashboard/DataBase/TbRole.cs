using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbRole
    {
        public TbRole()
        {
            TbMenuRole = new HashSet<TbMenuRole>();
            TbUserRole = new HashSet<TbUserRole>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTs { get; set; }
        public int Status { get; set; }

        public virtual ICollection<TbMenuRole> TbMenuRole { get; set; }
        public virtual ICollection<TbUserRole> TbUserRole { get; set; }
    }
}
