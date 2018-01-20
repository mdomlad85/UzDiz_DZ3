using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class SjemeMoraBitiBroj : Exception
    {
        public SjemeMoraBitiBroj() :base("Sjeme mora biti broj.") { }

        public SjemeMoraBitiBroj(string message) : base(message)
        {
        }

        public SjemeMoraBitiBroj(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SjemeMoraBitiBroj(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}