using FierceStukCloud_NetStandardLib.Models.AbstractModels;
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
        [Column("Id")]
        [JsonPropertyName("Id")]
        public int Id { get; set; } = 0;

        [Column("localId")]
        [JsonPropertyName("localId")]
        public List<KeyValuePair<MusicContainer, int>> LocalId { get; set; } = new List<KeyValuePair<MusicContainer, int>>();

        [Column("author")]
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [Column("album")]
        [JsonPropertyName("album")]
        public string Album { get; set; }

        [Column("year")]
        [JsonPropertyName("year")]
        public uint Year { get; set; }


        [Column("duration")]
        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [Column("localUrl")]
        [JsonPropertyName("localUrl")]
        public string LocalUrl { get; set; }

        [Column("playLists")]
        [JsonPropertyName("playLists")]
        public List<string> PlayLists { get; set; } = new List<string>();

        [Column("optionalInfo")]
        [JsonPropertyName("optionalInfo")]
        public string OptionalInfo { get; set; }






        #region Локальные свойства для работы приложения

       

        [JsonIgnore]
        public MusicContainer CurrentMusicContainer { get; set; }



        #endregion

        #region Локальные методы для работы приложения

        public int CurrentIdValue()
            => LocalId.Find(x => x.Key == CurrentMusicContainer).Value;

        public int IdValueInMC(MusicContainer musicContainer)
            => LocalId.Find(x => x.Key == musicContainer).Value;
        

        #endregion

    }
}
