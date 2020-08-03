using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    [Table("Users")]
    public class User
    {
        [Key]
        [JsonPropertyName("id")]
        public int ID { get; set; }


        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }


        [JsonPropertyName("name")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }


        [JsonPropertyName("statusPC")]
        public string StatusPC { get; set; }
        [JsonPropertyName("accessTokenPC")]
        public string AccessTokenPC { get; set; }
        [JsonPropertyName("connectionIdPC")]
        public string ConnectionIdPC { get; set; }

        [JsonPropertyName("statusPhone")]
        public string StatusPhone { get; set; }
        [JsonPropertyName("accessTokenPhone")]
        public string AccessTokenPhone { get; set; }
        [JsonPropertyName("connectionIdPhone")]
        public string ConnectionIdPhone { get; set; }
    }
}
