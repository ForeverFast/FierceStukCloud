using FierceStukCloud.Abstractions;
using FierceStukCloud.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core.MusicPlayerModels
{
    public abstract class SongBase : OnPropertyChangedClass, IBaseObject, IStatusExistence
    {
        [Column("Id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

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
        public List<string> PlayLists { get; set; }


        [Column("UserLogin")]
        [JsonPropertyName("userLogin")]
        public string UserLogin { get; set; }

        [Column("OnServer")]
        [JsonPropertyName("onServer")]
        public bool OnServer { get; set; }

        [Column("OnPC")]
        [JsonPropertyName("onPC")]
        public bool OnDevice { get; set; }


        [Column("OptionalInfo")]
        [JsonPropertyName("optionalInfo")]
        public string OptionalInfo { get; set; }


        #region Локальные свойства и методы для работы приложения

        #region Поля
        private bool _isSelected;
        #endregion

        [JsonIgnore]
        public IMusicContainer CurrentMusicContainer { get; set; }

        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }

        #endregion
    }
}
