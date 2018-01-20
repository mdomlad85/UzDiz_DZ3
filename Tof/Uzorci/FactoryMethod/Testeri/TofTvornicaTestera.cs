using System.Collections.Generic;
using Tof.Iznimke;

namespace Tof.Uzorci.FactoryMethod
{
    /// <summary>
    /// A 'ConcreteCreator' class
    /// </summary>
    class TofTvornicaTestera : TvornicaTestera
    {

        private static Dictionary<string, TipTestera> tipovi = new Dictionary<string, TipTestera>
        {
            { "NasumicniTester", TipTestera.NASUMICNI },
            { "SekvencijalniTester", TipTestera.SEKVENCIJALNI },
            { "HibridniTester", TipTestera.HIBRIDNI }
        };

        /// <summary>
        /// Singleton pattern
        /// </summary>

        private static TofTvornicaTestera _instance;

        // Lock synchronization object
        private static object syncLock = new object();
        public static TofTvornicaTestera Instanca
        {
            get
            {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TofTvornicaTestera();
                        }
                    }
                }

                return _instance;
            }

        }

        private TofTvornicaTestera() { }

        /// <summary>
        /// Implementaccija factory metode
        /// </summary>
        /// <param name="tip"></param>
        /// <returns></returns>
        public override ITesterUredjaja ProizvediTestera(string nazivTestera)
        {
            return ProizvediTestera(tipovi[nazivTestera]);            
        }

        /// <summary>
        /// Implementaccija factory metode
        /// </summary>
        /// <param name="tip"></param>
        /// <returns></returns>
        public override ITesterUredjaja ProizvediTestera(TipTestera tip)
        {
            switch (tip)
            {
                case TipTestera.SEKVENCIJALNI:
                    return new SekvencijalniTester();
                case TipTestera.NASUMICNI:
                    return new NasumicniTester();
                case TipTestera.HIBRIDNI:
                    return new HibridniTester();
                default:
                    throw new NepoznatTipTestera();
            }
        }
    }
}