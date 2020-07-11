using System.Collections.Generic;

namespace FierceStukCloud_NetStandartLib.Models.AbstractModels
{
    public abstract class MusicContainer : BaseMusicObject
    {
        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
