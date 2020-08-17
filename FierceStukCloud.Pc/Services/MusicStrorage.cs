using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Core.Services;
using System.Collections.ObjectModel;

namespace FierceStukCloud.Pc.Services
{
    public class MusicStrorage : IMusicStorage
    {
        public ObservableCollection<Song> AllSongs { get; set; }
        public PlayList Favourites { get; set; } = new PlayList() { Title = "Favourites" };
        public ObservableCollection<Album> Albums { get; set; } = new ObservableCollection<Album>();
        public ObservableCollection<LocalFolder> LocalFolders { get; set; } = new ObservableCollection<LocalFolder>();
        public ObservableCollection<PlayList> PlayLists { get; set; } = new ObservableCollection<PlayList>();
    }
}
