using FierceStukCloud.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    [Table("Songs")]
    public abstract class SongBase : OnPropertyChangedClass, IBaseObject, IStatusExistence
    {
        [Key]
        [Column("Id")]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [Column("Author")]
        [JsonPropertyName("Author")]
        public string Author { get; set; }

        [Column("Album")]
        [JsonPropertyName("Album")]
        public string Album { get; set; }

        [Column("Year")]
        [JsonPropertyName("Year")]
        public uint Year { get; set; }


        [Column("Duration")]
        [JsonPropertyName("Duration")]
        public string Duration { get; set; }

        [Column("LocalUrl")]
        [JsonPropertyName("LocalUrl")]
        public string LocalUrl { get; set; }

        [Column("PlayLists")]
        [JsonPropertyName("PlayLists")]
        public List<string> PlayLists { get; set; }


        [Column("UserLogin")]
        [JsonPropertyName("UserLogin")]
        public string UserLogin { get; set; }

        [Column("OnServer")]
        [JsonPropertyName("OnServer")]
        public bool OnServer { get; set; }

        [Column("OnDevice")]
        [JsonPropertyName("OnDevice")]
        public bool OnDevice { get; set; }


        [Column("OptionalInfo")]
        [JsonPropertyName("OptionalInfo")]
        public string OptionalInfo { get; set; }


        #region Локальные свойства и методы для работы приложения

        #region Поля
        private bool _isFavorite;
        #endregion

        [Column("Favorite")]
        public bool IsFavorite { get => _isFavorite; set => SetProperty(ref _isFavorite, value); }

        #endregion
    }
}
