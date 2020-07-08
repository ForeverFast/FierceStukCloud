using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace FierceStukCloud_Web.Controllers.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly FierceStukCloudDbContext _context;

        public SongsController(FierceStukCloudDbContext context)
            => _context = context;
        

        [Authorize]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }

        [Authorize(Roles = "admin")]
        [Route("getrole")]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }

    }
}
