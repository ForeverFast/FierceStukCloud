using FierceStukCloud.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    public class LocalFolder : MusicContainer, IStatusExistence
    {
        #region Поля
        private string _title;
        private string _localUrl;

        private string _userLogin;
        private bool _onServer;
        private bool _onDevice;
        #endregion


        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        [Column("LocalUrl")]
        [JsonPropertyName("LocalUrl")]
        public string LocalUrl { get => _localUrl; set => SetProperty(ref _localUrl, value); }

        [Column("UserLogin")]
        [JsonPropertyName("UserLogin")]
        public string UserLogin { get => _userLogin; set => SetProperty(ref _userLogin, value); }

        [Column("OnServer")]
        [JsonPropertyName("OnServer")]
        public bool OnServer { get => _onServer; set => SetProperty(ref _onServer, value); }

        [Column("OnDevice")]
        [JsonPropertyName("OnDevice")]
        public bool OnDevice { get => _onDevice; set => SetProperty(ref _onDevice, value); }

        public override void ExtractDbSongsToSongs()
        {
           
        }

        public LocalFolder() : base()
        { }
    }
}
