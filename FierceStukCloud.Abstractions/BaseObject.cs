using System;

namespace FierceStukCloud.Abstractions
{
    public abstract class BaseObject : OnPropertyChangedClass
    {
        public Guid Id { get; set; }
    }
}
