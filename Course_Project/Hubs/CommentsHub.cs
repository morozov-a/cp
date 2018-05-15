using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Hubs
{
    public class CommentsHub : Hub
    {
        public async Task Update()
        {
            await this.Clients.All.SendAsync("Update");
        }
    }
}
