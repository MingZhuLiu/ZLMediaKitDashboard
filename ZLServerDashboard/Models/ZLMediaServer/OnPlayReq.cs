using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLServerDashboard.Models.ZLMediaServer
{
    public class OnPlayReq
    {
        public string app { get; set; }
        public string id { get; set; }
        public string ip { get; set; }
        public string @params { get; set; }
        public int port { get; set; }
        public string schema { get; set; }
        public string stream { get; set; }
        public string vhost { get; set; }
    }


}
