using System;
using Tof.Model;

namespace Tof.Uzorci.Iterator
{
    /// <summary>
    /// Sučelje 'Iterator'
    /// </summary>
    public abstract class ApstraktniIterator<T>
    {
        public abstract T First();
        public abstract T Next();
        public abstract bool IsDone { get; }
        public abstract T CurrentItem { get; }
    }
}