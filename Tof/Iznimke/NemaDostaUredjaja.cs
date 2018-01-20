using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class NemaDostaUredjaja : Exception
    {
        public NemaDostaUredjaja() : base("Nema dosta uređaja!") { }

        public NemaDostaUredjaja(string message) : base(message)
        {
        }

        public NemaDostaUredjaja(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NemaDostaUredjaja(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}