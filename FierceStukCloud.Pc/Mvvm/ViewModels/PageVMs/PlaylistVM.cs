using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core;
using FierceStukCloud.Mvvm.Commands;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using FierceStukCloud.Pc.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

//FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs

namespace FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs
{
    public class PlaylistVM : BasePageViewModel, INavigatedToAware, INavigatingFromAware
    {
        #region Свойства

        #region Поля
        private Song _SelectedSong;
        #endregion

        public PlayList PlayList { get; set; }

        public ObservableCollection<PlayList> PlayLists { get; set; }

        public Song SelectedSong { get => _SelectedSong; set => SetProperty(ref _SelectedSong, value); }

        public bool IsPlaying
        {
            get => _musicPlayerService.IsPlaying &&
                  _musicPlayerService.CurrentSong.CurrentMusicContainer.Id == this.PlayList.Id;
        }
               
        public bool IsCurrentMusicContainer { get => _musicPlayerService.CurrentMusicContainer == PlayList; }
             

        #endregion

        #region Команды - Управление плеером

        public ICommand ListenPlaylistsCommand { get; private set; }

        private void ListenPlaylistsExecute(object parameter)
            => SetSongExecute(PlayList.Songs?.First?.Value);
        private bool ListenPlaylistsCabExecute(object parameter)
            => PlayList.Songs.Count > 0;

        #endregion


        #region Команды - ListViewItem

        /// <summary>
        /// Запуск песни по дабл клику
        /// </summary>
        public ICommand SetSongCommand { get; private set; }
        private void SetSongExecute(object parameter)
        {
            var value = parameter as Song;
            value.CurrentMusicContainer = PlayList;
            SetProperty(ref _SelectedSong, value);
            _musicPlayerService.CurrentSong = value;
        }

        private bool SetSongCanExecute(object parameter)
            => PlayList.Songs.Find(parameter as Song) != null;

        public ICommand PlayStateSongCommand { get; private set; }
        private void PlayStateSongExecute(object parameter)
        {
            if (_musicPlayerService.IsPlaying == true)
                _musicPlayerService.Pause();
            else
                _musicPlayerService.Play();
        }


        public ICommand AddOrRemoveInFavouritesCommand { get; private set; }

        public async Task AddOrRemoveInFavouritesExecute(object parameter)
        {
            SelectedSong.IsFavorite = !SelectedSong.IsFavorite;
            await _musicPlayerService.UpdateSongInfo(SelectedSong);
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

        #endregion


        #region Команды - работа с песнями

        #region Методы добавления

        public ICommand AddSongFromDeviceCommand { get; private set; }
        public ICommand AddSongToServerCommand { get; private set; }
        public ICommand AddToAnotherPlaylistCommand { get; set; }
   

      
        private async Task AddSongFromDeviceExecute(object parameter)
        {
            await _musicPlayerService.AddSongFromDevice(_dialogService.FileBrowserDialog(), PlayList.Id);
            OnPropertyChanged(nameof(PlayList.Songs.Count));
        }

        public async Task AddToAnotherPlaylistExecute(object parameter)
        {
            
        }

        #endregion

        #region Методы удаления

        public ICommand RemoveFromPlaylistCommand { get; set; }
        public ICommand RemoveSongFromDeviceCommand { get; private set; }
        public ICommand RemoveSongFromAppCommand { get; private set; }
        public ICommand RemoveSongFromServerCommand { get; private set; }

        public async Task RemoveFromPlaylistExecute(object parameter)
        {

        }

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

                        OnPropertyChanged(nameof(IsPlaying));
                        OnPropertyChanged(nameof(IsCurrentMusicContainer));
                        break;

                    case nameof(_musicPlayerService.Play):
                    case nameof(_musicPlayerService.Pause):
                    case nameof(_musicPlayerService.Stop):
                        OnPropertyChanged(nameof(IsPlaying));
                        OnPropertyChanged(nameof(IsCurrentMusicContainer));
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

            //PlayList.Songs.CollectionChanged += (o, e) => { OnPropertyChanged() }

            InitiailizeCommands();
        }

        public override void InitiailizeCommands()
        {
            ListenPlaylistsCommand = new RelayCommand(ListenPlaylistsExecute, ListenPlaylistsCabExecute);

            SetSongCommand = new RelayCommand(SetSongExecute, SetSongCanExecute);
            PlayStateSongCommand = new RelayCommand(PlayStateSongExecute);
            AddOrRemoveInFavouritesCommand = new AsyncRelayCommand(AddOrRemoveInFavouritesExecute);


            GoToAuthorPageCommand = new RelayCommand(GoToAuthorPageExecute);
            GoToAlbumPageCommand = new RelayCommand(GoToAlbumPageExecute);
            ShowDetailsCommand = new RelayCommand(ShowDetailsExecute);

         
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

        public void OnNavigatingFrom()
        {
            
        }

        #endregion
    }
}
