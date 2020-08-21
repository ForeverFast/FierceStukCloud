using System;

namespace TestNetStandardLib
{
    public interface test1
    {
#if NETCOREAPP3_1

        string Songs { get; set; }
#endif

#if NETSTANDARD2_0

        int Songs { get; set; }
#endif
    }
}
