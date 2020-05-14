using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbMenu
    {
        public TbMenu()
        {
            InverseParent = new HashSet<TbMenu>();
            TbMenuRole = new HashSet<TbMenuRole>();
        }

        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public DateTime CreateTs { get; set; }
        public int Status { get; set; }

        public virtual TbMenu Parent { get; set; }
        public virtual ICollection<TbMenu> InverseParent { get; set; }
        public virtual ICollection<TbMenuRole> TbMenuRole { get; set; }
    }
}
