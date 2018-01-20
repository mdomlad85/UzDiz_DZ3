using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class NepoznatTipTestera : Exception
    {
        public NepoznatTipTestera() :base("Nepoznat tip testera") { }

        public NepoznatTipTestera(string message) : base(message)
        {
        }

        public NepoznatTipTestera(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NepoznatTipTestera(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}