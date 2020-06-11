using FierceStukCloud_NetCoreLib.Models;
using FierceStukCloud_NetCoreLib.Models.AbstractModels;
using FierceStukCloud_NetCoreLib.Models.MusicContainers;
using FierceStukCloud_NetCoreLib.Services.ImageAsyncS;
using FierceStukCloud_PC.MVVM.Models.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static FierceStukCloud_NetCoreLib.Types.CallerType;

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
                song.Image = new ImageAsync()
                {
                    ImageDefault = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png")),
                    ImageUri = song.LocalURL
                };
                CurrentImage = (BitmapImage)song.Image.Image;
                MP.Open(new Uri(song.LocalURL));
                CurrentMusicContainer = song.CurrentMusicContainer;
            }
            catch(Exception)
            {

            }
            return song;
        }

        #endregion


        #region Добавление/Удаление контейнеров

        public List<BaseMusicObject> GetLocalFiles()
        {
            var temp = new List<BaseMusicObject>();
            temp.AddRange(MWM_LocalDB.GetLocalFoldersFromLocalDB().Result);

            return temp;
        }

        public async void AddLocalFolderFromPC(string path, Caller caller) =>
            LocalFolderAdded?.Invoke(await MWM_LocalDB.AddLocalFoldersFromPC(path) , caller);
        

        public async void DeleteMusicContainerFromPC<T>(T MusicContainer) where T : MusicContainer
        {

        }

        #endregion


        #region События

        public delegate void SongInfo(Song song, Caller caller);
        public event SongInfo SongAdded;
        public event SongInfo SongDeleted;
        public event SongInfo SongChanged;

        public delegate void LocalFolderInfo(LocalFolder LocalFolder, Caller caller);
        public event LocalFolderInfo LocalFolderAdded;
        public event LocalFolderInfo LocalFolderDeleted;
        public event LocalFolderInfo LocalFolderChanged;

        #endregion


        public MainWindowM()
        {
            MP = new MediaPlayer();
            MWM_LocalDB = new MWM_LocalDB(App.Connection);

           
            
        }
    }
}
