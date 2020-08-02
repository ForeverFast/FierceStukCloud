using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class Album : IMusicContainer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public IList<Song> Songs { get; set; }
    }
}
