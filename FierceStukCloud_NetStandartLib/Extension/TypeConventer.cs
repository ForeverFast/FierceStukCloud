using FierceStukCloud_NetStandartLib.Models.AbstractModels;
using FierceStukCloud_NetStandartLib.Models.MusicContainers;

namespace FierceStukCloud_NetStandartLib.Extension
{
    public static class TypeConventer
    {
        public static LocalFolder ToLF(this BaseMusicObject bmo) => bmo as LocalFolder;
        public static LocalFolder ToLF(this MusicContainer musicContainer) => musicContainer as LocalFolder;

        public static MusicContainer ToMC(this BaseMusicObject bmo) => bmo as MusicContainer;

      
    }
}
