using FierceStukCloud.Core;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using static FierceStukCloud.Core.CustomEnums;

namespace FierceStukCloud.Pc.Services
{
    public class MusicPlayerService : OnPropertyChangedClass, IMusicPlayerService
    {
        #region Переменные плеера

        #region Поля

        private readonly IDataService _dataService;
        private readonly IMusicStorage _musicStorage;
        private readonly ISignalRService _signalRService;

        private LoopMode _isRepeatSong;
        private bool _isRandomSong;

        #endregion

        /// <summary> Переменная плеера </summary>      
        public MediaPlayer MP { get; }

       

        /// <summary> Tекущий контейнер </summary>   
        public IMusicContainer CurrentMusicContainer { get; set; }
        /// <summary> Tекущий отображаемый контейнер </summary>   
        public IMusicContainer DisplayedMusicContainer { get; set; }
        /// <summary> Tекущая песня </summary>   
        public Song CurrentSong { get => CurrentSongNode?.Value;
                                  set => SetCurrentSong(value); }
        private LinkedListNode<Song> CurrentSongNode { get; set; }


        public LoopMode IsRepeatSong { get => _isRepeatSong; set => SetProperty(ref _isRepeatSong, value); }
        public bool IsRandomSong { get => _isRandomSong; set => SetProperty(ref _isRandomSong, value); }
        public bool IsPlaying { get => CurrentSong == null ? false : CurrentSong.IsPlaying; set => CurrentSong.IsPlaying = value; }

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

        public TimeSpan Duration
        {
            get => MP.NaturalDuration.TimeSpan;
        }


        #endregion

        #region Методы работы с файлами

        #region Методы добавления/удаления песен на устройстве

        public async Task<Song> AddSongFromDevice(string path, string ContainerId = "")
            => await _dataService.AddSongAsync(path, ContainerId);

        public async Task<bool> RemoveSongFromDevice(Song song)
        {
            await Task.Delay(0);
            return true;
        }

        public async Task<bool> RemoveSongFromApp(Song song)
            => await _dataService.RemoveSongAsync(song);

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
            => await Task.Run(() => _dataService.AddLocalFolderAsync(path));

        public async Task<bool> RemoveLocalFolderFromDevice(LocalFolder localFolder)
            => await Task.Run(() => _dataService.RemoveLocalFolderAsync(localFolder));

        #endregion


        #region Методы добавление/удаления плейлистов на устройстве

        public async Task AddPlayList(string title, string description, string imageUri)
            => await _dataService.AddPlayListAsync(title, description, imageUri);

        public async Task<bool> RemovePlayList(PlayList playList)
            => await _dataService.RemovePlayListAsync(playList);

        public async Task<bool> UpdatePlayList(PlayList playList)
        {
            return true;
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
            OnPropertyChanged("CurrentSong");
        }

        private void SongEnded(object sender, EventArgs e)
        {
            IsPlaying = false;
            timer.Stop();

            if (IsRepeatSong == LoopMode.LoopOne)
            {
                SetCurrentSong(CurrentSong);
                return;
            }

            //if (CurrentSong.CurrentIdValue() == CurrentMusicContainer.Songs.Count)
            //{
            //    MP.Stop();
            //    return;
            //}



            if (IsRandomSong == true)
            {
                return;
            }

           


            NextSong();
        }

        #region Таймер

        private DispatcherTimer timer;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MP.Source != null)
                OnPropertyChanged("Position");
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

       
        #endregion


        #region Конструкторы

        private MusicPlayerService()
        {
            MP = new MediaPlayer();
            MP.MediaEnded += SongEnded;
            MP.MediaOpened += SongLoaded;
           

            // Таймер
            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(17) };
            timer.Tick += Timer_Tick;
        }

        public MusicPlayerService(IDataService dataService,
                                  IMusicStorage musicStorage,
                                  ISignalRService signalRService) : this()
        {
            _dataService = dataService;
            _musicStorage = musicStorage;
            _signalRService = signalRService;

            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();

            myStopwatch.Start();
            _dataService.GetData();
            myStopwatch.Stop();

            // Инициализация класса работы с БД
            //_dataService = new DataService(AllSongs, Favourites, Albums, LocalFolders, PlayLists, );


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
