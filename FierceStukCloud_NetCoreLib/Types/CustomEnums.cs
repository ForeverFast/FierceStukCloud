using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FierceStukCloud_NetCoreLib.Types
{
    public static class CustomEnums
    {
        public enum Commands
        {
            GetSongs,
            SendSongs,
            SetCurrentSong
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
