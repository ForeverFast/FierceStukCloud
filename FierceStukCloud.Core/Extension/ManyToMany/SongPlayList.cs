using System;

namespace FierceStukCloud.Core.Extension.ManyToMany
{
    public class SongPlayList
    {
        public int Place { get; set; }

        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public Guid PlayListId { get; set; }
        public PlayList PlayList { get; set; }
    }
}
