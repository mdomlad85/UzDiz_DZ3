using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class NemaViseMjestaUKolekciji : Exception
    {
        public NemaViseMjestaUKolekciji() :base("Nema više mjesta u kolekciji.") { }

        public NemaViseMjestaUKolekciji(string message) : base(message)
        {
        }

        public NemaViseMjestaUKolekciji(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NemaViseMjestaUKolekciji(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}