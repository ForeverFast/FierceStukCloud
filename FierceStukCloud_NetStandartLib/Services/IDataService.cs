using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FierceStukCloud_NetStandardLib.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Добавлние песни с устройства
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Song> AddSong(string path, string optionalInfo = "LF");
        /// <summary>
        /// Удаление песни из БД
        /// </summary>
        /// <returns></returns>
        Task<bool> RemoveSong(Song song);
        /// <summary>
        /// Получение информации о песнях из БД
        /// </summary>
        /// <returns></returns>
        Task GetData();

        Task<LocalFolder> AddLocalFolder(string path);
        Task<bool> RemoveLocalFolder(LocalFolder localFolder);
    }
}
