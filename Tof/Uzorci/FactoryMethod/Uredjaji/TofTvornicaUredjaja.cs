using System;
using Tof.Iznimke;
using Tof.Model;

namespace Tof.Uzorci.FactoryMethod
{
    /// <summary>
    /// The 'RefinedAbstraction' class
    /// </summary>
    class TofTvornicaUredjaja : TvornicaUredjaja
    {
        private static TofTvornicaUredjaja _instance;

        // Lock synchronization object
        private static object syncLock = new object();
        public static TofTvornicaUredjaja Instanca
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
                            _instance = new TofTvornicaUredjaja();
                        }
                    }
                }

                return _instance;
            }

        }

        private TofTvornicaUredjaja() { }

        public override IUredjajAkcija ProizvediDinamikuUredjaja(Vrsta vrsta)
        {
            switch (vrsta)
            {
                case Vrsta.CJELOBROJNO:
                    return new UredjajCjelobrojnoIzvrsavanje();
                case Vrsta.RAZLOMLJENO_1D:
                    return new UredjajRazlomljeno1Izvrsavanje();
                case Vrsta.RAZLOMLJENO_5D:
                    return new UredjajRazlomljeno5Izvrsavanje();
                case Vrsta.ISTINITOST:
                    return new UredjajBoolIzvrsavanje();
                default:
                    throw new NepoznataVrstaUredjaja();
            }
        }
    }
}
