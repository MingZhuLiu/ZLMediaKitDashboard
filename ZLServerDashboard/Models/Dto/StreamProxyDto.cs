using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Models.Dto
{
    public class StreamProxyDto
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
        public RTPType RtpType { get; set; }
        public StreamProxyState State { get; set; }
        public DateTime CreateTs { get; set; }
        public long CreateBy { get; set; }
        public DateTime UpdateTs { get; set; }
        public long UpdateBy { get; set; }

        public virtual ApplicationDto App { get; set; }
        public virtual UserDto CreateByNavigation { get; set; }
        public virtual DomainDto Domain { get; set; }
        public virtual UserDto UpdateByNavigation { get; set; }

        public String CreateByName => CreateByNavigation?.Name;
        public String UpdateByName => UpdateByNavigation?.Name;

        public String ApplicationName => App?.App;
        public String DomainName => Domain?.DomainName;
    }
}
