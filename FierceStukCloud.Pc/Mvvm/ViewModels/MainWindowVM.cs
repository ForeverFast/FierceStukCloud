using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Mvvm.Commands;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs;
using FierceStukCloud.Pc.Mvvm.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static FierceStukCloud.Core.CustomEnums;

namespace FierceStukCloud.Pc.Mvvm.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel
    {
      
        #region Свойства - Коллекции/выбранные элементы списков

        #region Поля
        private MusicContainer _selectedMusicContainer;
        private Song _CurrentSong;
        #endregion
            
        public ObservableCollection<PlayList> PlayLists { get; }

        public MusicContainer SelectedMusicContainer { get => _selectedMusicContainer; set => SetProperty(ref _selectedMusicContainer, value); }

        public Song CurrentSong { get => _CurrentSong; set => SetProperty(ref _CurrentSong, value); }
      

        #endregion

        #region Свойства - Отображение информации о текущем треке

        #region Поля
        private string _songTime = "99:99";
        private string _songPos = "00:00";
        private double _songTimeForSlider;
        private double _songTimeLineForSlider;

        private double _songVolumeForSlider;

        private BitmapImage _songImage;
        #endregion

        /// <summary>Текущее время трека</summary>
        public string SongPos { get => _songPos; set => SetProperty(ref _songPos, value); }

        /// <summary>Общее время песни </summary>
        public string SongTime { get => _songTime; set => SetProperty(ref _songTime, value); }

        /// <summary>Общее время песни на слайдере </summary>
        public double SongTimeForSlider { get => _songTimeForSlider; set => SetProperty(ref _songTimeForSlider, value); }

        /// <summary>Позиция указателя на слайдере </summary>
        public double SongTimeLineForSlider
        {
            get => _songTimeLineForSlider;
            set
            {
                if (PosChanges == true)
                    SongPos = TimeSpan.FromSeconds(value).ToString(@"mm\:ss");
                SetProperty(ref _songTimeLineForSlider, value);
            }
        }

        public double SongVolumeForSlider { get => _songVolumeForSlider; set { _musicPlayerService.Volume = value; SetProperty(ref _songVolumeForSlider, value); } }

        public BitmapImage SongDefaultImage { get; set; }
        public BitmapImage SongImage
        {
            get
            {
                if (_songImage == null)
                    return SongDefaultImage;
                return _songImage;
            }
            set => SetProperty(ref _songImage, value);
        }

        #region Логика - Перетаскивание ползунка таймлайна.

        public ICommand SongPosChangedStartCommand { get; private set; }

        public ICommand SongPosChangedEndedCommand { get; private set; }

        private bool PosChanges;

        private void SongPosChangedStartExecute(object parameter)
            => PosChanges = true;

        private void SongPosChangedEndedExecute(object parameter)
        {
            _musicPlayerService.Position = TimeSpan.FromSeconds(SongTimeLineForSlider);
            PosChanges = false;
        }

        #endregion

        #endregion

     

        #region Команды - Навигациия

        public ICommand NavigationToCommand { get; private set; }
        public ICommand NavigationBackCommand { get; private set; }
        public ICommand NavigationForwardCommand { get; private set; }
        public ICommand NavigationToPlayListCommand { get; private set; }

        private void NavigationToExecute(object parameter)
            => _navigationManager.Navigate(parameter.ToString(), NavigateType.Default);

        private void NavigationBackExecute(object parameter)
            => _navigationManager.GoBack();

        private void NavigationForwardExecute(object parameter)
            => _navigationManager.GoForward();

        private void NavigationToPlayListExecute(object parameter)
            => _navigationManager.Navigate<PlaylistPage>((parameter as PlayList).Id, new PlaylistVM(parameter as PlayList),
                                                         _musicStorage.PlayLists,
                                                         _dialogService,
                                                         _musicPlayerService);

        #endregion

        #region Команды - Управления плеера

        public ICommand SetRandomPlaybackCommand { get; private set; }
        public ICommand PrevSongCommand { get; private set; }
        public ICommand PlayStateSongCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }
        public ICommand SetLoopPlaybackCommand { get; private set; }

        private void SetRandomPlaybackExecute(object parameter)
            => _musicPlayerService.IsRandomSong  = !IsRandomSong;

        private void PrevSongExecute(object parameter) => _musicPlayerService.PrevSong();

        private void PlayStateSongExecute(object parameter)
        {
            if (_musicPlayerService.IsPlaying == true)
                _musicPlayerService.Pause();
            else
                _musicPlayerService.Play();
        }

        private void NextSongExecute(object parameter) => _musicPlayerService.NextSong();

        private void SetLoopPlaybackExecute(object parameter)
        {
            if (++_lMCounter > 2)
                _lMCounter = 0;
            _musicPlayerService.IsRepeatSong = (LoopMode)_lMCounter;
        }

        #region Логика - Цикличное и рандомное воспроизведение трека

        private int _lMCounter;
        private LoopMode _loopMode;
        private bool _isRandomSong;

        public LoopMode LoopMode { get => _loopMode; set => SetProperty(ref _loopMode, value); }
        public bool IsRandomSong { get => _isRandomSong; set => SetProperty(ref _isRandomSong, value); }

        #endregion


        #endregion

        #region Команды - Плейлисты

        public ICommand AddPlayListCommand { get; private set; }
        public ICommand RemovePlayListCommand { get; private set; }

        private async Task AddPlayListExecute(object parameter)
        {
            IsDialogOpen = false;
            var parameters = (object[])parameter;

            await _musicPlayerService.AddPlayList(parameters[0].ToString(),
                                                  parameters[1].ToString(),
                                                  TempImageUri);
        }

        private async Task RemovePlayListExecute(object parameter)
        {
            if (await _musicPlayerService.RemovePlayList(parameter as PlayList) == false)
            {
                _dialogService.ShowMessage("Не удалось удалить.");
            }
        }

        #region Логика - Диалоговое окно

        public ICommand SetPlayListImage
        {
            get => new RelayCommand((o) => {
                TempImageUri = _dialogService.FileBrowserDialog("*.jpg;*.png");
            });
        }


        private bool _isDialogOpen;
        private string _tempImageUri;
        public bool IsDialogOpen { get => _isDialogOpen; set { SetProperty(ref _isDialogOpen, value); if (value == false) TempImageUri = ""; } }
        public string TempImageUri { get =>_tempImageUri; set => SetProperty(ref _tempImageUri, value); }

        #endregion

        #endregion


        #region Cобытия

        private void MusicPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case nameof(_musicPlayerService.CurrentSong):

                        SongImage = null;

                        var temp = _musicPlayerService.CurrentSong;
                        if (temp != CurrentSong)
                        {
                            CurrentSong = temp;
                            SongPos = "00:00";
                            SongTimeLineForSlider = 0;

                            SongTime = temp.Duration.ToString(@"mm\:ss");
                            SongTimeForSlider = _musicPlayerService.Duration.TotalSeconds;

                            try
                            {
                                TagLib.File file_TAG = TagLib.File.Create(temp.LocalUrl);
                                if (file_TAG.Tag.Pictures.Length < 1)
                                    return;

                                using var stream = new MemoryStream(file_TAG.Tag.Pictures[0].Data.Data);

                                var bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.StreamSource = stream;
                                bitmapImage.EndInit();
                                SongImage = bitmapImage;
                            }
                            catch(Exception)
                            {
                                SongImage = null;
                            }
                        }
                        break;

                    case nameof(_musicPlayerService.Position):

                        if (PosChanges == false)
                        {
                            SongPos = _musicPlayerService.Position.ToString(@"mm\:ss");
                            SongTimeLineForSlider = _musicPlayerService.Position.TotalSeconds;
                        }

                        break;
                        
                    case nameof(_musicPlayerService.IsRepeatSong):

                        LoopMode = _musicPlayerService.IsRepeatSong;

                        break;

                    case nameof(_musicPlayerService.IsRandomSong):

                        IsRandomSong = _musicPlayerService.IsRandomSong;

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

        public MainWindowVM(IMusicStorage musicStorage,
                            IDialogService dialogService,
                            INavigationManager navigationManager,
                            IMusicPlayerService musicPlayerService)
        {
            InitiailizeCommands();

            _musicStorage = musicStorage;
            PlayLists = _musicStorage.PlayLists;

            _dialogService = dialogService;
            _navigationManager = navigationManager;

            _musicPlayerService = musicPlayerService;
            _musicPlayerService.PropertyChanged += MusicPlayer_PropertyChanged;


            SongVolumeForSlider = 0.15;
            SongDefaultImage = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud.Pc;component/Images/fsc_icon.png"));

            _lMCounter = 0;
        }

        /// <summary> Инициализация команд </summary>
        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();

         

            SongPosChangedStartCommand = new RelayCommand(SongPosChangedStartExecute, null);
            SongPosChangedEndedCommand = new RelayCommand(SongPosChangedEndedExecute, null);

            SetRandomPlaybackCommand = new RelayCommand(SetRandomPlaybackExecute);
            PrevSongCommand = new RelayCommand(PrevSongExecute, null);
            PlayStateSongCommand = new RelayCommand(PlayStateSongExecute, null);
            NextSongCommand = new RelayCommand(NextSongExecute, null);
            SetLoopPlaybackCommand = new RelayCommand(SetLoopPlaybackExecute);

            AddPlayListCommand = new AsyncRelayCommand(AddPlayListExecute);
            RemovePlayListCommand = new AsyncRelayCommand(RemovePlayListExecute);

            NavigationToCommand = new RelayCommand(NavigationToExecute, (o) => _navigationManager.CanNavigate(o.ToString()));
            NavigationBackCommand = new RelayCommand(NavigationBackExecute, (o) => _navigationManager.CanGoBack());
            NavigationForwardCommand = new RelayCommand(NavigationForwardExecute, (o) => _navigationManager.CanGoForward());
            NavigationToPlayListCommand = new RelayCommand(NavigationToPlayListExecute, null); 
        }

        public override void CloseWindowMethod(object parameter)
        {
            _musicPlayerService.Dispose();
            base.CloseWindowMethod(parameter);
        }

        #endregion
    }
}

#region Команды - Старые 

//public ICommand OpenOnDiskCommand { get; private set; }

//public ICommand AddLocalFolderFromPcCommand { get; private set; }
//public ICommand AddLocalSongFromPCCommand { get; private set; }

//public ICommand DeleteFromAppCommand { get; private set; }

///// <summary>Метод открытия папки на диске</summary>
//public void OpenFolderOnDiskExecute(object parameter)
//    => _dialogService.ShowFolder(parameter.ToString());

//private async Task AddLocalFolderFromPcExecute(object parameter)
//    => LocalFolders.Add(await _musicPlayer.AddLocalFolderFromDevice(_dialogService.FolderBrowserDialog()));


////var path = ;
////LocalFolders.Add(await Task.Run(() => model.AddLocalSongFromPC(path)));


//private async Task RemoveFromAppExecute(object parameter)
//{
//    _musicPlayer.Stop();

//    //if (SelectedBMO is LocalFolder)
//    //{
//    //    if (await model.RemoveLocalFolderFromPC(SelectedMusicContainer.ToLF()) == true)
//    //    {
//    //        LocalFolders.Remove(SelectedMusicContainer.ToLF());
//    //    }
//    //}
//    //else
//    //{
//    //    //LocalFiles.Remove(await Task.Run(() => model.DeleteLocalSongFromPC(CurrentSong.Content)));

//    //}
//}


//OpenOnDiskCommand = new RelayCommand(OpenFolderOnDiskExecute, null);

//AddLocalFolderFromPcCommand = new AsyncRelayCommand(AddLocalFolderFromPcExecute, (ex) => _dialogService.ShowMessage(ex.Message));
////AddLocalSongFromPCCommand = new AsyncRelayCommand(AddSongFromPcExecute, (ex) => _dialogService.ShowMessage(ex.Message));

//DeleteFromAppCommand = new AsyncRelayCommand(RemoveFromAppExecute, null);

#endregion