using System;

namespace FierceStukCloud.Core
{
    public static class CustomEnums
    {
        public enum Commands
        {
            GetSongs,
            SendSongs,
            SetCurrentSong,

            PrevSong,
            NextSong,
            PlaySong,
            PauseSong,
            StopSong
        }

        public enum DeviceType
        {
            PC,
            Mobile,
            TV,
            Web
        }

        public enum PhoneMode
        {
            RemoteСontroller,
            MusicPlayer
        }

        public enum LoopMode
        {
           None,
           Loop,
           LoopOne
        }
        
    }
}
