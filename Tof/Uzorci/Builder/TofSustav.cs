using System;
using System.Collections.Generic;
using System.Threading;
using Tof.Uzorci.Iterator;
using Tof.Model;
using Tof.Uzorci.Singleton;
using Tof.Logger;

namespace Tof.Uzorci.Builder
{
    /// <summary>
    /// The 'Product' class
    /// </summary>
    public class TofSustav
    {
        #region Properties
        private KolekcijaMjesta _mjesta = new KolekcijaMjesta();

        private Postavke _postavke = new Postavke();

        private object syncLock = new object();

        public KolekcijaMjesta Mjesta
        {
            get
            {
                lock (syncLock)
                {
                    return _mjesta;
                }
            }

            internal set
            {
                lock (syncLock)
                {
                    _mjesta = value;
                }
            }
        }

        public Postavke Postavke
        {
            get
            {
                lock (syncLock)
                {
                    return _postavke;
                }
            }

            internal set
            {
                lock (syncLock)
                {
                    _postavke = value;
                }
            }
        }
        #endregion

        public void Pokreni()
        {
            for (int i = 0; i < Postavke.BrojCiklusaDretve; i++)
            {
                var finishedCorrectly = DoHardWork();
                if (finishedCorrectly)
                {
                    AplikacijskiPomagac.Instanca.Statistika.UspjesnihCiklusa++;
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Uspješno završen {0}. ciklus ", i + 1), VrstaLogZapisa.DEBUG);
                }
                else
                {
                    AplikacijskiPomagac.Instanca.Statistika.NeuspjesnihCiklusa++;
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Isteklo je vrijeme u {0}. ciklusu ", i + 1), VrstaLogZapisa.DEBUG);
                }
            }
            AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa /= Postavke.BrojCiklusaDretve;
        }

        private bool DoHardWork()
        {
            Thread workerThread = new Thread(new ThreadStart(Run));
            workerThread.Start();
            bool finished = workerThread.Join(new TimeSpan(0, 0, Postavke.TrajanjeDretveSek));

            if (!finished)
            {
                workerThread.Abort();
                AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa += Postavke.TrajanjeDretveSek;
            }

            return finished;
        }

        public void Run()
        {
            AplikacijskiPomagac.Instanca.Logger.Log("Počinje obrada mjesta...", VrstaLogZapisa.INFO);
            var startTime = DateTime.Now;

            lock (syncLock)
            {
                lock (_mjesta)
                {
                    Postavke.AlgoritamProvjere.ProvjeriMjesta(_mjesta);
                }
                AktivirajUredjaje();
            }

            var totalSec = (DateTime.Now - startTime).TotalSeconds;
            var diff = Postavke.TrajanjeDretveSek - totalSec;
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("...završila obrada mjesta nakon {0} sekundi", totalSec), VrstaLogZapisa.INFO);
            AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa += diff;
        }

        private void AktivirajUredjaje()
        {
            var iteratorMjesta = _mjesta.CreateIterator();
            var o = new Visitor.UredjajiObjectStructure();
            while (!iteratorMjesta.IsDone)
            {
                AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Počinjem s obradom mjesta {0}", iteratorMjesta.CurrentItem.Naziv), VrstaLogZapisa.INFO);

                var iteratorSenzora = iteratorMjesta.CurrentItem.Senzori.CreateIterator();
                var aktuatoriKojiSeTrebajuOkinuti = new Dictionary<int, Uredjaj>();
                var ids = new List<int>();

                while (!iteratorSenzora.IsDone)
                {
                    iteratorSenzora.CurrentItem.OdradiPosao();
                    if (iteratorSenzora.CurrentItem.DosloDoPromjene)
                    {
                        ids.Add(iteratorSenzora.CurrentItem.ID);
                    }

                    iteratorSenzora.Next();
                }

                var iteratorAktuatora = iteratorMjesta.CurrentItem.Aktuatori.CreateIterator();
                while (!iteratorAktuatora.IsDone)
                {
                    if(iteratorAktuatora.CurrentItem.PovezaniUredjaji.Find(x => ids.Contains(x.ID)) != null)
                    {
                        o.Attach(iteratorAktuatora.CurrentItem.ID, iteratorAktuatora.CurrentItem);
                    }

                    iteratorAktuatora.Next();
                }

                AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Završavam s obradom mjesta {0}\n", iteratorMjesta.CurrentItem.Naziv), VrstaLogZapisa.INFO);
                iteratorMjesta.Next();
            }
            o.Accept(new Visitor.AkcijskiVisitor());
            o.Accept(new Visitor.RadniVisitor());
        }
    }
}
