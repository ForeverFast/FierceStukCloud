using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class PlayList : IMusicContainer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public LinkedList<Song> Songs { get; set; }
        public string UserLogin { get; set; }
    }
}
