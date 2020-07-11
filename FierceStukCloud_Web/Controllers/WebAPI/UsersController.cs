using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_Web.Data;
using FierceStukCloud_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace FierceStukCloud_Web.Controllers.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FierceStukCloudDbContext _context;

        public UsersController(FierceStukCloudDbContext context)
            => _context = context;
        
        private int CodeA;

        [HttpPost("/api/Authentication")]
        public IActionResult Token([FromHeader] string username, [FromHeader] string password, [FromHeader] string device)
        {
            //username = "ForeverFast"; password = "789xxx44XX"; 
            var identity = GetIdentity(username, password, device);
            if (identity == null)
            {
                return BadRequest(CodeA);
            }

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: DateTime.UtcNow,
                    claims: identity.Claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //var Login = identity.FindFirst("Login").Value;
            //var ID = identity.FindFirst("ID").Value;
            //var Password = identity.FindFirst("Password").Value;
            //var LastName = identity.FindFirst("LastName").Value;
            //var Status = identity.FindFirst("Status").Value;
            //var Role = identity.FindFirst("Role").Value;

            switch (device)
            {
                case "PC":
                    var temp = new
                    {
                        ID = Convert.ToInt32(identity.FindFirst("ID").Value),
                        Login = identity.Name,
                        Password = identity.FindFirst("Password").Value,

                        FirstName = identity.FindFirst("FirstName").Value,
                        LastName = identity.FindFirst("LastName").Value,

                        StatusPC = "Online",
                        AccessTokenPC = encodedJwt
                    };
                    return new JsonResult(temp);

                case "Phone":

                    return new JsonResult(new
                    {
                        ID = Convert.ToInt32(identity.FindFirst("ID").Value),
                        Login = identity.Name,
                        Password = identity.FindFirst("Password").Value,

                        FirstName = identity.FindFirst("FirstName").Value,
                        LastName = identity.FindFirst("LastName").Value,

                        StatusPhone = "Online",
                        AccessTokenPhone = encodedJwt
                    });
            }

            return NoContent();
        }

        private ClaimsIdentity GetIdentity(string username, string password, string device)
        {
            User user = _context.Users.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("ID", user.ID.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim("Password", user.Password),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)    
                };

                if (device == "PC")
                    claims.Add(new Claim("Device", "PC"));
                else
                    claims.Add(new Claim("Device", "Phone"));

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }
            else
            {
                if (_context.Users.FirstOrDefault(x => x.Login == username) != null)
                    CodeA = 151;
                else
                    CodeA = 152;
                return null;
            }
        }

        
    }
}
