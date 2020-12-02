using System;

namespace FierceStukCloud.Core.Extension.ManyToMany
{
    public class AlbumAuthor
    {
        public int Place { get; set; }

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
