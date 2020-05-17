
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
using ZLServerDashboard.Models.ZLMediaServer;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.WebApi
{
    [Route("api/[controller]/[action]")]
    public class MediaKitHookController : ZLServerControllerBase
    {
        IMediaService mediaService;
        public MediaKitHookController(IMediaService mediaService)
        {
            this.mediaService = mediaService;
        }



        [HttpGet]
        [HttpPost]
        public BaseModel OnPlay()
        {
            var req = GetReqBody<OnPlayReq>();
            ConsoleHelper.Warning("触发了播放事件,远端IP" + req.ip + ":" + req.port + ",请求播放域[" + req.vhost + "],应用[" + req.app + "],流[" + req.stream + "]");
            BaseModel response = new BaseModel();
            response.Failed("403");
            try
            {
                var para = GetUrlParas(req?.@params);
                if (para.AllKeys.Contains("Token"))
                {
                    //走本系统Token验证逻辑
                    if (ValidateToken(para["Token"]))
                    {
                        response.Success("200");
                    }
                    else
                    {
                        response.Failed("403");
                    }
                }
                else if (para.AllKeys.Contains("Ticket"))
                {
                    //走本系统Ticket验证逻辑
                    if (ValidateTicket(para["Ticket"]))
                    {
                        response.Success("200");
                    }
                    else
                    {
                        response.Failed("403");
                    }
                }

            }
            catch (Exception ex)
            {
                ConsoleHelper.Failed("身份验证异常:" + ex.Message);
                response.Failed("403");
            }

            if (response.code == 0)
            {
                ConsoleHelper.Success("远端IP" + req.ip + ":" + req.port + "请求播放域[" + req.vhost + "],应用[" + req.app + "],流[" + req.stream + "]身份验证成功!");
            }
            else
            {
                ConsoleHelper.Failed("远端IP" + req.ip + ":" + req.port + "请求播放域[" + req.vhost + "],应用[" + req.app + "],流[" + req.stream + "]身份验证失败!");
            }
            return response;
        }

        private bool ValidateToken(string token)
        {

            if (String.IsNullOrWhiteSpace(token))
            {
                ConsoleHelper.Failed("身份验证失败,Token为空!");
                return false;
            }
            else
            {
                token = Tools.URLDecode(token);
                return Tools.ValidateToken(token, LoginPlatform.Web);
            }
        }
        private bool ValidateTicket(string ticket)
        {

            if (String.IsNullOrWhiteSpace(ticket))
            {
                ConsoleHelper.Failed("身份验证失败,Ticket为空!");
                return false;
            }
            else
            {
                //临时写死
                if(ticket=="hse")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [HttpGet]
        [HttpPost]
        public OnRtspRealmResp OnRtspRealm()
        {
            var req = GetReqBody<OnPlayReq>();
            ConsoleHelper.Warning("触发了鉴权事件,远端IP" + req.ip + ":" + req.port + ",请求播放域[" + req.vhost + "],应用[" + req.app + "],流[" + req.stream + "]");
            OnRtspRealmResp response = new OnRtspRealmResp();
            response.realm = "";
            response.Success("ok");
            return response;
        }

        [HttpGet]
        [HttpPost]
        public BaseModel OnRtspAuth()
        {
            var req = GetReqBody<OnPlayReq>();
            ConsoleHelper.Warning("触发了RTSP身份验证事件,远端IP" + req.ip + ":" + req.port + ",请求播放域[" + req.vhost + "],应用[" + req.app + "],流[" + req.stream + "]");
            BaseModel response = new BaseModel();
            response.Failed("403");
            try
            {
                var para = GetUrlParas(req?.@params);
                if (para.AllKeys.Contains("Token"))
                {
                    //走本系统Token验证逻辑
                    if (ValidateToken(para["Token"]))
                    {
                        response.Success("200");
                    }
                    else
                    {
                        response.Failed("403");
                    }
                }

            }
            catch (Exception ex)
            {
                ConsoleHelper.Failed("身份验证异常:" + ex.Message);
                response.Failed("403");
            }

            if (response.code == 0)
            {
                ConsoleHelper.Success("远端IP" + req.ip + ":" + req.port + "请求播放域[" + req.vhost + "],应用[" + req.app + "],流[" + req.stream + "]身份验证成功!");
            }
            else
            {
                ConsoleHelper.Failed("远端IP" + req.ip + ":" + req.port + "请求播放域[" + req.vhost + "],应用[" + req.app + "],流[" + req.stream + "]身份验证失败!");
            }
            return response;
        }



        [HttpGet]
        [HttpPost]
        public BaseModel OnServerStarted()
        {
            BaseModel response = new BaseModel();
            var medias = mediaService.GetStreamProxyList(new Models.QueryModel() { page = 1, limit = 9999999 })
             .data.Where(p => p.State == StreamProxyState.Normal).ToList();
            medias.ForEach(p =>
            {
                if (p.Domain.Status == DomainState.Normal && p.App.Status == ApplicationState.Normal)
                {
                    mediaService.AddStreamProxy(p, p.Domain, p.App);
                }
            });
            return response.Success("ok");
        }







    }
}
