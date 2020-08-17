using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Mvvm.Commands;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using FierceStukCloud.Wpf.Services.ImageAsyncS;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs
{
    public class PlaylistVM : BasePageViewModel, INavigatedToAware
    {
        public PlayList PlayList { get; set; }

        public ObservableCollection<PlayList> PlayLists { get; set; }


        private Song _selectedSong;
        public  Song SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != null)
                {
                    _selectedSong.IsSelected = false;
                    value.CurrentMusicContainer = PlayList;
                    value.IsSelected = true;
                    SetProperty(ref _selectedSong, value);
                    _musicPlayerService.CurrentSong = value;
                }
            }
        }

        #region Команды - Контекстное меню песни


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

        #region Команды - работа с файлами


        #region Методы добавления/удаления песен на устройстве

        public ICommand AddSongFromDeviceCommand { get; private set; }
        public ICommand RemoveSongFromDeviceCommand { get; private set; }
        public ICommand RemoveSongFromAppCommand { get; private set; }

        private async Task AddSongFromDeviceExecute(object parameter)
             => await _musicPlayerService.AddSongFromDevice(_dialogService.FileBrowserDialog(), PlayList.Id);




        #endregion


        #region Методы добавления/удаления песен на сервере 

        public ICommand AddSongToServerCommand { get; private set; }
        public ICommand RemoveSongFromServerCommand { get; private set; }

        #endregion

        public ICommand UpdateSongInfo { get; private set; }

        #endregion

        private void _musicPlayer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case nameof(_musicPlayerService.CurrentSong):

                        var temp = _musicPlayerService.CurrentSong;
                        if (temp != SelectedSong)
                        {
                            _selectedSong.IsSelected = false;
                            temp.IsSelected = true;
                            SetProperty(ref _selectedSong, temp);
                        }

                        break;

                    case nameof(_musicPlayerService.Play):


                        break;

                    case nameof(_musicPlayerService.Pause):
                    case nameof(_musicPlayerService.Stop):

                       

                        break;
                }
            }
        }

        #region Конструкторы

        public PlaylistVM(PlayList playList)
        {
            PlayList = playList;
            InitiailizeCommands();
            // _musicPlayer.PropertyChanged += _musicPlayer_PropertyChanged;
            //_musicPlayer 
        }

       

        public override void InitiailizeCommands()
        {
            GoToAuthorPageCommand = new RelayCommand(GoToAuthorPageExecute);
            GoToAlbumPageCommand = new RelayCommand(GoToAlbumPageExecute);
            ShowDetailsCommand = new RelayCommand(ShowDetailsExecute);

            AddToFavouritesCommand = new AsyncRelayCommand(AddToFavouritesExecute);
            AddToAnotherPlaylistCommand = new AsyncRelayCommand(AddToAnotherPlaylistExecute);
            RemoveFromPlaylistCommand = new AsyncRelayCommand(RemoveFromPlaylistExecute);

            AddSongFromDeviceCommand = new AsyncRelayCommand(AddSongFromDeviceExecute);

        }

        public void OnNavigatedTo(object arg)
        {
            PlayLists = arg as ObservableCollection<PlayList>;
        }

        #endregion
    }
}
