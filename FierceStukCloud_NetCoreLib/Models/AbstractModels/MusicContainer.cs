using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FierceStukCloud_NetCoreLib.Models.AbstractModels
{
    public abstract class MusicContainer
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public List<Song> Songs { get; set; } = new List<Song>();

        public bool OnServer { get; set; }

        public bool OnPC { get; set; }
    }
}
