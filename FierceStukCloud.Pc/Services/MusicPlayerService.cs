using FierceStukCloud.Core;
using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Mvvm;
using FierceStukCloud.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace FierceStukCloud.Pc.Services
{
    public class MusicPlayerService : OnPropertyChangedClass, IMusicPlayerService
    {
        #region Переменные плеера

        /// <summary> Переменная плеера </summary>      
        public MediaPlayer MP { get; }

        private IDataService _dataService { get; }
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
        public IMusicContainer CurrentMusicContainer { get; set; }
        /// <summary> Tекущий отображаемый контейнер </summary>   
        public IMusicContainer DisplayedMusicContainer { get; set; }
        /// <summary> Tекущая песня </summary>   
        public Song CurrentSong
        {
            get => CurrentSongNode.Value;
            set
            {
                SetCurrentSong(value);
                OnPropertyChanged();
            }
        }
        private LinkedListNode<Song> CurrentSongNode { get; set; }


        public bool IsRepeatSong { get; set; }
        public bool IsRandomSong { get; set; }
        public bool IsPlaying { get; set; }

        public double Volume
        {
            get => MP.Volume;
            set
            {
                MP.Volume = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Position
        {
            get => MP.Position;
            set
            {
                MP.Position = value;
                OnPropertyChanged();
            }
        }




        #endregion

        #region Методы работы с файлами

        #region Методы добавления/удаления песен на устройстве

        public async Task<Song> AddSongFromDevice(string path)
            => await _dataService.AddSong(path);

        public async Task<bool> RemoveSongFromDevice(Song song)
        {
            await Task.Delay(0);
            return true;
        }

        public async Task<bool> RemoveSongFromApp(Song song)
            => await _dataService.RemoveSong(song);

        #endregion


        #region Методы добавления/удаления песен на сервере 

        public async Task<bool> AddSongToServer(Song song)
        {
            await Task.Delay(0);
            return true;
        }

        public async Task<bool> RemoveSongFromServer(Song song)
        {
            await Task.Delay(0);
            return true;
        }

        #endregion

        public Task<bool> UpdateSongInfo(Song song)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Методы работы с контейнерами

        #region Методы добавления/удаления папок на устройстве

        public async Task<LocalFolder> AddLocalFolderFromDevice(string path)
            => await Task.Run(() => _dataService.AddLocalFolder(path));

        public async Task<bool> RemoveLocalFolderFromDevice(LocalFolder localFolder)
            => await Task.Run(() => _dataService.RemoveLocalFolder(localFolder));

        #endregion


        #region Методы добавление/удаления плейлистов на устройстве

        public async Task<PlayList> AddPlayList(string title, string description)
        {

        }

        public async Task<bool> RemovePlayList(PlayList playList)
        {

        }

        public async Task<bool> UpdatePlayList(PlayList playList)
        {

        }

        #endregion

        #endregion


        #region Управление состоянием воспроизведения

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


        #region Методы выбора/установки текущей песни/плейлиста

        public void PrevSong()
        {
            MP.Stop();

            if (CurrentSong == CurrentMusicContainer.Songs.First.Value)
            {
                SetCurrentSong(CurrentSong);
                return;
            }

            var temp = CurrentMusicContainer.Songs.Find(CurrentSong).Previous.Value;
            SetCurrentSong(temp);
        }
        public void NextSong()
        {
            MP.Stop();

            if (CurrentSong == CurrentMusicContainer.Songs.Last.Value)
            {
                SetCurrentSong(CurrentSong);
                return;
            }

            var temp = CurrentMusicContainer.Songs.Find(CurrentSong).Next.Value;
            SetCurrentSong(temp);
        }

        /// <summary>
        /// Установить текущий трек
        /// </summary>
        /// <param name="song"></param>
        private void SetCurrentSong(Song song)
        {
            try
            {
                if (song.CurrentMusicContainer == null)
                    song.CurrentMusicContainer = CurrentMusicContainer;
                else
                    CurrentMusicContainer = song.CurrentMusicContainer;

                CurrentSongNode = CurrentMusicContainer.Songs.Find(song);            
                MP.Open(new Uri(CurrentSong.LocalUrl));
            }
            catch (Exception)
            {

            }
        }

        #endregion


        #region События

        private void SongLoaded(object sender, EventArgs e)
        {
            MP.Play();
            IsPlaying = true;
            timer.Start();
            OnPropertyChanged();
        }

        private void SongEnded(object sender, EventArgs e)
        {
            //if (CurrentSong.CurrentIdValue() == CurrentMusicContainer.Songs.Count)
            //{
            //    MP.Stop();
            //    return;
            //}

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

        private void Timer_Tick(object sender, EventArgs e)
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

                        //var json1 = JsonSerializer.Serialize<ObservableCollection<LocalFolder>>(LocalFolders);
                        //var json2 = JsonSerializer.Serialize<LocalFolder>(LocalSongs);
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

        private MusicPlayerService()
        {
            MP = new MediaPlayer();
            MP.MediaEnded += SongEnded;
            MP.MediaOpened += SongLoaded;

            LocalSongs = new LocalFolder() { Title = "LF" };
            Albums = new ObservableCollection<Album>();
            LocalFolders = new ObservableCollection<LocalFolder>();
            PlayLists = new ObservableCollection<PlayList>();

            // Таймер
            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(17) };
            timer.Tick += Timer_Tick;
        }

        public MusicPlayerService(User user) : this()
        {
            // Инициализация класса работы с БД
            _dataService = new DataService(LocalSongs, Albums, LocalFolders, PlayLists, user);
            //_dbService.GetData();

            // Инициализация класса работы с SignalR
            //_signalRService = new SignalRService();
            //_signalRService.PropertyChanged += _signalRService_PropertyChanged;
        }


        #endregion

        public void Dispose()
        {
            _signalRService.Disconnect();  
        }
    }
}
