using FierceStukCloud.Core.Extension;
using FierceStukCloud.Core.Extension.ManyToMany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    public class PlayList : MusicContainer
    {
        #region Поля
        private string _title;
        private string _description;
        private string _imageUrl;
        private DateTime _creationDate;
        private string _userLogin;
        private bool _onServer;
        private bool _onDevice;
        #endregion


        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        [Column("Description")]
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        [Column("Image")]
        public string ImageUrl { get => _imageUrl; set => SetProperty(ref _imageUrl, value); }

        [Column("CreationDate")]
        public DateTime CreationDate { get => _creationDate; set => SetProperty(ref _creationDate, value); }

        [Column("UserLogin")]
        [JsonPropertyName("UserLogin")]
        public string UserLogin { get => _userLogin; set => SetProperty(ref _userLogin, value); }

        [Column("OnServer")]
        [JsonPropertyName("OnServer")]
        public bool OnServer { get => _onServer; set => SetProperty(ref _onServer, value); }

        [Column("OnDevice")]
        [JsonPropertyName("OnDevice")]
        public bool OnDevice { get => _onDevice; set => SetProperty(ref _onDevice, value); }


        public List<SongPlayList> DbSongs { get; set; }

        public override void ExtractDbSongsToSongs()
        {
            
        }

        public PlayList() : base()
        {
            DbSongs = new List<SongPlayList>();
        }

    }
}
