using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbUser
    {
        public TbUser()
        {
            TbUserRole = new HashSet<TbUserRole>();
        }

        public long Id { get; set; }
        public string LoginName { get; set; }
        public string LoginPasswd { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int State { get; set; }
        public DateTime CreateTs { get; set; }

        public virtual ICollection<TbUserRole> TbUserRole { get; set; }
    }
}
