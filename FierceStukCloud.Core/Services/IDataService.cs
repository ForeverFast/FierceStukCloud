using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using System.Threading.Tasks;

namespace FierceStukCloud.Core.Services
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
