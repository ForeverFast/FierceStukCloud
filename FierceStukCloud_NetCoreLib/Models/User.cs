using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud_NetCoreLib.Models
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
        public string Name { get; set; }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }


        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }



        [JsonPropertyName("accessTokenPC")]
        public string AccessTokenPC { get; set; }
        [JsonPropertyName("statePC")]
        public string StatePC { get; set; }


        [JsonPropertyName("accessTokenWebSite")]
        public string AccessTokenWebSite { get; set; }
        [JsonPropertyName("stateWebSite")]
        public string StateWebSite { get; set; }
    }
}
