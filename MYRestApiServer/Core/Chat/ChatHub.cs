using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MYRestApiServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MYRestApiServer.Core.Chat
{
    [Authorize]
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public static int Count = 0;
        public async Task Send(string UserName,string Message) 
        {
            await this.Clients.Group("Users").SendAsync("Send", UserName , Message);
        }

        public override async Task OnConnectedAsync() 
        {
            string userID = Context.ConnectionId;
            await Groups.AddToGroupAsync(userID, "Users");
            Count++;
            await Clients.Group("Users").SendAsync("Send", string.Empty , $"{userID} has joined. Now Online: {Count}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userID = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(userID, "Users");
            Count--;
            await Clients.Group("Users").SendAsync("Send", string.Empty , $"{userID} has left. Now Online: {Count}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
