﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLServerDashboard.Models.ViewDto
{
    public class LoginResultDto<T> : BaseModel<T>
    {
        public string Token { get; set; }
    }
}
