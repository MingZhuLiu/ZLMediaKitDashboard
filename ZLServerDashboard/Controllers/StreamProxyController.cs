

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ZhiHuReptile.Web.Commons;
using ZLServerDashboard.Commons;
using ZLServerDashboard.DataBase;
using ZLServerDashboard.Filters;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;
using ZLServerDashboard.Models.ViewDto;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Controllers
{
    public class StreamProxyController : BaseController
    {
        private readonly ILogger<HomeController> _logger;


        public StreamProxyController(ILogger<HomeController> logger, IUserService userService, IMediaService mediaService)
        {
            _logger = logger;
            this.mediaService = mediaService;
            this.userService = userService;
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Index()
        {

            PowerDto dto = new PowerDto();
            var menus = userService.FindUserMenus(UserInfo, false);
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("/StreamProxy") && p.Url.Contains("/Add")).Any())
                dto.Add = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("/StreamProxy") && p.Url.Contains("/Edit")).Any())
                dto.Edit = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("/StreamProxy") && p.Url.Contains("/Delete")).Any())
                dto.Delete = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("/StreamProxy") && p.Url.Contains("/ViewDetail")).Any())
                dto.RolePower = true;
            return View(dto);
        }

        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public TableQueryModel<StreamProxyDto> Index(QueryModel query)
        {
            return mediaService.GetStreamProxyList(query);
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Add()
        {
            SteamProxyEditDataSource dataSource = new SteamProxyEditDataSource();
            dataSource.Domains = mediaService.GetAllNoramlDomain();
            dataSource.Applications = mediaService.GetAllNoramlApplication();
            return View(dataSource);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Add(StreamProxyDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                baseModel = mediaService.AddStreamProxy(dto, UserInfo);
            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }


        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Edit(long id)
        {
            SteamProxyEditDataSource dataSource = new SteamProxyEditDataSource();
            dataSource.Domains = mediaService.GetAllNoramlDomain();
            dataSource.Applications = mediaService.GetAllNoramlApplication();
            dataSource.Dto = mediaService.GetStreamProxy(id);
            return View(dataSource);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Edit(StreamProxyDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                baseModel = mediaService.EditStreamProxy(dto, UserInfo);
            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Delete(long[] ids)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (mediaService.DeleteStreamProxy(ids, UserInfo))
                {
                    baseModel.Success("删除成功!");
                }
                else
                {
                    baseModel.Failed("删除失败!");
                }
            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }



        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult ViewDetail(long id)
        {
            StreamProxyViewDto dto = new StreamProxyViewDto();
            dto.Token = Tools.URIEncode(TokenDto.TokenStr);
            dto.StreamProxy = mediaService.GetStreamProxy(id);
            if (dto.StreamProxy != null)
            {
                dto.Domain = mediaService.GetDomain(dto.StreamProxy.DomainId);
                dto.Application = mediaService.GetApplication(dto.StreamProxy.AppId);
                // var mediaInfo = mediaService.GetStreamInfo(dto.Domain?.DomainName, dto.Application?.App, dto.StreamProxy.StreamName);
                // if (mediaInfo != null)
                // {
                //     dto.StreamProxyJson = Tools.ConvertStringToJson(Newtonsoft.Json.JsonConvert.SerializeObject(mediaInfo));
                // }
                // else
                // {
                //     dto.StreamProxyJson = "Error:流不在线!";
                // }
            }
            return View(dto);
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult DialogView(long id)
        {
            StreamProxyViewDto dto = new StreamProxyViewDto();
            if (id != 0)
            {
                dto.Token = Tools.URIEncode(TokenDto.TokenStr);
                dto.StreamProxy = mediaService.GetStreamProxy(id);
                if (dto.StreamProxy != null)
                {
                    dto.Domain = mediaService.GetDomain(dto.StreamProxy.DomainId);
                    dto.Application = mediaService.GetApplication(dto.StreamProxy.AppId);
                    if(dto.StreamProxy.State == StreamProxyState.Normal)
                        dto.isOnline = true;
                    // var mediaInfo = mediaService.GetStreamInfo(dto.Domain?.DomainName, dto.Application?.App, dto.StreamProxy.StreamName);
                    // if (mediaInfo != null)
                    // {
                    //     dto.isOnline = true;
                    //     dto.StreamProxyJson = Tools.ConvertStringToJson(Newtonsoft.Json.JsonConvert.SerializeObject(mediaInfo));
                    // }
                    // else
                    // {
                    //     dto.isOnline = false;
                    //     dto.StreamProxyJson = "Error:流不在线!";
                    // }
                }
            }
            else
            {
                dto.isOnline = false;
            }

            return View(dto);
        }


    }
}
