using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    public class Author : MusicContainer
    {
        #region Поля
        private string _title;

        private ICollection<Album> _albums;
        #endregion

        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        [Column("Albums")]
        [JsonPropertyName("Albums")]
        public ICollection<Album> Albums { get => _albums; set => SetProperty(ref _albums, value); }

        public override void ExtractDbSongsToSongs()
        {
            
        }

        public Author() : base()
        {
            Albums = new ObservableCollection<Album>();
        }
    }
}
