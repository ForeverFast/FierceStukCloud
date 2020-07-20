﻿using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace FierceStukCloud_NetStandardLib.Models
{
    [Table("Songs")]
    public class Song : BaseMusicObject
    {
        [JsonPropertyName("localId")]
        public int LocalID { get; set; } = 0;

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("album")]
        public Album Album { get; set; }

        [JsonPropertyName("year")]
        public uint Year { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }
     
        

        [JsonPropertyName("LocalURL")]
        public string LocalUrl { get; set; }

        [JsonPropertyName("playListNames")]
        public List<string> PlayLists { get; set; }



        [JsonPropertyName("optionalInfo")]
        public string OptionalInfo { get; set; }

        #region Локальные свойства для работы приложения
        [JsonIgnore]
        public MusicContainer CurrentMusicContainer { get; set; }

       

        [JsonIgnore]
        public List<PlayList> PlayLists { get; set; }

        [JsonIgnore]
        public Album OAlbum { get; set; }

        
        #endregion
    }
}