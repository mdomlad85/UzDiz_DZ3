using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class LosRedakIzDatoteke : Exception
    {
        public LosRedakIzDatoteke() : base("Redak iz datoteke nije dobar. Pogledaj u builder direktoru.") { }

        public LosRedakIzDatoteke(string message) : base(message)
        {
        }

        public LosRedakIzDatoteke(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LosRedakIzDatoteke(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}