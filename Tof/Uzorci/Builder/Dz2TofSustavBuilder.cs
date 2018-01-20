using System;
using System.Collections.Generic;
using System.Linq;
using Tof.Uzorci.Composite;
using Tof.Uzorci.Iterator;
using Tof.Iznimke;
using Tof.Model;
using Tof.Uzorci.Singleton;
using Tof.Logger;

namespace Tof.Uzorci.Builder
{
    /// <summary>
    /// The 'ConcreteBuilder' class
    /// </summary>
    public class Dz2TofSustavBuilder : ITofSustavBuilder
    {
        TofSustav _tofSustav = new TofSustav();

        private bool _sustavSpreman = false;

        private List<UredjajComponent> _aktuatoriComponents = new List<UredjajComponent>();

        private Dictionary<string, UredjajComponent> _senzoriComponents = new Dictionary<string, UredjajComponent>();

        public TofSustav TofSUstav
        {
            get
            {
                if (_sustavSpreman)
                {
                    return _tofSustav;
                }
                else
                {
                    throw new TofSustavNijeSpreman();
                }
            }
        }

        public void UcitajPostavke(object ulazniPodaci)
        {
            _tofSustav.Postavke = (Postavke)ulazniPodaci;
        }

        public void UcitajPodatke()
        {
            UcitajMjesta();
        }

        #region Ucitavanje podataka helperi

        private void UcitajMjesta()
        {
            _tofSustav.Mjesta = new KolekcijaMjesta();
            string[] linije = _tofSustav.Postavke.DatotekaMjesta.ReadAllLinesExceptFirstN();

            for (int i = 0; i < linije.Length; i++)
            {
                try
                {
                    var mjesto = new Mjesto(linije[i].Split(';'));
                    UcitajSenzoreZaMjesto(mjesto);
                    UcitajAktuatoreZaMjesto(mjesto);

                    _tofSustav.Mjesta[i] = mjesto;
                }
                catch (Exception ex)
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(ex.Message, VrstaLogZapisa.ERROR);
                }
            }
        }

        #region Ucitaj mjesta helperi
        private void UcitajSenzoreZaMjesto(Mjesto mjesto)
        {
            var senzori = MaticniPodaci.Senzori.DohvatiPoTipu(mjesto.Tip);

            if (mjesto.Senzori.Count > senzori.Count())
            {
                throw new NemaDostaUredjaja("Nema dosta Senzora");
            }
            else if (mjesto.Senzori.Count == senzori.Count())
            {
                mjesto.Senzori.PrekopirajPoljeSNovim(senzori);
            }
            else
            {
                var keys = new HashSet<int>();
                var max = senzori.Count();
                for (int i = 0; i < mjesto.Senzori.MaxCount; i++)
                {
                    var haveNewKey = false;
                    var key = -1;
                    while (!haveNewKey)
                    {
                        key = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0, max);
                        haveNewKey = keys.Add(key);
                    }
                    mjesto.Senzori[i] = senzori[key];
                }
            }
        }

        private void UcitajAktuatoreZaMjesto(Mjesto mjesto)
        {
            var aktuatori = MaticniPodaci.Aktuatori.DohvatiPoTipu(mjesto.Tip);

            if (mjesto.Aktuatori.Count > aktuatori.Count)
            {
                throw new NemaDostaUredjaja("Nema dosta Aktuatora");
            }
            else if (aktuatori.Count == mjesto.Aktuatori.Count)
            {
                MaticniPodaci.Aktuatori.PrekopirajPoljeSNovim(aktuatori);
            }
            else
            {
                var keys = new HashSet<int>();
                var max = aktuatori.Count();
                for (int i = 0; i < mjesto.Aktuatori.MaxCount; i++)
                {
                    var haveNewKey = false;
                    var key = -1;
                    while (!haveNewKey)
                    {
                        key = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0, max);
                        haveNewKey = keys.Add(key);
                    }
                    mjesto.Aktuatori[i] = aktuatori[key];
                }
            }
        }
        #endregion

        #endregion

        public void InicijalizirajSustav()
        {
            var mjestaIterator = _tofSustav.Mjesta.CreateIterator();
            while (!mjestaIterator.IsDone)
            {
                InicijalizirajUredjaje(mjestaIterator.CurrentItem.Senzori);
                InicijalizirajUredjaje(mjestaIterator.CurrentItem.Aktuatori);
                mjestaIterator.Next();
            }

            _sustavSpreman = true;
        }

        #region Inicijaliziraj sustav helperi
        private void InicijalizirajUredjaje(KolekcijaUredjaja uredjaji)
        {
            var iteratorUredjaja = uredjaji.CreateIterator();
            while (!iteratorUredjaja.IsDone)
            {
                iteratorUredjaja.CurrentItem.Inicijaliziraj(string.Format("Inicijalizacija uređaja {0}", iteratorUredjaja.CurrentItem.Naziv));
                if (iteratorUredjaja.CurrentItem.JeIspravan)
                {
                    AplikacijskiPomagac.Instanca
                        .Logger.Log(string.Format("Uređaj {0} je ispravan", iteratorUredjaja.CurrentItem.Naziv));
                }
                else
                {
                    AplikacijskiPomagac.Instanca
                        .Logger.Log(string.Format("Uređaj {0} je neispravan", iteratorUredjaja.CurrentItem.Naziv));
                }
                iteratorUredjaja.Next();
            }
        }

        public void Opremi()
        {
            var iteratorMjesta = _tofSustav.Mjesta.CreateIterator();

            while (!iteratorMjesta.IsDone)
            {
                var iteratorAktuatora = iteratorMjesta.CurrentItem.Aktuatori.CreateIterator();
                while (!iteratorAktuatora.IsDone)
                {
                    if (iteratorAktuatora.CurrentItem.JeIspravan)
                    {
                        var aktuatorRoot = new UredjajComposite(iteratorAktuatora.CurrentItem);
                        var ispravniSenzori = iteratorMjesta.CurrentItem.Senzori.DohvatiIspravne();
                        var brojSenzora = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(1, ispravniSenzori.Count);

                        HashSet<int> dodani = new HashSet<int> { -1 };
                        for (int i = 0; i < brojSenzora; i++)
                        {
                            var index = -1;
                            while (!dodani.Add(index))
                            {
                                index = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0, ispravniSenzori.Count);
                            }
                            var senzorZaDodati = ispravniSenzori[index];
                            iteratorAktuatora.CurrentItem.PovezaniUredjaji.Add(senzorZaDodati);

                            try
                            {
                                var senzorRoot = _senzoriComponents[GetKey(senzorZaDodati)];
                                senzorRoot.Add(new UredjajLeaf(iteratorAktuatora.CurrentItem));
                            } catch (Exception ex)
                            {
                                var senzorRoot = new UredjajComposite(senzorZaDodati);
                                if (ex is ArgumentNullException)
                                {
                                    AplikacijskiPomagac.Instanca.Logger.Log(ex.Message, VrstaLogZapisa.ERROR);
                                }
                                else
                                {
                                    senzorRoot.Add(new UredjajLeaf(iteratorAktuatora.CurrentItem));
                                    _senzoriComponents.Add(GetKey(senzorZaDodati), senzorRoot);
                                }
                            }


                            aktuatorRoot.Add(new UredjajLeaf(senzorZaDodati));
                        }

                        _aktuatoriComponents.Add(aktuatorRoot);
                    }

                    iteratorAktuatora.Next();
                }

                iteratorMjesta.Next();
            }

            var root = new UredjajComposite(new Uredjaj
            {
                Naziv = "Korijen"
            });
            root.AddRange(_aktuatoriComponents);
            root.AddRange(_senzoriComponents.Values);
            root.Display("--");
        }

        private string GetKey(Uredjaj senzorZaDodati)
        {
            return string.Format("{0}-{1}", senzorZaDodati.Naziv, senzorZaDodati.ID);
        }
        #endregion
    }
}
