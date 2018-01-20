using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class NeispravnaVelicinaMeduspremnika : Exception
    {
        public NeispravnaVelicinaMeduspremnika()
            : base("Velicina meduspremnika mora biti cijeli broj veći od nule.")
        {
        }

        public NeispravnaVelicinaMeduspremnika(string message) : base(message)
        {
        }

        public NeispravnaVelicinaMeduspremnika(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NeispravnaVelicinaMeduspremnika(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}