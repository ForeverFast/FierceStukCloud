using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Models.AbstractModels;
using FierceStukCloud_NetCoreLib.Models.MusicContainers;
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
using static FierceStukCloud_NetCoreLib.Types.CallerType;
using static FierceStukCloud_NetCoreLib.Services.Extension.DialogService;
namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {
        public ObservableCollection<Song> Songs { get; set; }

        private MainWindowM model;
        //private DialogService dialogService;


        #region Управление плеером


        #region Обработка событий

        private void Model_SongChangedEvent(Song song, Caller caller) => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
        {
            switch (caller)
            {
                case Caller.User:

                    break;
                case Caller.Program:

                    break;

                case Caller.Phone:
                    //SelectedSong = song;
                    //UpdataSongInfo();

                    break;

                case Caller.Web:
                    //SelectedSong = song;
                    //UpdataSongInfo();

                    break;

            }

        }));

        public void UpdataSongInfo()
        {
            while (!model.MP.NaturalDuration.HasTimeSpan) { }

            SongTime = model.MP.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

            SongTimeForSlider = model.MP.NaturalDuration.TimeSpan.TotalSeconds;

            SongName = model.CurrentSong.Title;
            SongAuthor = model.CurrentSong.Author;

            SongTimeLineForSlider = 0;

            SongBitmapImage = model.CurrentImage;
        }

        private void Model_SongPositionChangerd()
        {
            SongPos = model.MP.Position.ToString(@"mm\:ss");
            SongTimeLineForSlider = model.MP.Position.TotalSeconds;
        }

        #endregion


       

       




        /// <summary>Метод открытия папки на диске</summary>
        public void OpenFolderOnDiskMethod(object parameter)
        {
            //App.Dialog.ShowFolder(SelectedPlayList.URL);
        }

       





        #region Отображение информации о текущем треке

        #region Поля
        private BitmapImage _SongBitmapImage;
        private string _SongName;
        private string _SongAuthor;

        private string _SongTime = "99:99";
        private string _SongPos = "00:00";
        private double _SongTimeForSlider;
        private double _SongTimeLineForSlider;

        private double _SongVolumeForSlider;
        #endregion

        private int _SelectedSongOnLB;

        public BitmapImage SongBitmapImage
        {
            get => _SongBitmapImage;
            set
            {
                _SongBitmapImage = value;
                OnPropertyChanged(nameof(SongBitmapImage));
            }
        }

        public string SongName
        {
            get => _SongName;
            set
            {
                _SongName = value;
                OnPropertyChanged(nameof(SongName));
            }
        }

        public string SongAuthor
        {
            get => _SongAuthor;
            set
            {
                _SongAuthor = value;
                OnPropertyChanged(nameof(SongAuthor));
            }
        }

        /// <summary>Текущее время трека</summary>
        public string SongPos
        {
            get => _SongPos;
            set
            {
                _SongPos = value;
                OnPropertyChanged(nameof(SongPos));
            }
        }
        /// <summary>Общее время песни </summary>
        public string SongTime
        {
            get => _SongTime;
            set
            {
                _SongTime = value;
                OnPropertyChanged(nameof(SongTime));
            }
        }
        /// <summary>Общее время песни на слайдере </summary>
        public double SongTimeForSlider
        {
            get => _SongTimeForSlider;
            set
            {
                _SongTimeForSlider = value;
                OnPropertyChanged(nameof(SongTimeForSlider));
            }
        }
        /// <summary>Позиция указателя на слайдере </summary>
        public double SongTimeLineForSlider
        {
            get => _SongTimeLineForSlider;
            set
            {
                TimeSpan ts = TimeSpan.FromSeconds(value);
                model.MP.Position = ts;
                _SongTimeLineForSlider = value;

                OnPropertyChanged(nameof(SongTimeLineForSlider));
            }
        }

        public double SongVolumeForSlider
        {
            get => _SongVolumeForSlider;
            set
            {
                _SongVolumeForSlider = value;

                model.MP.Volume = value;
                OnPropertyChanged(nameof(SongVolumeForSlider));
            }
        }


        public int SelectedSongOnLB
        {
            get => _SelectedSongOnLB;
            set
            {

                _SelectedSongOnLB = value;
                OnPropertyChanged(nameof(SelectedSongOnLB));
            }
        }

        #endregion


        #region Кнопки управления плеера

        public RelayCommand PrevSongCommand { get; set; }
        private async void PrevSongExecute(object parameter) => await model.PrevSong(Caller.User);
        public RelayCommand PlayStateSongCommand { get; set; }
        private void PlayStateSongExecute(object parameter) => model.PlayState();
        public RelayCommand NextSongCommand { get; set; }
        private async void NextSongExecute(object parameter) => await model.NextSong(Caller.User);

        #endregion


        #region Свойства

        private MusicContainer _selectedMusicContainer;
        private Song _selectedSong;

        public MusicContainer SelectedMusicContainer { get => _selectedMusicContainer; set => SetProperty(ref _selectedMusicContainer, value); }

        public Song SelectedSong
        {
            get => _selectedSong;
            set
            {
                value.CurrentMusicContainer = SelectedMusicContainer != null ?
                                              SelectedMusicContainer : new LocalFolder() { Songs = GetLocalFilesAsMC() };

                model.SetCurrentSong(value, Caller.User);
                SetProperty(ref _selectedSong, value);
            }
        }


        #endregion


        #region Команды

        public RelayCommand AddMusicContainerFromPCCommand { get; private set; }

        private void AddMusicContainerFromPCExecute(object parameter) => model.AddMusicContainerFromPC(FolderBrowserDialog());


        #endregion


        #region ListBox Локальных файлов

        public ObservableCollection<BaseMusicObject> LocalFiles { get; set; }
             = new ObservableCollection<BaseMusicObject>();


        private BaseMusicObject _selectedBMO;

        public BaseMusicObject SelectedBMO
        {
            get => _selectedBMO;
            set
            {
                if (value is Song)
                {
                    _selectedBMO = value;
                    SelectedSong = value as Song;
                }
                else
                {
                    _selectedBMO = value;
                    SelectedMusicContainer = value as MusicContainer;
                }
            }
        }

        private List<Song> GetLocalFilesAsMC() => (from temp in LocalFiles where temp is Song select temp) as List<Song>;


        #endregion






        #endregion



        #region Конструкторы и ViewModels

        //private ObservableCollection<object> _children;
        //public ObservableCollection<object> Children { get { return _children; } }

        public MainWindowVM()
        {
            try
            {
                // Начальные настройки
                //_children = new ObservableCollection<object>();
                //_children.Add(new DefaultPageVM());
                //_children.Add(new SettingsVM());

                //dialogService = new DialogService();
                model = new MainWindowM();

                InitiailizeCommands();
                
               

                




                // Настройки плеера

                //model.NewPlayListEvent += Model_NewPlayListEvent;
                //model.DeletePlayListEvent += Model_DeletePlayListEvent;
                //model.UpdatePlayListsInfoEvent += Model_UpdatePlayListsInfoEvent;

                //model.SongChangedEvent += Model_SongChangedEvent;
                //model.SongPositionChangerd += Model_SongPositionChangerd;

               //PlayLists = new ObservableCollection<PlayList>(model.PL);

                //SongBitmapImage = model.CurrentImage;

                model.MP.Volume = 0.15;
                SongVolumeForSlider = 0.15;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();

            AddMusicContainerFromPCCommand = new RelayCommand(AddMusicContainerFromPCExecute, null);

            //NewFolderCommand = new RelayCommand(NewFolderMethod, null);
            //AddToMainServerCommand = new RelayCommand(AddToMainServerMethod, null);
            //DeleteFolderCommand = new RelayCommand(DeleteFolderMethod, null);
            //OpenFolderOnDiskCommand = new RelayCommand(OpenFolderOnDiskMethod, null);


            PrevSongCommand = new RelayCommand(PrevSongExecute, null);
            PlayStateSongCommand = new RelayCommand(PlayStateSongExecute, null);
            NextSongCommand = new RelayCommand(NextSongExecute, null);
        }

        #endregion
    }
}
