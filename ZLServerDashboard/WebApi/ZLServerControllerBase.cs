
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZLServerDashboard.Commons;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models.ZLMediaServer;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.WebApi
{
    public class ZLServerControllerBase : ControllerBase
    {
        public ZLServerControllerBase()
        {

        }


        protected T GetReqBody<T>()
        {
            var request = HttpContext.Request;
            request.EnableBuffering();
            var postJson = "";
            var stream = HttpContext.Request.Body;
            long? length = HttpContext.Request.ContentLength;
            if (length != null && length > 0)
            {
                // 使用这个方式读取，并且使用异步
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                postJson = streamReader.ReadToEndAsync().Result;
            }
            if (String.IsNullOrEmpty(postJson))
            {
                return default(T);
            }
            else
            {
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(postJson);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        public NameValueCollection GetUrlParas(string para)
        {
            if (String.IsNullOrWhiteSpace(para))
                return new NameValueCollection();
            // para = Tools.URLDecode(para);
            return Tools.ExtractQueryParams(para);
        }





    }
}
