using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Models.AbstractModels;
using FierceStukCloud_NetCoreLib.Models.MusicContainers;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using FierceStukCloud_PC.MVVM.Models.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static FierceStukCloud_NetCoreLib.Types.CallerType;
using static FierceStukCloud_NetCoreLib.Services.Extension.DialogService;
using System.Diagnostics.Tracing;
using System.Windows.Threading;

namespace FierceStukCloud_PC.MVVM.Models
{
    public class MainWindowM
    {
        #region Переменные плеера

        /// <summary> Переменная плеера </summary>      
        public MediaPlayer MP { get; }
        private MWM_LocalDB MWM_LocalDB { get; }

        /// <summary> Список плейлистов </summary>   
        public List<PlayList> PlayLists { get; set; }
        /// <summary> Список альбомов </summary>   
        public List<Album> Albums { get; set; }
        /// <summary> Список папок </summary>   
        public List<LocalFolder> LocalFolders { get; set; }

        /// <summary> Tекущий контейнер </summary>   
        public BaseMusicObject CurrentMusicContainer { get; set; }
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
        public void PlayState()
        {
            if (MP.Source != null)
            {
                if (IsPlaying == true)
                {
                    MP.Pause();
                    IsPlaying = false;
                }
                else
                {
                    MP.Play();
                    IsPlaying = true;
                }
            }

        }
        /// <summary> Пауза </summary>
        public void Pause()
        {
            if (MP.Source != null)
                MP.Pause();
        }
        /// <summary> остановка воспроизведения </summary>
        public void Stop()
        {
            if (MP.Source != null)
                MP.Stop();
        }

        #endregion


        #region Методы выбора/установки текущей песни/плейлиста

        public async Task<Song> PrevSong(Caller caller, int counter = 1)
        {
            var temp = (CurrentMusicContainer as MusicContainer).Songs.Find(x => x.ID == CurrentSong.ID - counter);

            if (temp != null)
            {
                if (temp.GetType().Name == "Song")
                {
                    if (temp.ID == 0)
                        return SetCurrentSong(CurrentSong, caller);

                    return SetCurrentSong(temp, caller);

                }
                else
                    return PrevSong(caller, ++counter).Result;
            }
            else
                return SetCurrentSong(CurrentSong, caller);
        }

        public async Task<Song> NextSong(Caller caller, int counter = 1)
        {
            var temp = (CurrentMusicContainer as MusicContainer).Songs.Find(x => x.ID == CurrentSong.ID + counter);

            if (temp != null)
            {
                if (temp.GetType().Name == "Song")
                {
                    if (temp.ID == (CurrentMusicContainer as MusicContainer).Songs.Count)
                        return SetCurrentSong(CurrentSong, caller);

                    return SetCurrentSong(temp, caller);

                }
                else
                    return NextSong(caller, ++counter).Result;
            }
            else
                return SetCurrentSong(CurrentSong, caller);
        }

        public Song SetCurrentSong(Song song, Caller caller)
        {
            try
            {
                CurrentSong = song;
                CurrentSong.Image = new ImageAsync()
                {
                    ImageDefault = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png")),
                };
                CurrentSong.Image.PropertyChanged += SongImage_PropertyChanged;
                CurrentSong.Image.ImageUri = CurrentSong.LocalURL;

                MP.Open(new Uri(CurrentSong.LocalURL));

               
              
                CurrentMusicContainer = CurrentSong.CurrentMusicContainer;
                CurrentImage = (BitmapImage)CurrentSong.Image.Image;      
                
                SongChanged?.Invoke(CurrentSong, caller);
            }
            catch(Exception ex)
            {

            }
            return CurrentSong;
        }

       

        #endregion



        #region Добавление/Удаление музыки

        /// <summary>
        /// Добавление песни в приложение
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task AddLocalSongFromPC(string path, Caller caller) =>
            SongAdded?.Invoke(await Task.Run(() => MWM_LocalDB.AddSongFromPC(path)), caller);

        /// <summary>
        /// Удаление песни из приложения
        /// </summary>
        /// <param name="song"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task DeleteLocalSongFromPC(Song song, Caller caller) =>
            SongDeleted?.Invoke(await Task.Run(() => MWM_LocalDB.DeleteSongFromApp(song)), caller);

        #endregion


        #region Добавление/Удаление контейнеров

        /// <summary>
        /// Получение списка локальных файлов
        /// </summary>
        /// <returns></returns>
        public List<BaseMusicObject> GetListLocalFiles()
        {
            var temp = new List<BaseMusicObject>();
            temp.AddRange(MWM_LocalDB.GetListLocalFolders());
            temp.AddRange(MWM_LocalDB.LocalSongs);
            return temp;
        }

        /// <summary>
        /// Добавление папки в приложение
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task AddLocalFolderFromPC(string path, Caller caller) =>
            LocalFolderAdded?.Invoke(await Task.Run(() => MWM_LocalDB.AddLocalFoldersFromPC(path)), caller);
            
        /// <summary>
        /// Удаление папки из приложения
        /// </summary>
        /// <param name="localFolder"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task DeleteLocalFolderFromPC(LocalFolder localFolder, Caller caller) =>
            LocalFolderDeleted?.Invoke(await Task.Run(() => MWM_LocalDB.DeleteLocalFolderFromApp(localFolder)), caller);    
        
           

        #endregion


        #region События

        public delegate void SongInfo(Song song, Caller caller);
        public event SongInfo SongAdded;
        public event SongInfo SongDeleted;
        public event SongInfo SongChanged;
        public event SongInfo SongImageChanged;

        public delegate void SongPositionInfo(TimeSpan ts);
        public event SongPositionInfo SongPositionChanged;

        public delegate void LocalFolderInfo(LocalFolder LocalFolder, Caller caller);
        public event LocalFolderInfo LocalFolderAdded;
        public event LocalFolderInfo LocalFolderDeleted;
        public event LocalFolderInfo LocalFolderChanged;

        #region Таймер

        private DispatcherTimer timer;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MP.Source != null)
                SongPositionChanged?.Invoke(MP.Position);
        }

        #endregion

        private void SongImage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SongImageChanged?.Invoke(CurrentSong, Caller.Program);
            CurrentImage = (BitmapImage)CurrentSong.Image.Image;
        }

        #endregion



        #region Конструкторы 
        public MainWindowM()
        {
            MP = new MediaPlayer();
            MWM_LocalDB = new MWM_LocalDB(App.Connection);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        
        #endregion
    }
}
