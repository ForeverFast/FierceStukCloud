using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Core.Services;
using FierceStukCloud.Mvvm;
using FierceStukCloud.Wpf.Services;
using FierceStukCloud_Mobile.MVVM.Models.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using static FierceStukCloud.Core.CustomEnums;

namespace FierceStukCloud_Mobile.Models
{
    public class MusicPlayerM : OnPropertyChangedClass
    {
        #region Переменные плеера

        /// <summary> Переменная плеера </summary>      
        //public MediaPlayer MP { get; }
        
        public PhoneMode PhoneMode { get; set; } 
        public DeviceType TargerDevice { get; set; }


        private IDataService _dbService { get; }
        private ISignalRService _signalRService { get; }

        /// <summary> Список плейлистов </summary>   
        public List<PlayList> PlayLists { get; set; }
        /// <summary> Список альбомов </summary>   
        public List<Album> Albums { get; set; }
   
       
        /// <summary> Tекущий контейнер </summary>   
        public IMusicContainer CurrentMusicContainer { get; set; }
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
               // MWM_SignalR.MusicPlayerCommand(Commands.PlaySong, TargerDevice);
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
               //MWM_SignalR.MusicPlayerCommand(Commands.PauseSong, TargerDevice);
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
               // MWM_SignalR.MusicPlayerCommand(Commands.StopSong, TargerDevice);
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
        //public List<BaseMusicObject> GetListLocalFiles()
        //{
        //    var temp = new List<BaseMusicObject>();
        //    if (PhoneMode == PhoneMode.RemoteСontroller)
        //    {
        //        Task.Run(() => _signalRService.MusicPlayerCommand(Commands.GetSongs, TargerDevice));
        //        return temp;
        //    }
        //    else
        //    {
        //        //temp.AddRange(MWM_LocalDB.GetListLocalFolders());
        //        //temp.AddRange(MWM_LocalDB.LocalSongs);
        //    }
        //    return LocalFiles = temp;
        //}

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
               // Task.Run(() => _signalRService.MusicPlayerCommand(Commands.PrevSong, TargerDevice));
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
              //  Task.Run(() => _signalRService.MusicPlayerCommand(Commands.NextSong, TargerDevice));
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
        public void SetCurrentSong(Song song, int parameter = 0)
        {
            try
            {
                //if (song.CurrentMusicContainer == null)
                //    song.CurrentMusicContainer = CurrentMusicContainer.ToMC();
                //else
                //    CurrentMusicContainer = song.CurrentMusicContainer;

                CurrentSong = song;

                switch (parameter)
                {
                    case 0:

                        if (PhoneMode == PhoneMode.RemoteСontroller)
                        {
                            //Task.Run(() => _signalRService.SetCurrentSongCommand(TargerDevice, CurrentSong));
                            OnPropertyChanged("MP_MediaOpened");
                        }
                        else
                        {
                            
                        }
                        

                        break;

                    case 1:

                        OnPropertyChanged("MP_MediaOpened");

                        break;
                }


               

                

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
            catch (Exception ex)
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
            //if (CurrentSong.LocalID == CurrentMusicContainer.ToLF().Songs.Count)
            //{
                //MP.Stop();
            //   return;
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

        private Timer timer;

        public void Timer_Tick(object sender, EventArgs e)
        {
            //if (MP.Source != null)
            OnPropertyChanged();
        }

        #endregion

        #region События Хаба

        //private void MWM_SignalR_UpdateInfoFromPC(List<BaseMusicObject> obj)
        //{
        //    LocalFiles = obj;
        //    OnPropertyChanged("UpdateInfoFromPC");
        //}

        private void MWM_SignalR__NewCurrentSong(Song obj)
        {
            SetCurrentSong(obj,1);
        }

        #endregion


        public void ShutDownConnection() => _signalRService.Disconnect();

        #endregion



        #region Конструкторы 

        public MusicPlayerM()
        {
            PhoneMode = PhoneMode.RemoteСontroller;
            
            //MP = new MediaPlayer();
            //MP.MediaEnded += MP_MediaEnded;
            //MP.MediaOpened += MP_MediaOpened;

            //timer = new Timer() { Interval = 17 };
            //timer.Elapsed += Timer_Tick;

            //MWM_LocalDB = new MWM_LocalDB(App.Connection);
           // _signalRService = new MWM_SignalR();
           // _signalRService.UpdateInfoFromPC += MWM_SignalR_UpdateInfoFromPC;
            //_signalRService._NewCurrentSong += MWM_SignalR__NewCurrentSong;

        }

        #endregion
    }
}
