using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FierceStukCloud.Core.Services
{
    public interface IMusicPlayerService : IFileManager, IDisposable
    {
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Tекущий контейнер </summary>   
        IMusicContainer CurrentMusicContainer { get; set; }
        /// <summary> Tекущий отображаемый контейнер </summary>   
        IMusicContainer DisplayedMusicContainer { get; set; }
        /// <summary> Tекущая песня </summary>   
        Song CurrentSong { get; set; }

        bool IsRepeatSong { get; set; }
        bool IsRandomSong { get; set; }
        bool IsPlaying { get; set; }

        double Volume { get; set; }
        TimeSpan Position { get; set; }

        TimeSpan Duration { get; }

        /// <summary>
        /// Продолжить воспроизведение
        /// </summary>
        void Play();
        /// <summary>
        /// Пауза
        /// </summary>
        void Pause();
        /// <summary>
        /// Остановка
        /// </summary>
        void Stop();

        /// <summary>
        /// Предыдущая песня
        /// </summary>
        void PrevSong();
        /// <summary>
        /// Следующая песня
        /// </summary>
        void NextSong();
    }

    public interface IFileManager
    {
        /// <summary>
        /// Добавление песни из файлов устройства
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<Song> AddSongFromDevice(string path, string ContainerId);
        /// <summary>
        /// Удаление песни из файлов устройства
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<bool> RemoveSongFromDevice(Song song);
        /// <summary>
        /// Удаление песни из приложения
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<bool> RemoveSongFromApp(Song song);


        /// <summary>
        /// Добавление песни на сервер
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<bool> AddSongToServer(Song song);
        /// <summary>
        /// Удаление песни из сервера
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<bool> RemoveSongFromServer(Song song);

        /// <summary>
        /// Обновляет информацию о песне
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<bool> UpdateSongInfo(Song song);


        Task<LocalFolder> AddLocalFolderFromDevice(string path);
        Task<bool> RemoveLocalFolderFromDevice(LocalFolder localFolder);

        Task AddPlayList(string title, string description, string imageUri);
        Task<bool> RemovePlayList(PlayList playList);
        Task<bool> UpdatePlayList(PlayList playList);
    }

    public interface IMusicEvents
    {
       
    }
}
