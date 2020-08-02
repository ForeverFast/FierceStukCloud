using FierceStukCloud.Abstractions;
using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels
{
    public interface IMusicContainer : IBaseObject
    {
        IList<Song> Songs { get; set; }
    }
}
