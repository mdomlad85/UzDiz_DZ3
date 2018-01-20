using System;
using System.Runtime.Serialization;

namespace Tof.Iznimke
{
    [Serializable]
    internal class NeispravniUlazniArgumenti : Exception
    {
        private static string msg = "Kod izvršavanja programa upisuje se sjeme za generator slučajnog broja (min 3 znamenke),  naziv datoteke mjesta, naziv datoteke senzora, naziv datoteke aktuatora, naziv klase algoritma provjere, trajanje ciklusa dretve u sek, broj ciklusa dretve, naziv datoteke u koju se sprema izlaz programa. Npr: > dkermek_zadaca_1 717 DZ_1_mjesta.txt DZ_1_senzori.txt DZ_1_aktuatori.txt  AlgoritamAbecedno 5 20 izlaz.txt";
        public NeispravniUlazniArgumenti() : base(msg) { }

        public NeispravniUlazniArgumenti(string message) : base(message)
        {
        }

        public NeispravniUlazniArgumenti(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NeispravniUlazniArgumenti(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}