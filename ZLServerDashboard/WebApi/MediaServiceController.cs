
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZLServerDashboard.Commons;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.ViewDto;
using ZLServerDashboard.Models.ZLMediaServer;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.WebApi
{
    [Route("api/[controller]/[action]")]
    public class MediaServiceController : ZLServerControllerBase
    {
        IMediaService mediaService;
        public MediaServiceController(IMediaService mediaService)
        {
            this.mediaService = mediaService;
        }



        [HttpGet]
        [HttpPost]
        public BaseModel<String> GetCamera(long id)
        {
            BaseModel<String> response = new BaseModel<String>();
            if (id == 0)
                return response.Failed("参数错误!");
            var streamProxy = mediaService.GetStreamProxy(id);
            if (streamProxy == null)
                return response.Failed("找不到摄像头!");
            if (streamProxy.State != StreamProxyState.Normal)
                return response.Failed("当前摄像头不在线!");
            StreamProxyViewDto dto = new StreamProxyViewDto();
            dto.Domain=mediaService.GetDomain(streamProxy.DomainId);
            dto.Application=mediaService.GetApplication(streamProxy.AppId);
            dto.StreamProxy = streamProxy;
            response.Data = dto.HttpFlvUrl+"&Ticket=hse";
            return response.Success("ok");
        }




    }
}
