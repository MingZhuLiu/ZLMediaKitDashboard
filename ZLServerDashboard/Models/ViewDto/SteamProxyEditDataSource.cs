using System.Collections.Generic;
using ZLServerDashboard.Models.Dto;

namespace ZLServerDashboard.Models.ViewDto
{
     public class SteamProxyEditDataSource
    {
        public StreamProxyDto Dto { get; set; }
        public List<DomainDto> Domains { get; set; }
        public List<ApplicationDto> Applications { get; set; }
    }
}