using FierceStukCloud_NetStandardLib.Models.AbstractModels;
using FierceStukCloud_NetStandardLib.Models.MusicContainers;

namespace FierceStukCloud_NetStandardLib.Extension
{
    public static class TypeConventer
    {
        public static LocalFolder ToLF(this BaseMusicObject bmo) => bmo as LocalFolder;
        public static LocalFolder ToLF(this MusicContainer musicContainer) => musicContainer as LocalFolder;

        public static MusicContainer ToMC(this BaseMusicObject bmo) => bmo as MusicContainer;

      
    }
}
