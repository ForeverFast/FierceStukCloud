using FierceStukCloud_NetStandardLib.Models;
using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;
using System;
using System.Collections.Generic;

namespace FierceStukCloud_NetStandardLib.Extension
{
    public static class TypeConventer
    {
        /// <summary>
        /// Преобразование из BaseMusicObject в Song
        /// </summary>
        /// <param name="bmo"></param>
        /// <returns></returns>
        public static Song ToSong(this BaseMusicObject bmo) => bmo as Song;

        /// <summary>
        /// Преобразование из BaseMusicObject в LocalFolder
        /// </summary>
        /// <param name="bmo"></param>
        /// <returns></returns>
        public static LocalFolder ToLF(this BaseMusicObject bmo) => bmo as LocalFolder;
        /// <summary>
        /// Преобразование из MusicContainer в LocalFolder
        /// </summary>
        /// <param name="musicContainer"></param>
        /// <returns></returns>
        public static LocalFolder ToLF(this MusicContainer musicContainer) => musicContainer as LocalFolder;

        /// <summary>
        /// Преобразование из BaseMusicObject в MusicContainer
        /// </summary>
        /// <param name="bmo"></param>
        /// <returns></returns>
        public static MusicContainer ToMC(this BaseMusicObject bmo) => bmo as MusicContainer;

        /// <summary>
        /// Преобразование из списка BaseMusicObject в 2 списка: Songs и LocalFolders
        /// </summary>
        /// <param name="sortMas"></param>
        /// <param name="localFolders"></param>
        /// <param name="songs"></param>
        /// <returns></returns>
        public static bool TrySort(this List<BaseMusicObject> sortMas, out List<LocalFolder> localFolders, out List<Song> songs)
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
    }
}
