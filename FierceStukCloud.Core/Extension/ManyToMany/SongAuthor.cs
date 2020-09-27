﻿using System;

namespace FierceStukCloud.Core.Extension.ManyToMany
{
    public class SongAuthor
    {
        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
