using FierceStukCloud_NetCoreLib.Models.AbstractModels;
using FierceStukCloud_NetCoreLib.Models.MusicContainers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FierceStukCloud_NetCoreLib.Services.Extension
{
    public static class TypeConventer
    {
        public static LocalFolder ToLF(this BaseMusicObject bmo) => bmo as LocalFolder;
        public static LocalFolder ToLF(this MusicContainer musicContainer) => musicContainer as LocalFolder;

        public static MusicContainer ToMC(this BaseMusicObject bmo) => bmo as MusicContainer;

      
    }
}
