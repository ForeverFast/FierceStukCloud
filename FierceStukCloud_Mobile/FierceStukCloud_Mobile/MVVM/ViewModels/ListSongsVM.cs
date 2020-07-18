using FierceStukCloud_Mobile.Models;
using FierceStukCloud_Mobile.MVVM.ViewModels.AbstractVM;
using FierceStukCloud_Mobile.ViewModels;
using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FierceStukCloud_Mobile.MVVM.ViewModels
{
    public class ListSongsVM : BaseListSongsVM
    {
        public MusicContainer MusicContainer { get; set; }

        public ListSongsVM()
        {

        }

        public ListSongsVM(MusicContainer MusicContainer) : this()
        {
            this.MusicContainer = MusicContainer;

            Songs = new ObservableCollection<Song>(MusicContainer.Songs);
            //Songs.Clear();
            //if (MusicContainer != null)
            //    foreach (var item in )
            //        Songs.Add(item);
        }
    }
}
