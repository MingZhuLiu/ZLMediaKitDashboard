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
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public static String uuid = "0";

        public HomeController(MediaPlatContext dbContext, ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.userService = userService;
        }


        [HttpGet]
        [SkipGlobalActionFilterAttribute]
        public IActionResult Login()
        {
            var token = GetCookies(CookieKeys.WebToken);
            if (!String.IsNullOrWhiteSpace(token) && userService.ValidateToken(token, LoginPlatform.Web))
            {
                return RedirectToAction("Index");
            }
            else
            {

                ViewBag.LastAccount = GetCookies(CookieKeys.LastLoginAccout);
                return View();
            }
        }



        [HttpGet]
        [SkipGlobalActionFilterAttribute]
        public IActionResult LogOut()
        {
            var token = GetCookies(CookieKeys.WebToken);
            if (!String.IsNullOrWhiteSpace(token))
            {
                DeleteCookies(CookieKeys.WebToken);
                userService.LoginOut(token, LoginPlatform.Web);
            }
            return RedirectToAction("Login");
        }


        [HttpPost]
        [SkipGlobalActionFilterAttribute]
        public IActionResult Login(UserDto dto)
        {
            var result = userService.LoginCheck(dto.LoginName, dto.LoginPasswd, LoginPlatform.Web);
            if (result.Flag)
            {
                SetCookies(CookieKeys.LastLoginAccout, dto.LoginName, 60 * 24 * 7);
                //登录成功.执行跳转
                SetCookies(CookieKeys.WebToken, result.Token, 60 * 24 * 3);
                return RedirectToAction("Index");
            }
            else
            {
                //登录失败,返回信息
                ViewBag.Info = result.Msg;
                return View();
            }

        }

        /// <summary>
        /// 修改过
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Index()
        {
            ViewBag.Account = UserInfo.LoginName;
            IndexPageDto dto = new IndexPageDto();
            dto.Title = "联力视频采集平台";
            dto.User = UserInfo;
            dto.Menus = userService.FindUserMenuTree(UserInfo);
            return View(dto);
        }






        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Dashboard()
        {
            ViewBag.ShowInfo = ConsoleHelper.ShowInfo;
            ViewBag.ShowWarning = ConsoleHelper.ShowWarning;
            ViewBag.ShowSuccess = ConsoleHelper.ShowSuccess;
            ViewBag.ShowFailed = ConsoleHelper.ShowFailed;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        [HttpPost]
        [SkipGlobalActionFilter]
        public IActionResult AuthError()
        {
            return View();
        }


        [HttpPost]
        [SkipGlobalActionFilter]
        public void ChangeLogLevel(int type, bool flag)
        {
            switch (type)
            {
                case 1:
                    ConsoleHelper.ShowInfo = flag;
                    break;
                case 2:
                    ConsoleHelper.ShowSuccess = flag;
                    break;
                case 3:
                    ConsoleHelper.ShowWarning = flag;
                    break;
                case 4:
                    ConsoleHelper.ShowFailed = flag;
                    break;
            }
        }



    }
}
