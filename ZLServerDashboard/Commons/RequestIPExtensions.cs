using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZhiHuReptile.Web.Commons
{
    public static class RequestIPExtensions

    {

        public static IApplicationBuilder UseWatchNoFound(this IApplicationBuilder builder)

        {

            return builder.UseMiddleware<WatchNoFoundMiddleWare>();

        }

    }

    public class WatchNoFoundMiddleWare

    {

        private readonly RequestDelegate _next;

        private readonly ILogger logger;

        public WatchNoFoundMiddleWare(RequestDelegate next, ILoggerFactory loggerFactory)

        {

            _next = next;

            logger = loggerFactory.CreateLogger<WatchNoFoundMiddleWare>();

        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context).ConfigureAwait(true);
            var path = context.Request.Path.ToString().Trim();
            if (path.StartsWith("/job/"))
            {//查看招聘信息
                var salt = path.Replace("/job/", "");
                if (Regex.IsMatch(salt, @"^[a-zA-Z0-9]{5}$"))
                {
                    salt = salt.ToLower();
                    context.Response.Redirect("/Public/Job?c=" + salt, true);
                }
            }
            else if (path.StartsWith("/njob/"))
            {//查看招聘信息
                var salt = path.Replace("/njob/", "");
                if (Regex.IsMatch(salt, @"^[a-zA-Z0-9]{5}$"))
                {
                    salt = salt.ToLower();
                    context.Response.Redirect("/Public/NJob?c=" + salt, true);
                }
            }
            else if (path.StartsWith("/nct/"))
            {//选择面试时间
                var salt = path.Replace("/nct/", "");
                //if (Regex.IsMatch(salt, @"^[a-zA-Z0-9]{5}$"))
                //{
                    salt = salt.ToLower();
                    context.Response.Redirect("/Public/nct?c=" + salt, true);
                //}
            }
            else if (path.StartsWith("/ct/"))
            {//选择面试时间
                var salt = path.Replace("/ct/", "");
                if (Regex.IsMatch(salt, @"^[a-zA-Z0-9]{5}$"))
                {
                    salt = salt.ToLower();
                    context.Response.Redirect("/Public/ct?c=" + salt, true);
                }
            }
            else if (path.StartsWith("/mt/"))
            {//参与会议或面试
                var salt = path.Replace("/mt/", "");
                if (Regex.IsMatch(salt, @"^[a-zA-Z0-9]{5}$"))
                {
                    salt = salt.ToLower();
                    context.Response.Redirect("/Meet/Join?c=" + salt, true);
                }
            }
        }
    }
}
