using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using NAudio;

namespace FierceStukCloud_NetCoreLib.FSC_MusicCore
{
    public class FSC_Core
    {
        public FSC_SinglaR SinglaR { get; }

        

        #region Конструкторы

        public FSC_Core()
        {
            SinglaR = new FSC_SinglaR();
        }

        #endregion
    }
}
