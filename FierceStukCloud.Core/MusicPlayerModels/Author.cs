using FierceStukCloud.Abstractions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FierceStukCloud.Core
{
    public class Author : BaseObject
    {
        #region Поля
        private string _title;

        private ObservableCollection<Album> _albums;
        #endregion

        [Column("Title")]
        [JsonPropertyName("Title")]
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        [Column("Albums")]
        [JsonPropertyName("Albums")]
        public ObservableCollection<Album> Albums { get => _albums; set => SetProperty(ref _albums, value); }

    }
}
