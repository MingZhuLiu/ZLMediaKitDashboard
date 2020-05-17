using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbApplication
    {
        public TbApplication()
        {
            TbStreamProxy = new HashSet<TbStreamProxy>();
        }

        public long Id { get; set; }
        public string App { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime CreateTs { get; set; }
        public long CreateBy { get; set; }
        public DateTime UpdateTs { get; set; }
        public long UpdateBy { get; set; }

        public virtual TbUser CreateByNavigation { get; set; }
        public virtual TbUser UpdateByNavigation { get; set; }
        public virtual ICollection<TbStreamProxy> TbStreamProxy { get; set; }
    }
}
