using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Abstractions
{
    public interface IBaseObject
    {
        [JsonPropertyName("id")]
        string Id { get; set; }

        string Title { get; set; }
    }
}
