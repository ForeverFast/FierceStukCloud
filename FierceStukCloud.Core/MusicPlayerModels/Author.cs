using FierceStukCloud.Core.Extension.ManyToMany;
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

        private ObservableCollection<Album> _albums;
        #endregion

        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        [NotMapped]
        [JsonPropertyName("Albums")]
        public ObservableCollection<Album> Albums { get => _albums; set => SetProperty(ref _albums, value); }


        public List<SongAuthor> DbSongs { get; set; }
        public List<AlbumAuthor> DbAlbums { get; set; }

        public override void ExtractDbSongsToSongs()
        {
            
        }

        public Author() : base()
        {
            DbSongs = new List<SongAuthor>();
            DbAlbums = new List<AlbumAuthor>();
        }
    }
}
