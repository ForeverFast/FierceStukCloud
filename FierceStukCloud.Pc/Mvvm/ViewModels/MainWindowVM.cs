using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Mvvm.Commands;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using FierceStukCloud.Pc.Mvvm.ViewModels.PageVMs;
using FierceStukCloud.Pc.Mvvm.Views.Pages;
using FierceStukCloud.Wpf.Services.ImageAsyncS;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FierceStukCloud.Pc.Mvvm.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel
    {
        #region Управление плеером


        #region Свойства - Отображение информации о текущем треке

        #region Поля
        private BitmapImage _songBitmapImage;
        private string _songName;
        private string _SongAuthor;

        private string _songTime = "99:99";
        private string _songPos = "00:00";
        private double _songTimeForSlider;
        private double _songTimeLineForSlider;

        private double _songVolumeForSlider;

        private string _selectedStyle;
        #endregion

        public BitmapImage SongBitmapImage { get => _songBitmapImage; set => SetProperty(ref _songBitmapImage, value); }

        public string SongName { get => _songName; set => SetProperty(ref _songName, value); }

        public string SongAuthor { get => _SongAuthor; set => SetProperty(ref _SongAuthor, value); }

        /// <summary>Текущее время трека</summary>
        public string SongPos { get => _songPos; set => SetProperty(ref _songPos, value); }

        /// <summary>Общее время песни </summary>
        public string SongTime { get => _songTime; set => SetProperty(ref _songTime, value); }

        /// <summary>Общее время песни на слайдере </summary>
        public double SongTimeForSlider { get => _songTimeForSlider; set => SetProperty(ref _songTimeForSlider, value); }

        /// <summary>Позиция указателя на слайдере </summary>
        public double SongTimeLineForSlider { get => _songTimeLineForSlider; set => SetProperty(ref _songTimeLineForSlider, value); }

        public double SongVolumeForSlider { get => _songVolumeForSlider; set { _musicPlayerService.Volume = value; SetProperty(ref _songVolumeForSlider, value); } }


        public string SelectedStyle { get => _selectedStyle; set => SetProperty(ref _selectedStyle, value); }

        #endregion


        #region Свойства - Коллекции/выбранные элементы списков

        #region Поля
        private IMusicContainer _selectedMusicContainer;
        private ImageAsync<Song> _selectedSong;
        #endregion
            
        public ObservableCollection<PlayList> PlayLists { get => _musicStorage.PlayLists; }

        public IMusicContainer SelectedMusicContainer { get => _selectedMusicContainer; set => SetProperty(ref _selectedMusicContainer, value); }

        public ImageAsync<Song> SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != null)
                {
                    //value.Content.CurrentMusicContainer = SelectedMusicContainer;
                    _musicPlayerService.CurrentSong = value.Content;
                    SetProperty(ref _selectedSong, value);
                }
            }
        }

        #endregion



     

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
            => _navigationManager.Navigate<PlaylistPage>(new PlaylistVM(parameter as PlayList),
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

        private void SetRandomPlaybackExecute(object parameter) => _musicPlayerService.IsRandomSong = !_musicPlayerService.IsRandomSong;

        private void PrevSongExecute(object parameter) => _musicPlayerService.PrevSong();

        private void PlayStateSongExecute(object parameter)
        {
            if (_musicPlayerService.IsPlaying == true)
                _musicPlayerService.Pause();
            else
                _musicPlayerService.Play();
        }

        private void NextSongExecute(object parameter) => _musicPlayerService.NextSong();

        private void SetLoopPlaybackExecute(object parameter) => _musicPlayerService.IsRepeatSong = !_musicPlayerService.IsRepeatSong;

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









        #region Обработка событий



        private void MusicPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case nameof(_musicPlayerService.CurrentSong):

                        var temp = _musicPlayerService.CurrentSong;
                        if (temp != SelectedSong.Content)
                        {
                            SelectedSong = new ImageAsync<Song>(temp.LocalUrl, temp);
                            SongPos = "00:00";
                            SongTimeLineForSlider = 0;

                            SongTime = temp.Duration;
                           
                            //SongTimeForSlider = 
                            //_musicPlayer.MP.NaturalDuration.TimeSpan.TotalSeconds;
                        }
                        break;

                    case nameof(_musicPlayerService.Position):

                        if (PosChanges == false)
                        {
                            SongPos = _musicPlayerService.Position.ToString(@"mm\:ss");
                            SongTimeLineForSlider = _musicPlayerService.Position.TotalSeconds;
                        }

                        break;

                    case "SongLoaded":

                        SelectedStyle = "PauseButton";

                       

                        //SongName = .Title;
                        //SongAuthor = _musicPlayer.CurrentSong.Author;
                        //SongBitmapImage = _musicPlayer.CurrentImage;

                       

                        break;

                  

                    case nameof(_musicPlayerService.Play):

                        SelectedStyle = "PauseButton";

                        break;

                    case nameof(_musicPlayerService.Pause):
                    case nameof(_musicPlayerService.Stop):

                        SelectedStyle = "PlayButton";

                        break;
                }
            }
        }

        #endregion


        #region Конструкторы

        //public MainWindowVM()
        //{

        //}

        public MainWindowVM(IMusicStorage musicStorage,
                            IDialogService dialogService,
                            INavigationManager navigationManager,
                            IMusicPlayerService musicPlayerService)
        {
            try
            {
                InitiailizeCommands();

                _musicStorage = musicStorage;
                _dialogService = dialogService;
                _navigationManager = navigationManager;
                _musicPlayerService = musicPlayerService;
               

                _musicPlayerService.PropertyChanged += MusicPlayer_PropertyChanged;
            
                //SongBitmapImage = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));

                SongVolumeForSlider = 0.15;
                //SelectedStyle = "PlayButton";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //public MainWindowVM() : this()
        //{
           

        //    //Songs = new ImageAsyncCollection<ImageAsync<Song>>
        //    //(
        //    //    _dispatcher,
        //    //    new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud.Core;component/Images/fsc_icon.png"))
        //    //);

        //}

        /// <summary>
        /// Инициализация команд
        /// </summary>
        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();

            //OpenOnDiskCommand = new RelayCommand(OpenFolderOnDiskExecute, null);

            //AddLocalFolderFromPcCommand = new AsyncRelayCommand(AddLocalFolderFromPcExecute, (ex) => _dialogService.ShowMessage(ex.Message));
            ////AddLocalSongFromPCCommand = new AsyncRelayCommand(AddSongFromPcExecute, (ex) => _dialogService.ShowMessage(ex.Message));

            //DeleteFromAppCommand = new AsyncRelayCommand(RemoveFromAppExecute, null);

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
            NavigationToPlayListCommand = new RelayCommand(NavigationToPlayListExecute, null); //, 
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
//    //    //LocalFiles.Remove(await Task.Run(() => model.DeleteLocalSongFromPC(SelectedSong.Content)));

//    //}
//}

#endregion