using FierceStukCloud.Abstractions;
using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels
{
    public interface IMusicContainer : IBaseObject
    {
        LinkedList<Song> Songs { get; set; }
    }
}
