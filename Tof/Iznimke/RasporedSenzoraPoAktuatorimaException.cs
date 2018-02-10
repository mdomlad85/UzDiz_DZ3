using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class RasporedSenzoraPoAktuatorimaException : Exception
    {
        public RasporedSenzoraPoAktuatorimaException()
            : base("Format rasporeda senzora po aktuatorima nije ispravan!")
        {
        }

        public RasporedSenzoraPoAktuatorimaException(string message) : base(message)
        {
        }

        public RasporedSenzoraPoAktuatorimaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RasporedSenzoraPoAktuatorimaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}