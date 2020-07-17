using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FierceStukCloud_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FierceStukCloud_Web.Controllers
{
    public class HomeController : Controller
    {
        IHubContext<FierceStukCloudHub> _hubContext;
        public HomeController(IHubContext<FierceStukCloudHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
