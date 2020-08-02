using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Abstractions
{
    public interface IBaseObject
    {
        [Key]
        [JsonPropertyName("id")]
        int Id { get; set; }

        string Title { get; set; }
    }
}
