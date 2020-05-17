

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
    public class ApplicationController : BaseController
    {
        private readonly ILogger<HomeController> _logger;


        public ApplicationController(ILogger<HomeController> logger, IUserService userService, IMediaService mediaService)
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
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("/Application") && p.Url.Contains("/Add")).Any())
                dto.Add = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("/Application") && p.Url.Contains("/Edit")).Any())
                dto.Edit = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("/Application") && p.Url.Contains("/Delete")).Any())
                dto.Delete = true;
            return View(dto);
        }

        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public TableQueryModel<ApplicationDto> Index(QueryModel query)
        {
            return mediaService.GetApplicationList(query);
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Add(ApplicationDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (mediaService.AddApplication(dto, UserInfo))
                {
                    baseModel.Success("保存成功!");
                }
                else
                {
                    baseModel.Failed("保存失败!");
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
        public IActionResult Edit(long id)
        {
            ApplicationDto dto = mediaService.GetApplication(id);
            return View(dto);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Edit(ApplicationDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (mediaService.EditApplication(dto, UserInfo))
                {
                    baseModel.Success("保存成功!");
                }
                else
                {
                    baseModel.Failed("保存失败!");
                }
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
                if (mediaService.DeleteApplication(ids, UserInfo))
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



    }
}
