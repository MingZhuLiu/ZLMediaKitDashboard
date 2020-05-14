using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLServerDashboard.Models
{
    public class WebApiQueryModel
    {
        public string enterpriseId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
