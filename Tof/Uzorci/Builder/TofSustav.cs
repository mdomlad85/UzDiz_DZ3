using System;
using System.Collections.Generic;
using System.Threading;
using Tof.Model;
using Tof.Uzorci.Singleton;
using Tof.Logger;
using Tof.Uzorci.Iterator;
using System.Linq;
using Tof.Uzorci.FactoryMethod;
using System.Threading.Tasks;
using Tof.Uzorci.MVC;

namespace Tof.Uzorci.Builder
{
    /// <summary>
    /// The 'Product' class
    /// </summary>
    [Serializable]
    public class TofSustav
    {
        #region Properties
        private List<Mjesto> _mjesta = new List<Mjesto>();
        private List<Uredjaj> _aktuatori = new List<Uredjaj>();
        private List<Uredjaj> _senzori = new List<Uredjaj>();

        private Postavke _postavke = new Postavke();

        private object syncLock = new object();

        public List<Mjesto> Mjesta
        {
            get
            {
                lock (syncLock)
                {
                    return _mjesta;
                }
            }

            set
            {
                lock (syncLock)
                {
                    _mjesta = value;
                }
            }
        }

        public List<Uredjaj> Aktuatori
        {
            get
            {
                lock (syncLock)
                {
                    return _aktuatori;
                }
            }

            set
            {
                lock (syncLock)
                {
                    _aktuatori = value;
                }
            }
        }

        public List<Uredjaj> Senzori
        {
            get
            {
                lock (syncLock)
                {
                    return _senzori;
                }
            }

            set
            {
                lock (syncLock)
                {
                    _senzori = value;
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

            set
            {
                lock (syncLock)
                {
                    _postavke = value;
                }
            }
        }

        public bool Zavrsio { get; set; }
        #endregion

        public void Pokreni(VT100Model model)
        {
            for (int i = 0; i < Postavke.Instanca.BrojCiklusaDretve; i++)
            {
                var finishedCorrectly = ObradiMjesta();
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
            AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa /= Postavke.Instanca.BrojCiklusaDretve;
            foreach (var line in AplikacijskiPomagac.Instanca.Logger.PovijestLogiranja.ToArray())
            {
               model.Logger.PovijestLogiranja.AppendLine(line);
            }
            AplikacijskiPomagac.Instanca.Logger.PovijestLogiranja.Clear();
            model.Notify();
        }

        private bool ObradiMjesta()
        {
            Thread workerThread = new Thread(new ThreadStart(Obradi));
            workerThread.Start();
            bool finished = workerThread.Join(new TimeSpan(0, 0, Postavke.TrajanjeDretveSek));


            if (!finished)
            {
                workerThread.Abort();
                AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa += Postavke.TrajanjeDretveSek;
            }

            return finished;
        }

        public void Obradi()
        {
            AplikacijskiPomagac.Instanca.Logger.Log("Počinje obrada mjesta...", VrstaLogZapisa.INFO);
            var startTime = DateTime.Now;

            foreach (var mjesto in _mjesta)
            {
                AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Počinjem s obradom mjesta {0}", mjesto.Naziv), VrstaLogZapisa.INFO);

                for (int i = 0; i < mjesto.Senzori.Length; i++)
                {
                    var senzor = mjesto.Senzori[i];
                    if (senzor == null) continue;

                    if (!senzor.JeIspravan)
                    {
                        AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Senzor s ID {0} je neispravan i već je zamijenjen", senzor.ExternalID), VrstaLogZapisa.INFO);
                        continue;
                    }

                    if (senzor.JeZdrav() == 1)
                    {
                        AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Senzor s ID {0} je ispravan", senzor.ExternalID), VrstaLogZapisa.INFO);
                        senzor.OdradiPosao();
                        AplikacijskiPomagac.Instanca.Statistika.BrojObradenihSenzora++;
                        if (senzor.DosloDoPromjene)
                        {
                            for (int j = 0; j < senzor.PovezaniUredjaji.Count; j++)
                            {
                                var aktuator = senzor.PovezaniUredjaji[j];
                                if (!aktuator.JeIspravan)
                                {
                                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator s ID {0} nije ispravan i već je zamijenjen", aktuator.ExternalID), VrstaLogZapisa.ERROR);
                                    continue;
                                }
                                if (aktuator.JeZdrav() == 1)
                                {
                                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator s ID {0} je ispravan", aktuator.ExternalID), VrstaLogZapisa.INFO);
                                    aktuator.OdradiPosao();
                                    TofTvornicaUredjaja
                                        .Instanca
                                        .ProizvediDinamikuUredjaja(aktuator.Vrsta)
                                        .Izvrsi(aktuator);
                                    AplikacijskiPomagac.Instanca.Statistika.BrojObradenihAktuatora++;
                                }
                                else
                                {
                                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator s ID {0} nije ispravan", aktuator.ExternalID), VrstaLogZapisa.ERROR);
                                    var noviUredjaj = (Uredjaj)aktuator.Clone();
                                    noviUredjaj.Inicijaliziraj("Zamijena");
                                    _aktuatori.Add(noviUredjaj);
                                    mjesto.Aktuatori[j] = noviUredjaj;
                                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator s ID {0} je zamijenjen s aktuatorom ID-a: {1}", aktuator.ExternalID, noviUredjaj.ExternalID), VrstaLogZapisa.WARNING);
                                    AplikacijskiPomagac.Instanca.Statistika.BrojZamijenjenihAktuatora++;
                                }
                            }
                        }
                    }
                    else
                    {
                        AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Senzor s ID {0} nije ispravan", senzor.ExternalID), VrstaLogZapisa.ERROR);
                        var noviUredjaj = (Uredjaj)senzor.Clone();
                        noviUredjaj.Inicijaliziraj("Zamijena");
                        _senzori.Add(noviUredjaj);
                        mjesto.Senzori[i] = noviUredjaj;
                        AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Senzor s ID {0} je zamijenjen s senzorom ID-a: {1}", senzor.ExternalID, noviUredjaj.ExternalID), VrstaLogZapisa.WARNING);
                        AplikacijskiPomagac.Instanca.Statistika.BrojZamijenjenihSenzora++;
                    }
                }

                AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Završavam s obradom mjesta {0}\n", mjesto.Naziv), VrstaLogZapisa.INFO);
            }

            var totalSec = (DateTime.Now - startTime).TotalSeconds;
            var diff = Postavke.TrajanjeDretveSek - totalSec;
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("...završila obrada svih mjesta nakon {0} sekundi", totalSec), VrstaLogZapisa.INFO);
            AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa += diff;
        }
        public TofSustav Clone()
        {
            return (TofSustav)MemberwiseClone();
        }
    }
}
