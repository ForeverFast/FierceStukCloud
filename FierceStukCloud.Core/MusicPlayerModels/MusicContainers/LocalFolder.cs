using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class LocalFolder : IMusicContainer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LocalUrl { get; set; }
        public IList<Song> Songs { get; set; }     
    }
}
