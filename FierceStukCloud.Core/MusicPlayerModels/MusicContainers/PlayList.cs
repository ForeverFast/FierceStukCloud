using System.Collections.Generic;

namespace FierceStukCloud.Core.MusicPlayerModels.MusicContainers
{
    public class PlayList : IMusicContainer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<Song> Songs { get; set; } = new List<Song>();
        public string UserLogin { get; set; }
    }
}
