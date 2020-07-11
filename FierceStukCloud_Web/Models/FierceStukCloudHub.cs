using FierceStukCloud_Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FierceStukCloud_Web.Models
{
    [Authorize]
    public class FierceStukCloudHub : Hub
    {
        private readonly FierceStukCloudDbContext _context;

        public FierceStukCloudHub(FierceStukCloudDbContext context)
            => _context = context;

        public async Task Send(string Command, params object[] objects)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == Context.User.Identity.Name);

            if (Context.ConnectionId == user.ConnectionIdPC)
            {
                await Clients.Client(user.ConnectionIdPhone).SendAsync("MessageFromPC", Command, objects);
            }
            else if (Context.ConnectionId == user.ConnectionIdPhone)
            {
                await Clients.Client(user.ConnectionIdPC).SendAsync("MessageFromPhone", Command, objects);
            }
            else
            {
                
            }       
        }


        public override Task OnConnectedAsync()
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == Context.User.Identity.Name);
            if (Context.User.FindFirst("Device").Value == "PC")
            { 
                user.ConnectionIdPC = Context.ConnectionId;
                user.StatusPC = "Online";        
            }
            else
            { 
                user.ConnectionIdPhone = Context.ConnectionId;
                user.StatusPhone = "Online";
            }

            _context.SaveChanges();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == Context.User.Identity.Name);
            if (Context.User.FindFirst("Device").Value == "PC")
            {
                user.ConnectionIdPC = "";
                user.AccessTokenPC = "";
                user.StatusPC = "Offline";
            }
            else
            {
                user.ConnectionIdPhone = "";
                user.AccessTokenPhone = "";
                user.StatusPhone = "Offline";
            }

            _context.SaveChanges();

            return base.OnDisconnectedAsync(exception);
        }
    }
}
