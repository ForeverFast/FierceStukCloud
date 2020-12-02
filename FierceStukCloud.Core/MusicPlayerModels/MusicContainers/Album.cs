using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    public class Album : MusicContainer
    {
        #region Поля
        private string _title;
        private ObservableCollection<Author> _authors;
        private uint _year;

        private string _userLogin;
        #endregion


        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        [Column("Authors")]
        [JsonPropertyName("Authors")]
        public ObservableCollection<Author> Authors { get => _authors; set => SetProperty(ref _authors, value); }
        [Column("Year")]
        [JsonPropertyName("Year")]
        public uint Year { get => _year; set => SetProperty(ref _year, value); }

        [Column("UserLogin")]
        [JsonPropertyName("UserLogin")]
        public string UserLogin { get => _userLogin; set => SetProperty(ref _userLogin, value); }

        public override void ExtractDbSongsToSongs()
        {
            
        }

        public void ExtractDbAuthorsToAuthors()
        {

        }

        public Album() : base()
        {
            Authors = new ObservableCollection<Author>();
        }
    }
}
