using FierceStukCloud_Mobile.MVVM.Models.Modules;
using FierceStukCloud_NetStandardLib.Extension;
using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using FierceStukCloud_NetStandardLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using static FierceStukCloud_NetStandardLib.Types.CustomEnums;
using static FierceStukCloud_NetStandardLib.Extension.TypeConventer;

namespace FierceStukCloud_Mobile.Models
{
    public class MusicPlayerM : OnPropertyChangedClass
    {
        #region Переменные плеера

        /// <summary> Переменная плеера </summary>      
        //public MediaPlayer MP { get; }
        
        public PhoneMode PhoneMode { get; set; } 
        public DeviceType TargerDevice { get; set; }


        private MWM_LocalDB MWM_LocalDB { get; }
        private MWM_SignalR MWM_SignalR { get; }

        /// <summary> Список плейлистов </summary>   
        public List<PlayList> PlayLists { get; set; }
        /// <summary> Список альбомов </summary>   
        public List<Album> Albums { get; set; }
        /// <summary> Список папок </summary>   
        public ObservableCollection<BaseMusicObject> LocalFiles { get; set; }

        /// <summary> Tекущий контейнер </summary>   
        public BaseMusicObject CurrentMusicContainer { get; set; }
        /// <summary> Tекущая песня </summary>   
        public Song CurrentSong { get; set; } = null;

        /// <summary> Изображение текущей песни </summary>   
        //public BitmapImage CurrentImage { get; set; }

        public bool IsRepeatSong { get; set; } = false;
        public bool IsRandomSong { get; set; } = false;
        public bool IsPlaying { get; set; } = false;


        #endregion



        #region Управление состоянием воспроизведения

        /// <summary> Изменение состояние воспроизведения </summary>
        public void Play()
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                MWM_SignalR.MusicPlayerCommand(Commands.PlaySong, TargerDevice);
            }
            else
            {
                //if (MP.Source != null)
                //{
                //    MP.Play();
                //    timer.Start();
                //    IsPlaying = true;
                //    OnPropertyChanged();
                //}
            }     
        }

        /// <summary> Пауза </summary>
        public void Pause()
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                MWM_SignalR.MusicPlayerCommand(Commands.PauseSong, TargerDevice);
            }
            else
            {
                //if (MP.Source != null)
                //{
                //    MP.Pause();
                //    timer.Stop();
                //    IsPlaying = false;
                //    OnPropertyChanged();
                //}
            }       
        }

        /// <summary> Остановка воспроизведения </summary>
        public void Stop()
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                MWM_SignalR.MusicPlayerCommand(Commands.StopSong, TargerDevice);
            }
            else
            {
                //if (MP.Source != null)
                //{
                //    MP.Stop();
                //    timer.Stop();
                //    IsPlaying = false;
                //    OnPropertyChanged();
                //}
            }     
        }

        #endregion


        #region Добавление/Удаление музыки

        /// <summary>
        /// Добавление песни в приложение
        /// </summary>
        /// <param name="path"> путь к mp3 файлу</param>
        /// <returns></returns>
        public Song AddLocalSongFromPC(string path)
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                //return MWM_SignalR.MusicPlayerCommandToPC(Commands.PlaySong);
            }
            else
            {
                //MWM_LocalDB.AddSongFromPC(path);
            }
            return null;
        }
           

        /// <summary>
        /// Удаление песни из приложения
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public Song DeleteLocalSongFromPC(Song song)
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                //return MWM_SignalR.MusicPlayerCommandToPC(Commands.PlaySong);
            }
            else
            {
                // MWM_LocalDB.DeleteSongFromApp(song);
            }
            return null;
        }
       

        #endregion


        #region Добавление/Удаление контейнеров

        /// <summary>
        /// Получение списка локальных файлов
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<BaseMusicObject> GetListLocalFiles()
        {
            var temp = new ObservableCollection<BaseMusicObject>();
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                Task.Run(() => MWM_SignalR.MusicPlayerCommand(Commands.GetSongs, TargerDevice));
                return temp;
            }
            else
            {
                //temp.AddRange(MWM_LocalDB.GetListLocalFolders());
                //temp.AddRange(MWM_LocalDB.LocalSongs);
            }
            return LocalFiles = temp;
        }

        /// <summary>
        /// Добавление папки в приложение
        /// </summary>
        /// <param name="path"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public LocalFolder AddLocalFolderFromPC(string path)
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                //return MWM_SignalR.MusicPlayerCommandToPC(Commands.PlaySong);
            }
            else
            {
                //  MWM_LocalDB.AddLocalFoldersFromPC(path);
            }
            return null;
        }
      

        /// <summary>
        /// Удаление папки из приложения
        /// </summary>
        /// <param name="localFolder"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public LocalFolder DeleteLocalFolderFromPC(LocalFolder localFolder)
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                //return MWM_SignalR.MusicPlayerCommandToPC(Commands.PlaySong);
            }
            else
            {
                //  MWM_LocalDB.DeleteLocalFolderFromApp(localFolder);
            }
            return null;
        }

        #endregion


        #region Методы выбора/установки текущей песни/плейлиста

        /// <summary> Включить предыдущую песню </summary>
        public void PrevSong()
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                Task.Run(() => MWM_SignalR.MusicPlayerCommand(Commands.NextSong, TargerDevice));
            }
            else
            {
                //MP.Stop();

                //if (CurrentSong.LocalID - 1 < 0)
                //{
                //    SetCurrentSong(CurrentSong);
                //    return;
                //}

                //var temp = CurrentMusicContainer.ToMC().Songs.Find(x => x.LocalID == CurrentSong.LocalID - 1);
                //SetCurrentSong(temp);
            }      
        }

        /// <summary> Включить следующую песню </summary>
        public void NextSong()
        {
            if (PhoneMode == PhoneMode.RemoteСontroller)
            {
                Task.Run(() => MWM_SignalR.MusicPlayerCommand(Commands.NextSong, TargerDevice));
            }
            else
            {
                //MP.Stop();

                //if (CurrentSong.LocalID + 1 > CurrentMusicContainer.ToMC().Songs.Count)
                //{
                //    SetCurrentSong(CurrentSong);
                //    return;
                //}

                //var temp = CurrentMusicContainer.ToMC().Songs.Find(x => x.LocalID == CurrentSong.LocalID + 1);
                //SetCurrentSong(temp);
            }
           
        }

        /// <summary> Подготовка песни к воспроизведению </summary>
        public void SetCurrentSong(Song song)
        {
            try
            {
                if (song.CurrentMusicContainer == null)
                    song.CurrentMusicContainer = CurrentMusicContainer.ToMC();
                else
                    CurrentMusicContainer = song.CurrentMusicContainer;

                CurrentSong = song;

                Task.Run(() => MWM_SignalR.SetCurrentSongCommand(TargerDevice, CurrentSong)); 

                //MP.Open(new Uri(CurrentSong.LocalURL));


                //// Загрузка изображения
                //try
                //{
                //    TagLib.File file_TAG = TagLib.File.Create(song.LocalURL);
                //    var bin = file_TAG.Tag.Pictures[0].Data.Data; // Конвертация в массив байтов

                //    var bitmapImage = new BitmapImage();
                //    bitmapImage.BeginInit();
                //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                //    bitmapImage.StreamSource = new MemoryStream(bin);
                //    bitmapImage.EndInit();
                //    CurrentImage = bitmapImage;
                //}
                //catch (Exception)
                //{
                //    CurrentImage
                //        = new BitmapImage(new Uri("pack://application:,,,/FierceStukCloud_NetCoreLib;component/Resources/Images/fsc_icon.png"));
                //}


            }
            catch (Exception)
            {

            }
        }

        #endregion


        #region События

        public void MP_MediaOpened(object sender, EventArgs e)
        {
            //MP.Play();
            IsPlaying = true;
            timer.Start();
            OnPropertyChanged();
        }

        private void MP_MediaEnded(object sender, EventArgs e)
        {
            if (CurrentSong.LocalID == CurrentMusicContainer.ToLF().Songs.Count)
            {
                //MP.Stop();
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

        private Timer timer;

        public void Timer_Tick(object sender, EventArgs e)
        {
            //if (MP.Source != null)
            OnPropertyChanged();
        }

        #endregion

        public void ShutDownConnection() => MWM_SignalR.Disconnect();

        #endregion



        #region Конструкторы 

        public MusicPlayerM()
        {
            PhoneMode = PhoneMode.RemoteСontroller;
            TargerDevice = DeviceType.PC;
            LocalFiles = new ObservableCollection<BaseMusicObject>();
            
            //MP = new MediaPlayer();
            //MP.MediaEnded += MP_MediaEnded;
            //MP.MediaOpened += MP_MediaOpened;

            //timer = new Timer() { Interval = 17 };
            //timer.Elapsed += Timer_Tick;

            //MWM_LocalDB = new MWM_LocalDB(App.Connection);
            MWM_SignalR = new MWM_SignalR(this);
            MWM_SignalR.Update += (s) => OnPropertyChanged("GettingSongsEvent");
           

        }


        #endregion
    }
}
