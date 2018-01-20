using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class PrelijevanjeMeduspremika : Exception
    {
        public PrelijevanjeMeduspremika() : base("Došlo je do prelijevanja međuspremnika.")
        {
        }

        public PrelijevanjeMeduspremika(string message) : base(message)
        {
        }

        public PrelijevanjeMeduspremika(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PrelijevanjeMeduspremika(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}