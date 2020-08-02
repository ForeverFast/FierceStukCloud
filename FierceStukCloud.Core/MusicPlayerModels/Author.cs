using FierceStukCloud.Core.MusicPlayerModels.MusicContainers;
using FierceStukCloud.Abstractions;
using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels
{
    public class Author : IBaseObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
