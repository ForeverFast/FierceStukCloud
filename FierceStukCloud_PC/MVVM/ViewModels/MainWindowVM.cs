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
using System.Threading.Tasks;
using System.Threading;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {

        private MainWindowM model;

        #region Управление плеером


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

        public string SongName { get => _SongName; set => SetProperty(ref _SongName, value); }

        public string SongAuthor { get => _SongAuthor; set => SetProperty(ref _SongAuthor, value); }

        /// <summary>Текущее время трека</summary>
        public string SongPos { get => _SongPos; set => SetProperty(ref _SongPos, value); }

        /// <summary>Общее время песни </summary>
        public string SongTime { get => _SongTime; set => SetProperty(ref _SongTime, value); }

        /// <summary>Общее время песни на слайдере </summary>
        public double SongTimeForSlider { get => _SongTimeForSlider; set => SetProperty(ref _SongTimeForSlider, value); }
        
        /// <summary>Позиция указателя на слайдере </summary>
        public double SongTimeLineForSlider { get => _SongTimeLineForSlider;
                                              set { if(PosChanges == false) model.MP.Position = TimeSpan.FromSeconds(value); SetProperty(ref _SongTimeLineForSlider, value); } }

        public double SongVolumeForSlider {  get => _SongVolumeForSlider; set {  model.MP.Volume = value; SetProperty(ref _SongVolumeForSlider, value); } }


        public int SelectedSongOnLB { get => _SelectedSongOnLB; set => SetProperty(ref _SelectedSongOnLB, value); }

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

        public ObservableCollection<Song> Songs { get; set; }
             = new ObservableCollection<Song>();


        private MusicContainer _selectedMusicContainer;
        private Song _selectedSong;

        public MusicContainer SelectedMusicContainer
        { 
            get => _selectedMusicContainer;
            set
            {
                SetProperty(ref _selectedMusicContainer, value);
                Songs.Clear();
                if (value != null)
                    foreach (var item in value.Songs) Songs.Add(item);
            }
        }

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


        #region Команды - Локальные папки/песни

        public RelayCommand OpenOnDiskCommand { get; private set; }

        public RelayCommand AddLocalFolderFromPCCommand { get; private set; }
        public RelayCommand AddLocalSongFromPCCommand { get; private set; }

        public RelayCommand DeleteFromAppCommand { get; private set; }

        /// <summary>Метод открытия папки на диске</summary>
        public void OpenFolderOnDiskExecute(object parameter)
        {
            if (SelectedBMO is LocalFolder)
            {
                ShowFolder((SelectedBMO as LocalFolder).LocalURL);
            }
            else
            {
                ShowFolder((SelectedBMO as Song).LocalURL);
            }
        }

        private async void AddLocalFolderFromPCExecute(object parameter) =>
            await model.AddLocalFolderFromPC(FolderBrowserDialog(), Caller.User);

        private async void AddLocalSongFromPCExecute(object parameter) =>
            await model.AddLocalSongFromPC(FileBrowserDialog(), Caller.User);

        private async void DeleteFromAppExecute(object parameter)
        {
            if (SelectedBMO is LocalFolder)
            {
                await model.DeleteLocalFolderFromPC(SelectedMusicContainer as LocalFolder, Caller.User);
            }
            else
            {
                await model.DeleteLocalSongFromPC(SelectedSong, Caller.User);
            }
        }



        #region Выключение звука при перетаскивании ползунка слайдера таймлайна.

        public RelayCommand SongPosChangedStartCommand { get; private set; }

        public RelayCommand SongPosChangedEndedCommand { get; private set; }

        private void SongPosChangedStartExecute(object parameter)
        {
            PosChanges = true;
            tempVolume = model.MP.Volume;
            model.MP.Volume = 0;
        }

        private void SongPosChangedEndedExecute(object parameter)
        {
            model.MP.Position = TimeSpan.FromSeconds(SongTimeLineForSlider);
            PosChanges = false;
            model.MP.Volume = tempVolume;
        }

        private bool PosChanges;
        private double tempVolume;

        #endregion


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




        #region Обработка событий


        private void Model_SongPositionChanged(TimeSpan ts)
        {
            if (PosChanges == false)
            {
                SongPos = ts.ToString(@"mm\:ss");
                SongTimeLineForSlider = ts.TotalSeconds;
            }
        }

        //private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        //{
        //    DoWork(((Slider)sender).Value);
           
        //}

        private void Model_LocalFolderAdded(LocalFolder LocalFolder, Caller caller)
        {

            LocalFiles.Add(LocalFolder as BaseMusicObject);
           
        }

        private void Model_LocalFolderDeleted(LocalFolder LocalFolder, Caller caller)
        {
            LocalFiles.Remove(LocalFolder);
        }

        private void Model_SongAdded(Song song, Caller caller)
        {
            LocalFiles.Add(song);
        }

        private void Model_SongDeleted(Song song, Caller caller)
        {
            LocalFiles.Remove(song);
        }

        private void Model_SongChanged(Song song, Caller caller)// => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
        {
            switch (caller)
            {
                case Caller.User:
                    UpdataSongInfo();
                    model.MP.Play();

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

        }//));

        private void Model_SongImageChanged(Song song, Caller caller)
        {
            SongBitmapImage = (BitmapImage)song.Image.Image;
        }

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

        #endregion




        #region Конструкторы

        public MainWindowVM()
        {
            try
            {
                InitiailizeCommands();

                model = new MainWindowM();

                model.LocalFolderAdded += Model_LocalFolderAdded;
                model.LocalFolderDeleted += Model_LocalFolderDeleted;

                model.SongAdded += Model_SongAdded;
                model.SongDeleted += Model_SongDeleted;
                model.SongChanged += Model_SongChanged;
                model.SongImageChanged += Model_SongImageChanged;

                model.SongPositionChanged += Model_SongPositionChanged;

                LocalFiles = new ObservableCollection<BaseMusicObject>(model.GetListLocalFiles());

                //SongBitmapImage = model.CurrentImage;

                model.MP.Volume = 0.15;
                SongVolumeForSlider = 0.15;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        



        /// <summary>
        /// Инициализация команд
        /// </summary>
        public override void InitiailizeCommands()
        {
            base.InitiailizeCommands();

            OpenOnDiskCommand = new RelayCommand(OpenFolderOnDiskExecute, null);

            AddLocalFolderFromPCCommand = new RelayCommand(AddLocalFolderFromPCExecute, null);
            AddLocalSongFromPCCommand = new RelayCommand(AddLocalSongFromPCExecute, null);

            DeleteFromAppCommand = new RelayCommand(DeleteFromAppExecute, null);

            SongPosChangedStartCommand = new RelayCommand(SongPosChangedStartExecute, null);
            SongPosChangedEndedCommand = new RelayCommand(SongPosChangedEndedExecute, null);

            PrevSongCommand = new RelayCommand(PrevSongExecute, null);
            PlayStateSongCommand = new RelayCommand(PlayStateSongExecute, null);
            NextSongCommand = new RelayCommand(NextSongExecute, null);
        }

        #endregion
    }
}
