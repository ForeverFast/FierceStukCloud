using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class Album : IMusicContainer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public LinkedList<Song> Songs { get; set; }
    }
}
