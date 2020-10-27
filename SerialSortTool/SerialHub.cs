using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerialSortTool
{
    public class SerialHub : Hub
    {
        public SerialHub()
        {
        }

        public override Task OnConnectedAsync()
        {
            string group = Context.GetHttpContext().Request.Query["group"];
            if (!string.IsNullOrEmpty(group))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

    }
}
