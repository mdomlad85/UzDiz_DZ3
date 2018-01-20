using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class DuplaZastavica : Exception
    {
        private const string PORUKA = "Dupla zastavica";

        private const string FORMAT_PORUKE = "Dupla zastavica {0}.";
        public DuplaZastavica() :base(PORUKA) { }

        public DuplaZastavica(string zastavica) : base(string.Format(FORMAT_PORUKE, zastavica))
        {
        }

        public DuplaZastavica(string zastavica, Exception innerException) : base(FORMAT_PORUKE, innerException)
        {
        }

        protected DuplaZastavica(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}