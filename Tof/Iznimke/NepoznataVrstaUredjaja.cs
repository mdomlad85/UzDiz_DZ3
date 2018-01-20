using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class NepoznataVrstaUredjaja : Exception
    {
        public NepoznataVrstaUredjaja() :base("Nepoznata vrsta uređaja") { }

        public NepoznataVrstaUredjaja(string message) : base(message)
        {
        }

        public NepoznataVrstaUredjaja(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NepoznataVrstaUredjaja(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}