using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ZLServerDashboard.Commons;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Hubs
{
    public class WebHub : Hub
    {

        public override Task OnConnectedAsync()
        {
            if (!Context.GetHttpContext().Request.Cookies.ContainsKey("WebToken"))
                Context.Abort();
            var token = Context.GetHttpContext().Request.Cookies["WebToken"];
            if (string.IsNullOrWhiteSpace(token) || !Tools.ValidateToken(token, LoginPlatform.Web))
            {
                Context.Abort();
            }
            return base.OnConnectedAsync();
        }

        public async Task WriteLog(int type, string message)
        {
            await Clients.All.SendAsync("WriteLog", type, message);
        }
    }
}