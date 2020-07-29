using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FierceStukCloud_NetStandardLib.Models.AbstractModels
{
    public abstract class MusicContainer : BaseMusicObject
    {
        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
