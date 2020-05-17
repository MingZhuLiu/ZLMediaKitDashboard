using System;
using System.Collections.Generic;

namespace ZLServerDashboard.DataBase
{
    public partial class TbStreamProxy
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public long DomainId { get; set; }
        public long AppId { get; set; }
        public string StreamName { get; set; }
        public string SourceUrl { get; set; }
        public bool EnableRtsp { get; set; }
        public bool EnableRtmp { get; set; }
        public bool EnableHls { get; set; }
        public bool EnableMp4 { get; set; }
        public int RtpType { get; set; }
        public int State { get; set; }
        public DateTime CreateTs { get; set; }
        public long CreateBy { get; set; }
        public DateTime UpdateTs { get; set; }
        public long UpdateBy { get; set; }

        public virtual TbApplication App { get; set; }
        public virtual TbUser CreateByNavigation { get; set; }
        public virtual TbDomain Domain { get; set; }
        public virtual TbUser UpdateByNavigation { get; set; }
    }
}
