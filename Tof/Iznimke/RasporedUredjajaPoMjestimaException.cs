using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class RasporedUredjajaPoMjestimaException : Exception
    {
        public RasporedUredjajaPoMjestimaException()
            : base("Format rasporeda uredjaja po mjestima nije ispravan!")
        {
        }

        public RasporedUredjajaPoMjestimaException(string message) : base(message)
        {
        }

        public RasporedUredjajaPoMjestimaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RasporedUredjajaPoMjestimaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}