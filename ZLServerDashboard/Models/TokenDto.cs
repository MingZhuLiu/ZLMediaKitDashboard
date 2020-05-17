using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Models
{
    public class TokenDto
    {
        public String LoginAccount { get; set; }
        public String TokenStr { get; set; }
        public long UserId { get; set; }
        public LoginPlatform LoginPlatform { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
