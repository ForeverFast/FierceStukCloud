using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.MusicPlayerModels;
using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FierceStukCloud.Core.Extension
{
    public static class TypeConventer
    {
        /// <summary>
        /// Преобразование из BaseMusicObject в Song
        /// </summary>
        /// <param name="bmo"></param>
        /// <returns></returns>
        public static Song ToSong(this IBaseObject bmo) => bmo as Song;

        /// <summary>
        /// Преобразование из BaseMusicObject в LocalFolder
        /// </summary>
        /// <param name="bmo"></param>
        /// <returns></returns>
        public static LocalFolder ToLF(this IBaseObject bmo) => bmo as LocalFolder;
        /// <summary>
        /// Преобразование из MusicContainer в LocalFolder
        /// </summary>
        /// <param name="musicContainer"></param>
        /// <returns></returns>
        public static LocalFolder ToLF(this IMusicContainer musicContainer) => musicContainer as LocalFolder;

        /// <summary>
        /// Преобразование из BaseMusicObject в MusicContainer
        /// </summary>
        /// <param name="bmo"></param>
        /// <returns></returns>
        public static IMusicContainer ToMC(this IBaseObject bmo) => bmo as IMusicContainer;

        /// <summary>
        /// Преобразование из списка BaseMusicObject в 2 списка: Songs и LocalFolders
        /// </summary>
        /// <param name="sortMas"></param>
        /// <param name="localFolders"></param>
        /// <param name="songs"></param>
        /// <returns></returns>
        public static bool TrySort(this List<IBaseObject> sortMas, out List<LocalFolder> localFolders, out List<Song> songs)
        {
            try
            {
                songs = new List<Song>();
                localFolders = new List<LocalFolder>();
                foreach (var item in sortMas)
                {
                    if (item is LocalFolder)
                    {
                        localFolders.Add(item.ToLF());
                    }
                    else
                    {
                        songs.Add(item.ToSong());
                    }
                }
                return true;
            }
            catch(Exception)
            {
                songs = null;
                localFolders = null;
                return false;
            }
        }

        public static void AddRange<T>(this ObservableCollection<T> ts, ICollection<T> items)
        {
            foreach (var item in items)
                ts.Add(item);
        }

        //public static int CurrentMusicContainerIdValue(this Song song) => song.LocalId.Find(x => x.Key == song.CurrentMusicContainer).Value;

    }
}
