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

        public static bool TryConvert(this string value, out Commands command)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    if (value == ((Commands)i).ToString())
                    {
                        command = (Commands)i;
                        return true;
                    } 
                }
            }
            catch(Exception)
            {  }
            command = (Commands)0;
            return false;
        }
    }
}
