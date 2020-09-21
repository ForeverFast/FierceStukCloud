using FierceStukCloud.Core.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
   
    public class Song : SongBase
    {

        #region Локальные свойства и методы для работы приложения

        #region Поля
        private bool _isCurrentSong;
        private bool _isPlaying;
        #endregion

        [JsonIgnore]
        [NotMapped]
        public IMusicContainer CurrentMusicContainer { get; set; }
        [JsonIgnore]
        [NotMapped]
        public IMusicPlayerService MusicPlayer { get; set; }


        [NotMapped]
        public bool IsCurrentSong { get => _isCurrentSong; set => SetProperty(ref _isCurrentSong, value); }

        [NotMapped]
        public bool IsPlaying { get => _isPlaying; set => SetProperty(ref _isPlaying, value); }



        #endregion

    }
}
