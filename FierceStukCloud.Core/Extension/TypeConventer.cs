using FierceStukCloud.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FierceStukCloud.Core.Extension
{
    public static class TypeConventer
    {
        

        public static void AddRange<T>(this ObservableCollection<T> ts, ICollection<T> items)
        {
            foreach (var item in items)
                ts.Add(item);
        }
    }
}
