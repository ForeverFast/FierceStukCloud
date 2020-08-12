using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Mvvm.Commands;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs
{
    public class PlaylistVM : BaseViewModel
    {
        public PlayList PlayList { get; set; }

        public ObservableCollection<PlayList> PlayLists { get; set; }

        #region Команды 

        
        public ICommand GoToAuthorPageCommand { get; set; }

        public ICommand GoToAlbumPageCommand { get; set; }

        public ICommand ShowDetailsCommand { get; set; }


        public void GoToAuthorPageExecute(object parameter)
           => _navigationManager.Navigate(parameter.ToString(), NavigateType.Default);

        public void GoToAlbumPageExecute(object parameter)
           => _navigationManager.Navigate(parameter.ToString(), NavigateType.Default);

        public void ShowDetailsExecute(object parameter)
        {

        }


        #region Команды - добавление/удаление

        public ICommand AddToFavouritesCommand { get; set; }

        public ICommand AddToAnotherPlaylistCommand { get; set; }

        public ICommand RemoveFromPlaylistCommand { get; set; }


        public async Task AddToFavouritesExecute(object parameter)
        {
           
        }

        public async Task AddToAnotherPlaylistExecute(object parameter)
        {

        }

        public async Task RemoveFromPlaylistExecute(object parameter)
        {

        }


        #endregion

        #endregion


        #region Конструкторы

        public PlaylistVM(PlayList playList)
        {
            PlayList = playList;
            InitiailizeCommands();
        }

        public override void InitiailizeCommands()
        {
            GoToAuthorPageCommand = new RelayCommand(GoToAuthorPageExecute);
            GoToAlbumPageCommand = new RelayCommand(GoToAlbumPageExecute);
            ShowDetailsCommand = new RelayCommand(ShowDetailsExecute);

            AddToFavouritesCommand = new AsyncRelayCommand(AddToFavouritesExecute);
            AddToAnotherPlaylistCommand = new AsyncRelayCommand(AddToAnotherPlaylistExecute);
            RemoveFromPlaylistCommand = new AsyncRelayCommand(RemoveFromPlaylistExecute);
        }

        #endregion
    }
}
