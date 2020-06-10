using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FierceStukCloud_NetCoreLib.Models.AbstractModels
{
    public abstract class MusicContainer : BaseMusicObject
    {
        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
