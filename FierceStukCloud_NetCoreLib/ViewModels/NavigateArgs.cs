using System;
using System.Collections.Generic;
using System.Text;

namespace FierceStukCloud_NetCoreLib.ViewModels
{
    public enum NavigateType
    {
        NavigateTo,
        Back,
        Forward
    }

    public class NavigateArgs
    {
        public NavigateArgs()
        {

        }

        public NavigateArgs(NavigateType NT)
        {
            NavigateType = NT;
        }

        public NavigateArgs(string url, object vm, NavigateType NT) : this (NT)
        {
            Url = url;
            VM = vm;
        }

        public NavigateType NavigateType { get; set; }
        public object VM { get; set; }
        public string Url { get; set; }
    }   
}
