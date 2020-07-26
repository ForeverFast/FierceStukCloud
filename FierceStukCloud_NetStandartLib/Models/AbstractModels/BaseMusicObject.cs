using FierceStukCloud_NetStandardLib.MVVM;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FierceStukCloud_NetStandardLib.Models.AbstractModels
{
    public abstract class BaseMusicObject : OnPropertyChangedClass
    {
        [Key]
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }


        [JsonPropertyName("userLogin")]
        public string UserLogin { get; set; }


        [JsonIgnore]
        public bool OnServer { get; set; }

        [JsonIgnore]
        public bool OnPC { get; set; }
    }
}
