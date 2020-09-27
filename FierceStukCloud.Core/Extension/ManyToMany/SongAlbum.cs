﻿using System;

namespace FierceStukCloud.Core.Extension.ManyToMany
{
    public class SongAlbum
    {
        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
