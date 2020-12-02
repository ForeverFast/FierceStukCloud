using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.Extension;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FierceStukCloud.Core
{
    public abstract class MusicContainer : BaseObject
    {
        #region Поля
        private ICollection<Song> _songs;

        #endregion

        [Column("Songs")]
        public ICollection<Song> Songs { get => _songs; set => SetProperty(ref _songs, value); }

        public abstract void ExtractDbSongsToSongs();

        public MusicContainer()
        {
            Songs = new ObservableLinkedList<Song>();       
        }
    }
}
