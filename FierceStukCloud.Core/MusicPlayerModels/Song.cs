﻿using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    public class Song : BaseObject, IStatusExistence
    {
        #region Поля
        private string _title;
        private ICollection<Author> _authors;
        private ICollection<Album> _albums;
        private uint _year;

        private TimeSpan _duration;
        private string _localUrl;
        private LocalFolder _localFolder;

        private string _userLogin;
        private bool _onServer;
        private bool _onDevice;

        private string _optionalInfo;

        private bool _isCurrentSong;
        private bool _isPlaying;
        private bool _isFavorite;
        #endregion

        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get => _title; set => SetProperty(ref _title, value); }

      
        [Column("Authors")]
        [JsonPropertyName("Authors")]
        public ICollection<Author> Authors { get => _authors; set => SetProperty(ref _authors, value); }

        [Column("Albums")]
        [JsonPropertyName("Albums")]
        public ICollection<Album> Albums { get => _albums; set => SetProperty(ref _albums, value); }

        [Column("Year")]
        [JsonPropertyName("Year")]
        public uint Year { get => _year; set => SetProperty(ref _year, value); }


        [Column("Duration")]
        [DefaultValue(0)]
        [JsonPropertyName("Duration")]
        public TimeSpan Duration { get => _duration; set => SetProperty(ref _duration, value); } 

        [Column("LocalUrl")]
        [JsonPropertyName("LocalUrl")]
        public string LocalUrl { get => _localUrl; set => SetProperty(ref _localUrl, value); }

        [Column("LocalFolder")]
        [JsonPropertyName("LocalFolder")]
        public LocalFolder LocalFolder { get => _localFolder; set => SetProperty(ref _localFolder, value); }



        [Column("UserLogin")]
        [JsonPropertyName("UserLogin")]
        public string UserLogin { get => _userLogin; set => SetProperty(ref _userLogin, value); }

        [Column("OnServer")]
        [JsonPropertyName("OnServer")]
        public bool OnServer { get => _onServer; set => SetProperty(ref _onServer, value); }

        [Column("OnDevice")]
        [JsonPropertyName("OnDevice")]
        public bool OnDevice { get => _onDevice; set => SetProperty(ref _onDevice, value); }


        [Column("OptionalInfo")]
        [JsonPropertyName("OptionalInfo")]
        public string OptionalInfo { get => _optionalInfo; set => SetProperty(ref _optionalInfo, value); }


        #region Cвойства и методы для работы приложения

        [NotMapped]
        public bool IsCurrentSong { get => _isCurrentSong; set => SetProperty(ref _isCurrentSong, value); }

        [NotMapped]
        public bool IsPlaying { get => _isPlaying; set => SetProperty(ref _isPlaying, value); }

        [Column("Favorite")]
        public bool IsFavorite { get => _isFavorite; set => SetProperty(ref _isFavorite, value); }

        [JsonIgnore]
        [NotMapped]
        public MusicContainer CurrentMusicContainer { get; set; }
        [JsonIgnore]
        [NotMapped]
        public IMusicPlayerService MusicPlayer { get; set; }

        #endregion

        public Song()
        {
            Albums = new ObservableCollection<Album>();
            Authors = new ObservableCollection<Author>();
        }

    }
}
