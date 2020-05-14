using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ZLServerDashboard;
using ZLServerDashboard.Hubs;
// using ZhiHuReptile.Web.Hubs;

namespace ZLServerDashboard.Commons
{
    public class ConsoleHelper
    {
        public static bool ShowInfo = true;
        public static bool ShowSuccess = true;
        public static bool ShowFailed = true;
        public static bool ShowWarning = true;

        private static ILogger<ConsoleHelper> logger = Program.GetLogger<ConsoleHelper>();
        private static IHubContext<WebHub> webHub = (IHubContext<WebHub>)Startup.Instance.App.ApplicationServices.GetService(typeof(IHubContext<WebHub>));
        public static void Info(string msg)
        {
            if (!ShowInfo)
                return;
            var message = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg;
            logger.LogDebug(message);
            webHub.Clients.All.SendAsync("WriteLog", 1, message);
        }

        public static void Success(string msg)
        {
            if (!ShowSuccess)
                return;
            var message = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg;
            logger.LogInformation(message);
            webHub.Clients.All.SendAsync("WriteLog", 2, message);
        }

        public static void Warning(string msg)
        {
            if (!ShowWarning)
                return;
            var message = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg;
            logger.LogWarning(message);
            webHub.Clients.All.SendAsync("WriteLog", 3, message);
        }

        public static void Failed(string msg)
        {
            if (!ShowFailed)
                return;
            var message = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + msg;
            logger.LogError(message);
            webHub.Clients.All.SendAsync("WriteLog", 4, message);
        }
    }
}