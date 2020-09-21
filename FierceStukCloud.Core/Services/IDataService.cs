using FierceStukCloud.Core;
using System.Threading.Tasks;

namespace FierceStukCloud.Wpf.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Получение информации о песнях из БД
        /// </summary>
        /// <returns></returns>
        void GetData();

        /// <summary>
        /// Добавлние песни с устройства
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Song AddSong(string path, string optionalInfo = "");
        /// <summary>
        /// Удаление песни из БД
        /// </summary>
        /// <returns></returns>
        bool RemoveSong(Song song);

        bool UpdateSong(Song song);

        LocalFolder AddLocalFolder(string path);

        bool RemoveLocalFolder(LocalFolder localFolder);

        PlayList AddPlayList(string title, string description, string imageUri);

        bool RemovePlayList(PlayList playList);
    }
}
