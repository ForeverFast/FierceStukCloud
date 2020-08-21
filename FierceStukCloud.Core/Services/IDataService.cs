using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using System.Threading.Tasks;

namespace FierceStukCloud.Wpf.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Получение информации о песнях из БД
        /// </summary>
        /// <returns></returns>
        Task GetData();

        /// <summary>
        /// Добавлние песни с устройства
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Song> AddSongAsync(string path, string optionalInfo = "");
        /// <summary>
        /// Удаление песни из БД
        /// </summary>
        /// <returns></returns>
        Task<bool> RemoveSongAsync(Song song);


        Task<LocalFolder> AddLocalFolderAsync(string path);

        Task<bool> RemoveLocalFolderAsync(LocalFolder localFolder);

        Task<PlayList> AddPlayListAsync(string title, string description, string imageUri);

        Task<bool> RemovePlayListAsync(PlayList playList);
    }
}
