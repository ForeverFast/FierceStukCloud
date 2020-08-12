using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class LocalFolder : IMusicContainer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string LocalUrl { get; set; }
        public LinkedList<Song> Songs { get; set; }
    }
}
