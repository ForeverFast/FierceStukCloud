using Egor92.MvvmNavigation;
using Egor92.MvvmNavigation.Abstractions;
using FierceStukCloud.Core;
using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Mvvm.Commands;
using FierceStukCloud.Pc.Mvvm.ViewModels.Abstractions;
using FierceStukCloud.Pc.Services;
using FierceStukCloud.Wpf.Services.ImageAsyncS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FierceStukCloud.Pc.Mvvm.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
       

        public string Title { get; set; }




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

        public double SongVolumeForSlider { get => _songVolumeForSlider; set { _musicPlayer.Volume = value; SetProperty(ref _songVolumeForSlider, value); } }


        public string SelectedStyle { get => _selectedStyle; set => SetProperty(ref _selectedStyle, value); }

        #endregion


        #region Свойства - Коллекции/выбранные элементы списков

        #region Поля
        private IMusicContainer _selectedMusicContainer;
        private ImageAsync<Song> _selectedSong;
        #endregion

        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<LocalFolder> LocalFolders { get; set; }
        public ObservableCollection<PlayList> PlayLists { get; set; }
        public ImageAsyncCollection<ImageAsync<Song>> Songs { get; set; }

        public IMusicContainer SelectedMusicContainer
        {
            get => _selectedMusicContainer;
            set
            {
                SetProperty(ref _selectedMusicContainer, value);
                Songs.Clear();
                if (value != null)
                {
                    _musicPlayer.DisplayedMusicContainer = value;

                    foreach (var item in value.Songs)
                    {
                        if (item == _musicPlayer.CurrentSong)
                            Songs.Add(SelectedSong);
                        else
                            Songs.Add(new ImageAsync<Song>(item.LocalUrl, item));
                    }
                }
            }
        }
        public ImageAsync<Song> SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != null)
                {
                    value.Content.CurrentMusicContainer = SelectedMusicContainer;// != null ?
                                                                                 //SelectedMusicContainer : new LocalFolder() { Songs = GetLocalFilesAsMC() };
                    
                    _musicPlayer.CurrentSong = value.Content;
                    SetProperty(ref _selectedSong, value);
                }
            }
        }
        //public BaseMusicObject SelectedBMO
        //{
        //    get => _selectedBMO;
        //    set
        //    {
        //        if (value is Song)
        //        {
        //            _selectedBMO = value;
        //            SelectedSong = new ImageAsync<Song>(
        //                _dispatcher,
        //                new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"),
        //                value as Song);
        //        }
        //        else
        //        {
        //            _selectedBMO = value;
        //            SelectedMusicContainer = value as MusicContainer;
        //        }
        //    }
        //}


        // private List<Song> GetLocalFilesAsMC() => (from temp in LocalFolder where temp is Song select temp) as List<Song>;


        #endregion



     

        #region Логика перетаскивания ползунка слайдера таймлайна.

        public ICommand SongPosChangedStartCommand { get; private set; }

        public ICommand SongPosChangedEndedCommand { get; private set; }

        private bool PosChanges;

        private void SongPosChangedStartExecute(object parameter)
            => PosChanges = true;

        private void SongPosChangedEndedExecute(object parameter)
        {
            _musicPlayer.Position = TimeSpan.FromSeconds(SongTimeLineForSlider);
            PosChanges = false;
        }

        #endregion


        #endregion

        #region Команды - Старые 

        public ICommand OpenOnDiskCommand { get; private set; }

        public ICommand AddLocalFolderFromPcCommand { get; private set; }
        public ICommand AddLocalSongFromPCCommand { get; private set; }

        public ICommand DeleteFromAppCommand { get; private set; }

        /// <summary>Метод открытия папки на диске</summary>
        public void OpenFolderOnDiskExecute(object parameter)
            => _dialogService.ShowFolder(parameter.ToString());

        private async Task AddLocalFolderFromPcExecute(object parameter)
            => LocalFolders.Add(await _musicPlayer.AddLocalFolderFromDevice(_dialogService.FolderBrowserDialog()));


        //var path = ;
        //LocalFolders.Add(await Task.Run(() => model.AddLocalSongFromPC(path)));


        private async Task RemoveFromAppExecute(object parameter)
        {
            _musicPlayer.Stop();

            //if (SelectedBMO is LocalFolder)
            //{
            //    if (await model.RemoveLocalFolderFromPC(SelectedMusicContainer.ToLF()) == true)
            //    {
            //        LocalFolders.Remove(SelectedMusicContainer.ToLF());
            //    }
            //}
            //else
            //{
            //    //LocalFiles.Remove(await Task.Run(() => model.DeleteLocalSongFromPC(SelectedSong.Content)));

            //}
        }

        #endregion

        #region Команды - работа с файлами


        #region Методы добавления/удаления песен на устройстве

        public ICommand AddSongFromDeviceCommand { get; private set; }
        public ICommand RemoveSongFromDeviceCommand { get; private set; }
        public ICommand RemoveSongFromAppCommand { get; private set; }

        private async Task AddSongFromDeviceExecute(object parameter)
             => LocalFolders.Add(await _musicPlayer.AddLocalFolderFromDevice(_dialogService.FileBrowserDialog()));




        #endregion


        #region Методы добавления/удаления песен на сервере 

        public ICommand AddSongToServerCommand { get; private set; }
        public ICommand RemoveSongFromServerCommand { get; private set; }

        #endregion

        public ICommand UpdateSongInfo { get; private set; }

        #endregion


        #region Команды - Управления плеера

        public ICommand SetRandomPlaybackCommand { get; private set; }
        public ICommand PrevSongCommand { get; private set; }
        public ICommand PlayStateSongCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }
        public ICommand SetLoopPlaybackCommand { get; private set; }

        private void SetRandomPlaybackExecute(object parameter) => _musicPlayer.IsRandomSong = !_musicPlayer.IsRandomSong;

        private void PrevSongExecute(object parameter) => _musicPlayer.PrevSong();

        private void PlayStateSongExecute(object parameter)
        {
            if (_musicPlayer.IsPlaying == true)
                _musicPlayer.Pause();
            else
                _musicPlayer.Play();
        }

        private void NextSongExecute(object parameter) => _musicPlayer.NextSong();

        private void SetLoopPlaybackExecute(object parameter) => _musicPlayer.IsRepeatSong = !_musicPlayer.IsRepeatSong;
        #endregion


        #region Команды - Навигациия

        //public ICommand NavigateToHomePageCommand { get; private set; }
        //public ICommand NavigateToReviewPageCommand { get; private set; }
        //public ICommand NavigateToProfilePageCommand { get; private set; }

        public ICommand NavigationToCommand { get; private set; }
        public ICommand NavigationBackCommand { get; private set; }
        public ICommand NavigationForwardCommand { get; private set; }

        public void NavigationToExecute(object parameter)
            => _navigationManager.Navigate(parameter.ToString(), NavigateType.Default);

        public void NavigationBackExecute(object parameter)
            => _navigationManager.GoBack();

        public void NavigationForwardExecute(object parameter)
            => _navigationManager.GoForward();

        #endregion


       


        #region Обработка событий



        private void MusicPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case nameof(_musicPlayer.CurrentSong):

                        var temp = _musicPlayer.CurrentSong;
                        if (temp != SelectedSong.Content)
                            SelectedSong = new ImageAsync<Song>(temp.LocalUrl, temp);

                        break;

                    case "SongLoaded":

                        SelectedStyle = "PauseButton";

                        //SongTime = _musicPlayer.MP.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                        SongTimeLineForSlider = 0;
                        //SongTimeForSlider = _musicPlayer.MP.NaturalDuration.TimeSpan.TotalSeconds;

                        SongName = _musicPlayer.CurrentSong.Title;
                        SongAuthor = _musicPlayer.CurrentSong.Author;
                        //SongBitmapImage = _musicPlayer.CurrentImage;

                        SelectedSong.Content.IsSelected = false;
                        SelectedSong = Songs.FirstOrDefault(x => x.Content == _musicPlayer.CurrentSong);
                        SelectedSong.Content.IsSelected = true;

                        break;

                    //case nameof(_musicPlayer.Timer_Tick):

                    //    if (PosChanges == false)
                    //    {
                    //        SongPos = _musicPlayer.Position.ToString(@"mm\:ss");
                    //        SongTimeLineForSlider = _musicPlayer.Position.TotalSeconds;
                    //    }

                    //    break;

                    case nameof(_musicPlayer.Play):

                        SelectedStyle = "PauseButton";

                        break;

                    case nameof(_musicPlayer.Pause):
                    case nameof(_musicPlayer.Stop):

                        SelectedStyle = "PlayButton";

                        break;
                }
            }
        }

        #endregion


        #region Конструкторы

        public MainWindowVM()
        {
            try
            {
                InitiailizeCommands();

                _dialogService = new DialogService();         
                _musicPlayer = new MusicPlayerService(App.CurrentUser);
                _musicPlayer.PropertyChanged += MusicPlayer_PropertyChanged;

                Albums = _musicPlayer.Albums;
                LocalFolders = _musicPlayer.LocalFolders;
                PlayLists = _musicPlayer.PlayLists;

                //SongBitmapImage = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));

                SongVolumeForSlider = 0.15;
                SelectedStyle = "PlayButton";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public MainWindowVM(INavigationManager navigationManager) : this()
        {
            _navigationManager = navigationManager;

            //Songs = new ImageAsyncCollection<ImageAsync<Song>>
            //(
            //    _dispatcher,
            //    new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud.Core;component/Images/fsc_icon.png"))
            //);

        }

        /// <summary>
        /// Инициализация команд
        /// </summary>
        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();

            OpenOnDiskCommand = new RelayCommand(OpenFolderOnDiskExecute, null);

            AddLocalFolderFromPcCommand = new AsyncRelayCommand(AddLocalFolderFromPcExecute, (ex) => _dialogService.ShowMessage(ex.Message));
            //AddLocalSongFromPCCommand = new AsyncRelayCommand(AddSongFromPcExecute, (ex) => _dialogService.ShowMessage(ex.Message));

            DeleteFromAppCommand = new AsyncRelayCommand(RemoveFromAppExecute, null);

            SongPosChangedStartCommand = new RelayCommand(SongPosChangedStartExecute, null);
            SongPosChangedEndedCommand = new RelayCommand(SongPosChangedEndedExecute, null);

            SetRandomPlaybackCommand = new RelayCommand(SetRandomPlaybackExecute);
            PrevSongCommand = new RelayCommand(PrevSongExecute, null);
            PlayStateSongCommand = new RelayCommand(PlayStateSongExecute, null);
            NextSongCommand = new RelayCommand(NextSongExecute, null);
            SetLoopPlaybackCommand = new RelayCommand(SetLoopPlaybackExecute);

            NavigationToCommand = new RelayCommand(NavigationToExecute, (o) => _navigationManager.CanNavigate(o.ToString()));
            NavigationBackCommand = new RelayCommand(NavigationBackExecute, (o) => _navigationManager.CanGoBack());
            NavigationForwardCommand = new RelayCommand(NavigationForwardExecute, (o) => _navigationManager.CanGoForward());
        }

        public override void CloseWindowMethod(object parameter)
        {

            //_musicPlayer.ShutDownConnection();
            base.CloseWindowMethod(parameter);
        }



        #endregion
    }
}
