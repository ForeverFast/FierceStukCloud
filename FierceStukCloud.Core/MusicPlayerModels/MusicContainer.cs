using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.Extension;
using FierceStukCloud.Core.Extension.ManyToMany;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FierceStukCloud.Core
{
    public abstract class MusicContainer : BaseObject
    {
        #region Поля
        private ObservableLinkedList<Song> _songs;

        #endregion

        [NotMapped]
        public ObservableLinkedList<Song> Songs { get => _songs; set => SetProperty(ref _songs, value); }

   
        public abstract void ExtractDbSongsToSongs();

        public MusicContainer()
        {
           
        }
    }
}
