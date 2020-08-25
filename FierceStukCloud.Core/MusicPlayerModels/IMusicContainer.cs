using FierceStukCloud.Abstractions;
using FierceStukCloud.Core.Extension;

namespace FierceStukCloud.Core
{
    public interface IMusicContainer : IBaseObject
    {
        ObservableLinkedList<Song> Songs { get; set; }
    }
}
