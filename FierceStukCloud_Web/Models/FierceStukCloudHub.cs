using FierceStukCloud.Core;
using FierceStukCloud_Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static FierceStukCloud.Core.CustomEnums;

namespace FierceStukCloud_Web.Models
{
    [Authorize]
    public class FierceStukCloudHub : Hub
    {
        private readonly FierceStukCloudDbContext _context;
        private readonly ILogger<FierceStukCloudHub> _logger;
        public FierceStukCloudHub(FierceStukCloudDbContext context, ILogger<FierceStukCloudHub> logger)
        { 
            _context = context;
            _logger = logger;
        }
        public string GetTargetConnectionId(DeviceType deviceTo)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == Context.User.Identity.Name);
           
            switch (deviceTo)
            {
                case DeviceType.PC:
                    return user.ConnectionIdPC;
                case DeviceType.Mobile:
                    return user.ConnectionIdPhone;
                case DeviceType.TV:
                    return "";
                case DeviceType.Web:
                    return "";
                default:
                    return "";
            }
        }

        public async Task MusicPlayerCommand(DeviceType deviceFrom, DeviceType deviceTo, Commands command)
            => await Clients.Clients(GetTargetConnectionId(deviceTo)).SendAsync("Commands", deviceFrom, command);

        public async Task SetCurrentSongCommand(DeviceType deviceFrom, DeviceType deviceTo, Song song)
            => await Clients.Clients(GetTargetConnectionId(deviceTo)).SendAsync("SetCurrentSong", deviceFrom, song);

        public async Task NewCurrentSongCommand(DeviceType deviceFrom, DeviceType deviceTo, Song song)
          => await Clients.Clients(GetTargetConnectionId(deviceTo)).SendAsync("NewCurrentSong", deviceFrom, song);

        public async Task SendSongsCommand(DeviceType deviceFrom, DeviceType deviceTo, string json1, string json2)
         => await Clients.Clients(GetTargetConnectionId(deviceTo)).SendAsync("SendSongs", deviceFrom, json1, json2);

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
