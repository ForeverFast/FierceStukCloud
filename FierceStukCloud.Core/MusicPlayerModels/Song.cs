using FierceStukCloud.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core.MusicPlayerModels
{
    [Table("Songs")]
    public class Song : IBaseObject, IStatusExistence
    {
        [Column("Id")]
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [Column("LocalId")]
        [JsonPropertyName("localId")]
        public List<KeyValuePair<IMusicContainer, int>> LocalId { get; set; } = new List<KeyValuePair<IMusicContainer, int>>();

        [Column("Title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Column("Author")]
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [Column("Album")]
        [JsonPropertyName("album")]
        public string Album { get; set; }

        [Column("Year")]
        [JsonPropertyName("year")]
        public uint Year { get; set; }


        [Column("Duration")]
        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [Column("LocalUrl")]
        [JsonPropertyName("localUrl")]
        public string LocalUrl { get; set; }

        [Column("PlayLists")]
        [JsonPropertyName("playLists")]
        public List<string> PlayLists { get; set; } = new List<string>();


        [Column("UserLogin")]
        [JsonPropertyName("userLogin")]
        public string UserLogin { get; set; }

        [Column("OnServer")]
        [JsonPropertyName("onServer")]
        public bool OnServer { get; set; }

        [Column("OnPC")]
        [JsonPropertyName("onPC")]
        public bool OnPC { get; set; }

        [Column("OnPhone")]
        [JsonPropertyName("onPhone")]
        public bool OnPhone { get; set; }

        [Column("OptionalInfo")]
        [JsonPropertyName("optionalInfo")]
        public string OptionalInfo { get; set; }



        #region Локальные свойства и методы для работы приложения

        [JsonIgnore]
        public IMusicContainer CurrentMusicContainer { get; set; }
       
        public int CurrentIdValue()
            => LocalId.Find(x => x.Key == CurrentMusicContainer).Value;

        public int IdValueInMC(IMusicContainer musicContainer)
            => LocalId.Find(x => x.Key == musicContainer).Value;
        
        #endregion

    }
}
