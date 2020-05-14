using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhiHuReptile.Web.Commons;
using ZLServerDashboard.DataBase;
using ZLServerDashboard.Filters;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Controllers
{
    public class BaseController : Controller
    {
        protected IUserService userService;
        protected IMenuService menuService;
        protected IRoleService roleService;
        protected IMapper mapper;
        protected MediaPlatContext dbContext;

        private UserDto userDto;
        

        public UserDto UserInfo
        {
            get
            {
                if (userDto == null)
                {
                    if (TokenDto != null && !String.IsNullOrWhiteSpace(TokenDto.LoginAccount))
                        userDto = userService.FindUserByLoginName(TokenDto.LoginAccount);
                }
                return userDto;
            }
        }

        private TokenDto _TokenDto;
        public TokenDto TokenDto
        {
            get
            {
                if (_TokenDto == null)
                    _TokenDto = GetWebTokenDto();
                return _TokenDto;
            }
        }


        private TokenDto GetWebTokenDto()
        {
            var token = GetCookies(CookieKeys.WebToken);
            token = RSAHelper.Instance.Decrypt(token);
            var tokenDto = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenDto>(token);
            return tokenDto;
        }



        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        protected void SetCookies(CookieKeys key, string value, int minutes = 30)
        {
            SetCookies(key + "", value, minutes);
        }


        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        protected void DeleteCookies(CookieKeys key)
        {
            DeleteCookies(key + "");
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }

        protected string GetCookies(CookieKeys key)
        {
            return GetCookies(key + "");
        }


        [HttpGet]
        [SkipGlobalActionFilter]
        public IActionResult ErrorMsg(string msg)
        {
            ViewBag.ErrorMsg = msg;
            return View("ErrorMsg");
        }

    }
}
