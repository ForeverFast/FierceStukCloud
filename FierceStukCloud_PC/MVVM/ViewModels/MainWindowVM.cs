using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using FierceStukCloud_NetCoreLib.Services;
using FierceStukCloud_NetCoreLib.ViewModels;
using FierceStukCloud_PC.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static FierceStukCloud_NetCoreLib.Services.DialogService;
using System.Threading.Tasks;
using System.Threading;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using System.ComponentModel;
using System.IO;
using FierceStukCloud_NetStandardLib;
using FierceStukCloud_NetStandardLib.Extension;
using FierceStukCloud_NetCoreLib.Commands;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using Egor92.MvvmNavigation.Abstractions;
using Egor92.MvvmNavigation;
using FierceStukCloud_PC.MVVM.Views.TestView;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        private MainWindowM model { get; }
        private readonly Dispatcher _dispatcher;
        private readonly NavigationManager _navigationManager;

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

        public double SongVolumeForSlider {  get => _songVolumeForSlider; set {  model.MP.Volume = value; SetProperty(ref _songVolumeForSlider, value); } }


        public string SelectedStyle{ get => _selectedStyle; set => SetProperty(ref _selectedStyle, value); }

        #endregion


        #region Свойства - Коллекции/выбранные элементы списков

        #region Поля
        private MusicContainer _selectedMusicContainer;
        private ImageAsync<Song> _selectedSong;
        private BaseMusicObject _selectedBMO;
        #endregion

        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<LocalFolder> LocalFolders { get; set; }
        public ObservableCollection<PlayList> PlayLists { get; set; }    
        public ImageAsyncCollection<ImageAsync<Song>> Songs { get; set; }

        public MusicContainer SelectedMusicContainer
        { 
            get => _selectedMusicContainer;
            set
            {
                SetProperty(ref _selectedMusicContainer, value);
                Songs.Clear();
                if (value != null)
                {
                    model.DisplayedMusicContainer = value;
                    
                    foreach (var item in value.Songs)
                    {
                        if (item == model.CurrentSong)
                            Songs.Add(SelectedSong);
                        else
                            Songs.Add(new ImageAsync<Song>
                                (
                                    _dispatcher,
                                    item.LocalUrl,
                                    item
                                ));
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

                    model.SetCurrentSong(value.Content);
                    SetProperty(ref _selectedSong, value);
                }
            }
        }
        public BaseMusicObject SelectedBMO
        {
            get => _selectedBMO;
            set
            {
                if (value is Song)
                {
                    _selectedBMO = value;
                    SelectedSong = new ImageAsync<Song>(
                        _dispatcher,
                        new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"),
                        value as Song);
                }
                else
                {
                    _selectedBMO = value;
                    SelectedMusicContainer = value as MusicContainer;
                }
            }
        }


        // private List<Song> GetLocalFilesAsMC() => (from temp in LocalFolder where temp is Song select temp) as List<Song>;


        #endregion


        #region Команды - Локальные папки/песни

        public ICommand OpenOnDiskCommand { get; private set; }

        public ICommand AddLocalFolderFromPCCommand { get; private set; }
        public ICommand AddLocalSongFromPCCommand { get; private set; }

        public ICommand DeleteFromAppCommand { get; private set; }

        /// <summary>Метод открытия папки на диске</summary>
        public async Task OpenFolderOnDiskExecute(object parameter)
        {
            if (SelectedBMO is LocalFolder)
            {
                ShowFolder((SelectedBMO as LocalFolder).LocalUrl);
            }
            else
            {
                ShowFolder((SelectedBMO as Song).LocalUrl);
            }
        }

        private async Task AddLocalFolderFromPCExecute(object parameter)
        {
            var path = FolderBrowserDialog();
            LocalFolders.Add(await Task.Run(() => model.AddLocalFolderFromPC(path)));    
        }

        private async Task AddLocalSongFromPCExecute(object parameter)
        {
            //var path = FileBrowserDialog();
            //LocalFolders.Add(await Task.Run(() => model.AddLocalSongFromPC(path)));
        }

        private async Task DeleteFromAppExecute(object parameter)
        {
            model.Stop();
            if (SelectedBMO is LocalFolder)
            {
                if (await model.RemoveLocalFolderFromPC(SelectedMusicContainer.ToLF()) == true)
                {
                    LocalFolders.Remove(SelectedMusicContainer.ToLF());
                }
            }
            else
            {
                //LocalFiles.Remove(await Task.Run(() => model.DeleteLocalSongFromPC(SelectedSong.Content)));

            }
        }


        #region Выключение звука при перетаскивании ползунка слайдера таймлайна.

        public RelayCommand SongPosChangedStartCommand { get; private set; }

        public RelayCommand SongPosChangedEndedCommand { get; private set; }

        private bool PosChanges;

        private void SongPosChangedStartExecute(object parameter)
            => PosChanges = true;
           
        private void SongPosChangedEndedExecute(object parameter)
        {
            model.MP.Position = TimeSpan.FromSeconds(SongTimeLineForSlider);
            PosChanges = false;        
        }

        #endregion


        #endregion


        #region Команды - Управления плеера

        public RelayCommand PrevSongCommand { get; set; }
        public RelayCommand PlayStateSongCommand { get; set; }
        public RelayCommand NextSongCommand { get; set; }

        private void PrevSongExecute(object parameter) => model.PrevSong();

        private void PlayStateSongExecute(object parameter)
        {
            if (model.IsPlaying == true)
                model.Pause();
            else
                model.Play();
        }

        private void NextSongExecute(object parameter) => model.NextSong();

        #endregion


        #region Команды - Навигациия

        public ICommand NavigateToHomePageCommand { get; private set; }
        public ICommand NavigateToProfilePageCommand { get; private set; }


        public ICommand NavigationToCommand { get; private set; }
        public ICommand NavigationBackCommand { get; private set; }
        public ICommand NavigationForwardCommand { get; private set; }


        public void NavigateToHomePageExecute(object parameter)
        {
            
        }

        public void NavigationToExecute(object parameter)
        {
            //NavigationToExecute("MVVM/Views/TestView/Page1.xaml", new TestVM() { test = "kek"});
            //var values = (object[])parameter;
            //Messenger.Default.Send<NavigateArgs>(new NavigateArgs("MVVM/Views/TestView/Page1.xaml", new TestVM() { test = "kek" }, NavigateType.NavigateTo));
            //if(_navigationManager.CanNavigate("test1"))
            _navigationManager.Navigate<Page1>(new TestVM() { test = "kek" });
            
        }

        public void NavigationBackExecute(object parameter)
            => Messenger.Default.Send<NavigateArgs>(new NavigateArgs(NavigateType.Back));
        

        public void NavigationForwardExecute(object parameter)
            => Messenger.Default.Send<NavigateArgs>(new NavigateArgs(NavigateType.Forward));
        

        #endregion


        #endregion


        #region Обработка событий



        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case nameof(model.MP_MediaOpened):

                        SelectedStyle = "PauseButton";

                        SongTime = model.MP.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                        SongTimeLineForSlider = 0;
                        SongTimeForSlider = model.MP.NaturalDuration.TimeSpan.TotalSeconds;

                        SongName = model.CurrentSong.Title;
                        SongAuthor = model.CurrentSong.Author;
                        SongBitmapImage = model.CurrentImage;

                        SelectedSong.IsSelected = false;
                        SelectedSong = Songs.FirstOrDefault(x => x.Content == model.CurrentSong);
                        SelectedSong.IsSelected = true;

                        break;

                    case nameof(model.Timer_Tick):

                        if (PosChanges == false)
                        {
                            SongPos = model.MP.Position.ToString(@"mm\:ss");
                            SongTimeLineForSlider = model.MP.Position.TotalSeconds;
                        }

                        break;

                    case nameof(model.Play):

                        SelectedStyle = "PauseButton";

                        break;

                    case nameof(model.Pause):
                    case nameof(model.Stop):

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

                model = new MainWindowM(_dispatcher);
                model.PropertyChanged += Model_PropertyChanged;

                Albums = model.Albums;
                LocalFolders = model.LocalFolders;
                PlayLists = model.PlayLists;

                SongBitmapImage = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));

                model.MP.Volume = 0.15;
                SongVolumeForSlider = 0.15;

                SelectedStyle = "PlayButton";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public MainWindowVM(Dispatcher dispatcher, NavigationManager navigationManager) : this()
        {
            _dispatcher = dispatcher;
            _navigationManager = navigationManager;

            Songs = new ImageAsyncCollection<ImageAsync<Song>>
            (
                _dispatcher,
                new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"))
            );
           
        }

        /// <summary>
        /// Инициализация команд
        /// </summary>
        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();

            OpenOnDiskCommand = new AsyncRelayCommand(OpenFolderOnDiskExecute, null);

            AddLocalFolderFromPCCommand = new AsyncRelayCommand(AddLocalFolderFromPCExecute, (ex) => ShowMessage(ex.Message));
            AddLocalSongFromPCCommand = new AsyncRelayCommand(AddLocalSongFromPCExecute, null);

            DeleteFromAppCommand = new AsyncRelayCommand(DeleteFromAppExecute, null);

            SongPosChangedStartCommand = new RelayCommand(SongPosChangedStartExecute, null);
            SongPosChangedEndedCommand = new RelayCommand(SongPosChangedEndedExecute, null);

            PrevSongCommand = new RelayCommand(PrevSongExecute, null);
            PlayStateSongCommand = new RelayCommand(PlayStateSongExecute, null);
            NextSongCommand = new RelayCommand(NextSongExecute, null);

            NavigationToCommand = new RelayCommand(NavigationToExecute);
            NavigationBackCommand = new RelayCommand(NavigationBackExecute);
            NavigationForwardCommand = new RelayCommand(NavigationForwardExecute);

            

        }

        public override void CloseWindowMethod(object parameter)
        {
           
            model.ShutDownConnection();
            base.CloseWindowMethod(parameter);
        }

        

        #endregion
    }
}
