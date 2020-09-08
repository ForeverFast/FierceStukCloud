using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core;
using FierceStukCloud.Mvvm.Commands;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using FierceStukCloud.Pc.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs
{
    public class PlaylistVM : BasePageViewModel, INavigatedToAware
    {
        #region Свойства

        #region Поля
        private Song _selectedSong;

        #endregion

        public PlayList PlayList { get; set; }

        public ObservableCollection<PlayList> PlayLists { get; set; }


        public Song SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != null)
                {
                    if (_selectedSong != null)
                        _selectedSong.IsSelected = false;
                    value.CurrentMusicContainer = PlayList;
                    value.IsSelected = true;
                    SetProperty(ref _selectedSong, value);
                    _musicPlayerService.CurrentSong = value;
                }
            }
        }

        public bool IsPlaying
        {
            get => _musicPlayerService.IsPlaying;
            set => _musicPlayerService.IsPlaying = value;
        }

        #endregion

        #region Команды - Управление плеером

        public ICommand PlayStateSongCommand { get; private set; }

        public ICommand ListenPlaylistsCommand { get; private set; }

        private void PlayStateSongExecute(object parameter)
        {
            if (_musicPlayerService.IsPlaying == true)
                _musicPlayerService.Pause();
            else
                _musicPlayerService.Play();
        }

        private void ListenPlaylistsExecute(object parameter)
        {
           
            SelectedSong = PlayList.Songs?.First?.Value;
        }

        #endregion

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

        #region События

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
                            if(SelectedSong != null)
                                SelectedSong.IsSelected = false;
                            temp.IsSelected = true;
                            _selectedSong = temp;
                            OnPropertyChanged("SelectedSong");
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

        #endregion

        #region Конструкторы

        public PlaylistVM(PlayList playList)
        {
            PlayList = playList;

            if (playList.Songs == null)
                playList.Songs = new Core.Extension.ObservableLinkedList<Song>();

            playList.Songs.CollectionChanged += Songs_CollectionChanged;
            InitiailizeCommands();
        }

        private void Songs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var q = Thread.CurrentThread.ManagedThreadId;
        }

        public override void InitiailizeCommands()
        {
            PlayStateSongCommand = new RelayCommand(PlayStateSongExecute);
            ListenPlaylistsCommand = new RelayCommand(ListenPlaylistsExecute);

            GoToAuthorPageCommand = new RelayCommand(GoToAuthorPageExecute);
            GoToAlbumPageCommand = new RelayCommand(GoToAlbumPageExecute);
            ShowDetailsCommand = new RelayCommand(ShowDetailsExecute);

            AddToFavouritesCommand = new AsyncRelayCommand(AddToFavouritesExecute);
            AddToAnotherPlaylistCommand = new AsyncRelayCommand(AddToAnotherPlaylistExecute);
            RemoveFromPlaylistCommand = new AsyncRelayCommand(RemoveFromPlaylistExecute);

            AddSongFromDeviceCommand = new AsyncRelayCommand(AddSongFromDeviceExecute);
        }

        public void OnNavigatedTo(params object[] args)
        {
            if (PlayLists == null)
                PlayLists = args[0] as ObservableCollection<PlayList>;
            if (_dialogService == null)
                _dialogService = args[1] as DialogService;
            if (_musicPlayerService == null)
            { 
                _musicPlayerService = args[2] as MusicPlayerService;
                _musicPlayerService.PropertyChanged += _musicPlayer_PropertyChanged;
            }
        }

        #endregion
    }
}
