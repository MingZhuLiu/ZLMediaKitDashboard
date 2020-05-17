using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbUser
    {
        public TbUser()
        {
            TbApplicationCreateByNavigation = new HashSet<TbApplication>();
            TbApplicationUpdateByNavigation = new HashSet<TbApplication>();
            TbDomainCreateByNavigation = new HashSet<TbDomain>();
            TbDomainUpdateByNavigation = new HashSet<TbDomain>();
            TbStreamProxyCreateByNavigation = new HashSet<TbStreamProxy>();
            TbStreamProxyUpdateByNavigation = new HashSet<TbStreamProxy>();
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

        public virtual ICollection<TbApplication> TbApplicationCreateByNavigation { get; set; }
        public virtual ICollection<TbApplication> TbApplicationUpdateByNavigation { get; set; }
        public virtual ICollection<TbDomain> TbDomainCreateByNavigation { get; set; }
        public virtual ICollection<TbDomain> TbDomainUpdateByNavigation { get; set; }
        public virtual ICollection<TbStreamProxy> TbStreamProxyCreateByNavigation { get; set; }
        public virtual ICollection<TbStreamProxy> TbStreamProxyUpdateByNavigation { get; set; }
        public virtual ICollection<TbUserRole> TbUserRole { get; set; }
    }
}
