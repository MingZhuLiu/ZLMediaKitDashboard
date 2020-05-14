using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLServerDashboard.Commons;
using ZLServerDashboard.Interface;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Filters
{
    public class GlobalFiler : Attribute, IAuthorizationFilter
    {
        private IUserService userService;
        public GlobalFiler(IUserService userService)
        {
            this.userService = userService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            var isSkip = false;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isSkip = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(SkipGlobalActionFilterAttribute)));
            }
            if (isSkip) return;

            context.HttpContext.Request.Cookies.TryGetValue(CookieKeys.WebToken + "", out string value);
            if (string.IsNullOrWhiteSpace(value) || !Tools.ValidateToken(value, LoginPlatform.Web))
            {
                RedirectResult result = new RedirectResult("~/Home/Login");
                context.Result = result;
                return;
            }
            //再判断是否有菜单操作权限
            var reqPath = context.HttpContext.Request.Path.ToString();
            if (reqPath != null && reqPath.EndsWith("/"))
                reqPath = reqPath.Substring(0, reqPath.Length - 1);
            if (reqPath == "/Home/Index" || reqPath == "/Home/Dashboard")
                return;
            var user = Tools.GetTokenDto(value);
            if (user == null)
            {
                //未登录  无权
                context.Result = new RedirectResult("/Home/AuthError");
                return;
            }
            var menus = userService.FindUserMenus(userService.FindUserByLoginName(user.LoginAccount), false);
            if (!menus.Where(p =>p.Url!=null&& p.Url.ToLower().Contains(reqPath.ToLower())).Any())
            {
                //已登录 无权
                context.Result = new RedirectResult("/Home/AuthError");
                return;
            }
            else
            {
                //继续
                return;

            }
        }
    }
}
