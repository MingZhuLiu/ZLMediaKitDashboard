using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLServerDashboard.Models.ZLMediaServer
{
    public class BaseModel
    {
        public int code { get; set; }
        public String msg { get; set; }
        // public T Data { get; set; }

        public BaseModel()
        {

        }
        public BaseModel(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }
        public BaseModel Success(string msg)
        {
            this.code = 0;
            this.msg = msg;
            return this;
        }

        public BaseModel Failed(Exception ex)
        {
            this.code = -1;
            this.msg = ex.Message;
            return this;
        }
        public BaseModel Failed(string msg)
        {
            this.code = -1;
            this.msg = msg;
            return this;
        }
    }

}
