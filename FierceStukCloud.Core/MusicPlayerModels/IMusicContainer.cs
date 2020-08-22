using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.Extension;
using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels
{
    public interface IMusicContainer : IBaseObject
    {
        ObservableLinkedList<Song> Songs { get; set; }
    }
}
