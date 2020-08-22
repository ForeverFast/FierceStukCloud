using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.Extension;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class LocalFolder : IMusicContainer, IStatusExistence
    {
        [Column("Id")]
        public string Id { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("LocalUrl")]
        public string LocalUrl { get; set; }
        public ObservableLinkedList<Song> Songs { get; set; }

        [Column("UserLogin")]
        [JsonPropertyName("userLogin")]
        public string UserLogin { get; set; }
        [JsonIgnore]
        [Column("OnServer")]
        public bool OnServer { get; set; }
        [JsonIgnore]
        [Column("OnDevice")]
        public bool OnDevice { get; set; }
    }
}
