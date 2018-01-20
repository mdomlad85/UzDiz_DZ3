using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class NemaDovoljnoPodataka : Exception
    {
        public NemaDovoljnoPodataka() : base("Nema dovoljno podataka u međuspremniku")
        {
        }

        public NemaDovoljnoPodataka(string message) : base(message)
        {
        }

        public NemaDovoljnoPodataka(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NemaDovoljnoPodataka(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}