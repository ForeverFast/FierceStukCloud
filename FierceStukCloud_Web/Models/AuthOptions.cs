using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FierceStukCloud_Web.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "FierceStukCloud_WebAPI"; // издатель токена
        public const string AUDIENCE = "FierceStukCloud_Client"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
