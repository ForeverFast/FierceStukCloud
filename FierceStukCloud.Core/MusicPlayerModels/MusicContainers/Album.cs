using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class Album : IMusicContainer
    {
        [Column("Id")]
        public string Id { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Author")]
        public string Author { get; set; }
        [Column("UserLogin")]
        [JsonPropertyName("userLogin")]
        public string UserLogin { get; set; }
        public LinkedList<Song> Songs { get; set; }
    }
}
