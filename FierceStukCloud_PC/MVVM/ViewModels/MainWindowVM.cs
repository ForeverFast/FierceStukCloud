using FierceStukCloud_NetStandartLib.Models;
using FierceStukCloud_NetStandartLib.Models.AbstractModels;
using FierceStukCloud_NetStandartLib.Models.MusicContainers;
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
using static FierceStukCloud_NetCoreLib.Types.CustomEnums;
using static FierceStukCloud_NetCoreLib.Extension.DialogService;
using System.Threading.Tasks;
using System.Threading;

using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using System.ComponentModel;
using System.IO;
using FierceStukCloud_NetStandartLib;
using FierceStukCloud_NetStandartLib.Extension;

namespace FierceStukCloud_PC.MVVM.ViewModels
{
    public class MainWindowVM : BaseViewModel
    {

        private MainWindowM model { get; }
        private Dispatcher Dispatcher { get; }

        #region Управление плеером


        #region Отображение информации о текущем треке

        #region Поля
        private BitmapImage _songBitmapImage;
        private string _songName;
        private string _SongAuthor;

        private string _songTime = "99:99";
        private string _songPos = "00:00";
        private double _songTimeForSlider;
        private double _songTimeLineForSlider;

        private double _songVolumeForSlider;

        private int _selectedSongOnLB;

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


        public int SelectedSongOnLB { get => _selectedSongOnLB; set => SetProperty(ref _selectedSongOnLB, value); }

  
        public string SelectedStyle{ get => _selectedStyle; set => SetProperty(ref _selectedStyle, value); }
       
        #endregion


        #region Кнопки управления плеера

        public RelayCommand PrevSongCommand { get; set; }
        private void PrevSongExecute(object parameter) => model.PrevSong();
        public RelayCommand PlayStateSongCommand { get; set; }
        private void PlayStateSongExecute(object parameter)
        {
            if (model.IsPlaying == true)
                model.Pause();
            else
                model.Play();
        }
        public RelayCommand NextSongCommand { get; set; }
        private void NextSongExecute(object parameter) => model.NextSong();

        #endregion


        #region Свойства

        public ImageAsyncCollection<ImageAsync<Song>> Songs { get; set; }

        private MusicContainer _selectedMusicContainer;
        private ImageAsync<Song> _selectedSong;

        public MusicContainer SelectedMusicContainer
        { 
            get => _selectedMusicContainer;
            set
            {
                SetProperty(ref _selectedMusicContainer, value);
                Songs.Clear();
                if (value != null)
                    foreach (var item in value.Songs)
                        Songs.Add(new ImageAsync<Song>
                            (
                                Dispatcher,
                                item.LocalURL,
                                item
                            ));
            }
        }

        public ImageAsync<Song> SelectedSong
        {
            get => _selectedSong;
            set
            {
                if (value != null)
                {
                    value.Content.CurrentMusicContainer = SelectedMusicContainer != null ?
                                                          SelectedMusicContainer : new LocalFolder() { Songs = GetLocalFilesAsMC() };

                    model.SetCurrentSong(value.Content);
                    SetProperty(ref _selectedSong, value);
                }
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

        private async void AddLocalFolderFromPCExecute(object parameter)
        {
            var path = FolderBrowserDialog();
            LocalFiles.Add(await Task.Run(() => model.AddLocalFolderFromPC(path)));
        }

        private async void AddLocalSongFromPCExecute(object parameter)
        {
            var path = FolderBrowserDialog();
            LocalFiles.Add(await Task.Run(() => model.AddLocalSongFromPC(path)));
        }
        private async void DeleteFromAppExecute(object parameter)
        {
            model.Stop();
            if (SelectedBMO is LocalFolder)
                LocalFiles.Remove(await Task.Run(() => model.DeleteLocalFolderFromPC(SelectedMusicContainer.ToLF())));
            else
                LocalFiles.Remove(await Task.Run(() => model.DeleteLocalSongFromPC(SelectedSong.Content)));        
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
                    SelectedSong = new ImageAsync<Song>(
                        Dispatcher,
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

        private List<Song> GetLocalFilesAsMC() => (from temp in LocalFiles where temp is Song select temp) as List<Song>;
        

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

                model = new MainWindowM();
                model.PropertyChanged += Model_PropertyChanged;

                LocalFiles = new ObservableCollection<BaseMusicObject>(model.GetListLocalFiles());

                //SongBitmapImage = model.CurrentImage;

                model.MP.Volume = 0.15;
                SongVolumeForSlider = 0.15;

                SelectedStyle = "PlayButton";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public MainWindowVM(Dispatcher dispatcher) : this()
        {
            Dispatcher = dispatcher;
            Songs = new ImageAsyncCollection<ImageAsync<Song>>
            (
                Dispatcher,
                new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"))
            );
           
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

        public override void CloseWindowMethod(object parameter)
        {
            model.ShutDownConnection();
            base.CloseWindowMethod(parameter);
        }

        #endregion
    }
}
