using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using FierceStukCloud_PC.MVVM.Models.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static FierceStukCloud_NetCoreLib.Services.DialogService;
using static FierceStukCloud_NetStandardLib.Extension.TypeConventer;
using System.Diagnostics.Tracing;
using System.Windows.Threading;
using System.IO;
using FierceStukCloud_NetCoreLib.Services;
using System.Collections.ObjectModel;
using FierceStukCloud_NetStandardLib.MVVM;
using System.Windows;
using System.Text.Json;
using FierceStukCloud_NetStandardLib.Services;

namespace FierceStukCloud_PC.MVVM.Models
{
    public class MainWindowM : OnPropertyChangedClass
    {
        #region Переменные плеера

        /// <summary> Переменная плеера </summary>      
        public MediaPlayer MP { get; }

        private IDataService _dbService { get; }
        private ISignalRService _signalRService { get; }

        /// <summary> Список плейлистов </summary>   
        public ObservableCollection<PlayList> PlayLists { get; set; }
        /// <summary> Список альбомов </summary>   
        public ObservableCollection<Album> Albums { get; set; }
        /// <summary> Список папок </summary>   
        public ObservableCollection<LocalFolder> LocalFolders { get; set; }
        /// <summary> Список песен </summary>   
        public LocalFolder LocalSongs { get; set; }



        /// <summary> Tекущий контейнер </summary>   
        public MusicContainer CurrentMusicContainer { get; set; }
        /// <summary> Tекущий отображаемый контейнер </summary>   
        public MusicContainer DisplayedMusicContainer { get; set; }


        /// <summary> Tекущая песня </summary>   
        public Song CurrentSong { get; set; } = null;
        /// <summary> Изображение текущей песни </summary>   
        public BitmapImage CurrentImage { get; set; }

        public bool IsRepeatSong { get; set; } = false;
        public bool IsRandomSong { get; set; } = false;
        public bool IsPlaying { get; set; } = false;


        #endregion


        #region Управление состоянием воспроизведения

        /// <summary> Изменение состояние воспроизведения </summary>
        public void Play()
        {
            if (MP.Source != null)
            {
                MP.Play();
                timer.Start();
                IsPlaying = true;
                OnPropertyChanged();
            }
        }

        /// <summary> Пауза </summary>
        public void Pause()
        {
            if (MP.Source != null)
            {
                MP.Pause();
                timer.Stop();
                IsPlaying = false;
                OnPropertyChanged();
            }
        }

        /// <summary> Остановка воспроизведения </summary>
        public void Stop()
        {
            if (MP.Source != null)
            {
                MP.Stop();
                timer.Stop();
                IsPlaying = false;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Добавление/Удаление музыки

        /// <summary>
        /// Добавление песни в приложение
        /// </summary>
        /// <param name="path"> путь к mp3 файлу</param>
        /// <returns></returns>
        public async Task<Song> AddLocalSongFromPC(string path) 
            => await _dbService.AddSong(path);

        /// <summary>
        /// Удаление песни из приложения
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public async Task<bool> DeleteLocalSongFromPC(Song song)
            => await _dbService.RemoveSong(song);
       
        #endregion


        #region Добавление/Удаление контейнеров

        /// <summary>
        /// Получение списка локальных файлов
        /// </summary>
        /// <returns></returns>
        public List<BaseMusicObject> GetLocalData()
        {
            var temp = new List<BaseMusicObject>();
            temp.AddRange(this.LocalFolders);
            temp.AddRange(this.LocalSongs.Songs);
            return temp;
        }

        /// <summary>
        /// Добавление папки в приложение
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caller"></param>
        /// <returns></returns>а
        public async Task<LocalFolder> AddLocalFolderFromPC(string path)
            => await _dbService.AddLocalFolder(path);
        
        /// <summary>
        /// Удаление папки из приложения
        /// </summary>
        /// <param name="localFolder"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task<bool> RemoveLocalFolderFromPC(LocalFolder localFolder)
            => await _dbService.RemoveLocalFolder(localFolder);

        #endregion


        #region Методы выбора/установки текущей песни/плейлиста

        /// <summary> Включить предыдущую песню </summary>
        public void PrevSong()
        {
            MP.Stop();
            
            if (CurrentSong.CurrentIdValue() - 1 < 0)
            {
                SetCurrentSong(CurrentSong);
                return;
            }

            var temp = CurrentMusicContainer.ToMC().Songs.Find(x => x.IdValueInMC(CurrentMusicContainer) == CurrentSong.CurrentIdValue() - 1);
            SetCurrentSong(temp);
        }

        /// <summary> Включить следующую песню </summary>
        public void NextSong()
        {
            MP.Stop();

            if (CurrentSong.CurrentIdValue() + 1 > CurrentMusicContainer.ToMC().Songs.Count)
            {
                SetCurrentSong(CurrentSong);
                return;
            }

            var temp = CurrentMusicContainer.ToMC().Songs.Find(x => x.IdValueInMC(CurrentMusicContainer) == CurrentSong.CurrentIdValue() + 1);
            SetCurrentSong(temp);
        }

        /// <summary> Подготовка песни к воспроизведению </summary>
        public void SetCurrentSong(Song song)// => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
        {
            try
            {
                if (song.CurrentMusicContainer == null)
                    song.CurrentMusicContainer = CurrentMusicContainer.ToMC();
                else
                    CurrentMusicContainer = song.CurrentMusicContainer;

                CurrentSong = song;
                MP.Open(new Uri(CurrentSong.LocalUrl));


                // Загрузка изображения
                try
                {
                    TagLib.File file_TAG = TagLib.File.Create(song.LocalUrl);
                    var bin = file_TAG.Tag.Pictures[0].Data.Data; // Конвертация в массив байтов

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = new MemoryStream(bin);
                    bitmapImage.EndInit();
                    CurrentImage = bitmapImage;
                }
                catch (Exception)
                {
                    CurrentImage
                        = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));
                }

                
            }
            catch (Exception ex)
            {

            }
        }//));

        #endregion


        #region События

        public void MP_MediaOpened(object sender, EventArgs e)
        {
            MP.Play();
            IsPlaying = true;
            timer.Start();
            OnPropertyChanged();
        }

        private void MP_MediaEnded(object sender, EventArgs e)
        {
            if (CurrentSong.CurrentIdValue() == CurrentMusicContainer.ToLF().Songs.Count)
            {
                MP.Stop();
                return;
            }

            if (IsRandomSong == true)
            {
                return;
            }

            if (IsRepeatSong == true)
            {
                SetCurrentSong(CurrentSong);
                return;
            }

            
            NextSong();
        }

        #region Таймер

        private DispatcherTimer timer;

        public void Timer_Tick(object sender, EventArgs e)
        {
            if (MP.Source != null)
                OnPropertyChanged();
        }

        #endregion

        #region Хаб

        private void _signalRService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case "GetSongs":

                        var json1 = JsonSerializer.Serialize<ObservableCollection<LocalFolder>>(LocalFolders);
                        var json2 = JsonSerializer.Serialize<LocalFolder>(LocalSongs);

                        //_signalRService.
                        //await HubConnection.SendAsync("SendSongsCommand", DeviceType.PC, deviceFrom, json1, json2);

                        break;

                    case "PrevSong":

                        this.PrevSong();
                        break;
                    case "NextSong":

                        this.NextSong();
                        break;
                    case "PlaySong":

                        this.Play();
                        break;
                    case "PauseSong":

                        this.Pause();
                        break;
                    case "StopSong":

                        this.Stop();
                        break;

                }
            }
        }

        #endregion

        public void ShutDownConnection()
        {
         //   _signalRService.Disconnect();
        }
        #endregion



        #region Конструкторы 

        public MainWindowM()
        {
            // Инициализация плеера
            MP = new MediaPlayer();
            MP.MediaEnded += MP_MediaEnded;
            MP.MediaOpened += MP_MediaOpened;

            LocalSongs = new LocalFolder() { Title = "LF" };
            Albums = new ObservableCollection<Album>();
            LocalFolders = new ObservableCollection<LocalFolder>();
            PlayLists = new ObservableCollection<PlayList>();


            // Таймер
            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(17) };
            timer.Tick += Timer_Tick;

            // Инициализация класса работы с БД
            _dbService = new DataService(LocalSongs, Albums, LocalFolders, PlayLists, App.CurrentUser);
            _dbService.GetData();

            // Инициализация класса работы с SignalR
            //_signalRService = new SignalRService();
            //_signalRService.PropertyChanged += _signalRService_PropertyChanged;
        }



        #endregion

        #region Дополнительные методы

       

        #endregion
    }
}
