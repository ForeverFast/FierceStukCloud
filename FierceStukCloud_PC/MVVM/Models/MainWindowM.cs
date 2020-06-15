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
using System.IO;

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

        public async Task PrevSong(Caller caller = Caller.User)
        {
            if (CurrentSong.LocalID - 1 < 0)
            { 
                await SetCurrentSong(CurrentSong, caller);
                return;
            }

            var temp = (CurrentMusicContainer as MusicContainer).Songs.Find(x => x.LocalID == CurrentSong.LocalID - 1);
                await SetCurrentSong(temp, caller);
        }

        public async Task NextSong(Caller caller = Caller.User)
        {
            if (CurrentSong.LocalID + 1 > (CurrentMusicContainer as MusicContainer).Songs.Count)
            {
                await SetCurrentSong(CurrentSong, caller);
                return;
            }

            var temp = (CurrentMusicContainer as MusicContainer).Songs.Find(x => x.LocalID == CurrentSong.LocalID + 1);
            await SetCurrentSong(temp, caller);
        }

        public async Task SetCurrentSong(Song song, Caller caller = Caller.User)
        {
            try
            {
                CurrentSong = song;

                // Загрузка изображения
                try
                {
                    TagLib.File file_TAG = TagLib.File.Create(song.LocalURL);
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
                
                MP.Open(new Uri(CurrentSong.LocalURL));

                if (CurrentSong.CurrentMusicContainer == null)
                    CurrentSong.CurrentMusicContainer = CurrentMusicContainer as MusicContainer;
                else
                    CurrentMusicContainer = CurrentSong.CurrentMusicContainer;

                SongChanged?.Invoke(CurrentSong, caller);
            }
            catch(Exception)
            {

            }
        }

        #endregion


        #region Добавление/Удаление музыки

        /// <summary>
        /// Добавление песни в приложение
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task AddLocalSongFromPC(string path, Caller caller = Caller.User) =>
            SongAdded?.Invoke(await Task.Run(() => MWM_LocalDB.AddSongFromPC(path)), caller);

        /// <summary>
        /// Удаление песни из приложения
        /// </summary>
        /// <param name="song"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task DeleteLocalSongFromPC(Song song, Caller caller = Caller.User) =>
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
        public async Task AddLocalFolderFromPC(string path, Caller caller = Caller.User) =>
            LocalFolderAdded?.Invoke(await Task.Run(() => MWM_LocalDB.AddLocalFoldersFromPC(path)), caller);
            
        /// <summary>
        /// Удаление папки из приложения
        /// </summary>
        /// <param name="localFolder"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task DeleteLocalFolderFromPC(LocalFolder localFolder, Caller caller = Caller.User) =>
            LocalFolderDeleted?.Invoke(await Task.Run(() => MWM_LocalDB.DeleteLocalFolderFromApp(localFolder)), caller);    
        
           

        #endregion


        #region События

        public delegate void SongInfo(Song song, Caller caller);
        public event SongInfo SongAdded;
        public event SongInfo SongDeleted;
        public event SongInfo SongChanged;

        public delegate void SongPositionInfo(TimeSpan ts);
        public event SongPositionInfo SongPositionChanged;

        public delegate void LocalFolderInfo(LocalFolder LocalFolder, Caller caller);
        public event LocalFolderInfo LocalFolderAdded;
        public event LocalFolderInfo LocalFolderDeleted;
        //public event LocalFolderInfo LocalFolderChanged;

        private async void MP_MediaEnded(object sender, EventArgs e)
        {
            if (IsRandomSong == true)
            {

            }

            if (IsRepeatSong == true)
            {
                await SetCurrentSong(CurrentSong, Caller.Program);
            }

            await NextSong(Caller.Program);
        }

        #region Таймер

        private DispatcherTimer timer;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MP.Source != null)
                SongPositionChanged?.Invoke(MP.Position);
        }

        #endregion


        #endregion



        #region Конструкторы 

        public MainWindowM()
        {
            MP = new MediaPlayer();
            MP.MediaEnded += MP_MediaEnded;

            MWM_LocalDB = new MWM_LocalDB(App.Connection);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

       

        #endregion
    }
}
