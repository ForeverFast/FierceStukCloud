using System.Collections.ObjectModel;

namespace FierceStukCloud.Core.Services
{
    public interface IMusicStorage
    {
        ObservableCollection<Song> AllSongs { get; set; }
        PlayList Favourites { get; set; }
        ObservableCollection<Album> Albums { get; set; }
        ObservableCollection<LocalFolder> LocalFolders { get; set; }
        ObservableCollection<PlayList> PlayLists { get; set; }
    }
}
