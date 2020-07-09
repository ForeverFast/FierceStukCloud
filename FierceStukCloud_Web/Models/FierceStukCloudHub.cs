using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FierceStukCloud_Web.Models
{
    [Authorize]
    public class FierceStukCloudHub : Hub
    {

    }
}
