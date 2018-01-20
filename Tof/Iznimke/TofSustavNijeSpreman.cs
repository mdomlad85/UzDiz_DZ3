using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class TofSustavNijeSpreman : Exception
    {
        public TofSustavNijeSpreman()
            : base("Tof sustav nije izgrađen")
        {
        }

        public TofSustavNijeSpreman(string message) : base(message)
        {
        }

        public TofSustavNijeSpreman(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TofSustavNijeSpreman(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}