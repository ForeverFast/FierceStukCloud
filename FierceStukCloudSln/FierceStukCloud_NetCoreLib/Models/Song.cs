using FierceStukCloud_NetCoreLib.Models.MusicContainers;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace FierceStukCloud_NetCoreLib.Models
{
    [Table("Songs")]
    public class Song
    {
        [Key]
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("localId")]
        public int LocalID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }
      

        [JsonPropertyName("userLogin")]
        public string UserLogin { get; set; }

        [JsonPropertyName("playListName")]
        public string PlayListName { get; set; }


        //public ImageAsync Image { get; set; }

        [JsonIgnore]
        public PlayList PlayList { get; set; }

        [JsonIgnore]
        public string LocalURL { get; set; }




        private TimeSpan _Duration;
    }
}
