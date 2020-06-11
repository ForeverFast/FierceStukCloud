using FierceStukCloud_NetCoreLib.Models.AbstractModels;
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
    public class Song : BaseMusicObject
    {
        [JsonPropertyName("localId")]
        public int LocalID { get; set; } = 0;

        [JsonPropertyName("author")]
        public string Author { get; set; }

        public string Album { get; set; }
        public uint Year { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }
     



        [JsonPropertyName("playListNames")]
        public string PlayListNames { get; set; }


        #region Локальные свойства для работы приложения
        [JsonIgnore]
        public MusicContainer CurrentMusicContainer { get; set; }

        [JsonIgnore]
        public ImageAsync Image { get; set; }

        [JsonIgnore]
        public List<PlayList> PlayLists { get; set; }

        [JsonIgnore]
        public Album OAlbum { get; set; }

        [JsonIgnore]
        public string LocalURL { get; set; }
        #endregion
    }
}
