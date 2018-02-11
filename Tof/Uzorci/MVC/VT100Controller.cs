using System;
using System.Collections.Generic;
using Tof.Uzorci.Builder;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.MVC
{
    public class VT100Controller : IController
    {
        public VT100Controller(IModel model) : base(model)
        {
            
        }
        public override void Inicijaliziraj(Postavke postavke)
        {
            AplikacijskiPomagac.Instanca.PostaviNasumicnjak(new Nasumicnjak.SistemskiNasumicnjak(postavke.Sjeme));
            MaticniPodaci.Ucitaj(postavke);
            var dzTofDirektor = new TofSustavDirector(new Dz3TofSustavBuilder(), postavke);
            dzTofDirektor.KreirajTofSustav();
            _tofSustav = dzTofDirektor.TofSustav;
            foreach (var line in AplikacijskiPomagac.Instanca.Logger.PovijestLogiranja.ToArray())
            {
                _model._writer.PovijestLogiranja.AppendLine(line);
            }
            AplikacijskiPomagac.Instanca.Logger.PovijestLogiranja.Clear();
            _model.Notify();
        }

        public override void ObradiZahtjev(string zahtjev)
        {
            var naredba = new Naredba(zahtjev);

            switch (naredba.Komanda)
            {
                case Akcija.ISPIS_MJESTA:
                    try
                    {
                        _model.IspisPodatakaMjesta(_tofSustav.Mjesta.Find(x => x.ID == naredba.Vrijednost));
                    }
                    catch
                    {
                        _model.IspisiPogresku(string.Format("Nema mjesta s ID:{0}", naredba.Vrijednost));
                    }
                    break;
                case Akcija.ISPIS_SENZORA:
                    try
                    {
                        _model.IspisPodatakaSenzora(_tofSustav.Senzori.Find(x => x.ExternalID == naredba.Vrijednost));
                    } catch
                    {
                        _model.IspisiPogresku(string.Format("Nema senzora s ID:{0}",naredba.Vrijednost));
                    }  
                    break;
                case Akcija.ISPIS_AKTUATORA:
                    try
                    {
                        _model.IspisPodatakaAktuatora(_tofSustav.Aktuatori.Find(x => x.ExternalID == naredba.Vrijednost));
                    }
                    catch
                    {
                        _model.IspisiPogresku(string.Format("Nema aktuatora s ID:{0}", naredba.Vrijednost));
                    }                    
                    break;
                case Akcija.ISPIS_STATISTIKE:
                    _model.IspisStatistike();
                    break;
                case Akcija.SPREMII_PODATKE:
                    _model.SpremiPodatke(_tofSustav);
                    break;
                case Akcija.VRATI_PODATKE:
                    var podaci = _model.VratiSpremljenePodatke();
                    if(podaci != null)
                    {
                        _tofSustav = podaci;
                    }
                    _model.Notify();
                    break;
                case Akcija.CIKLUS_DRETVI:
                    _model.IzvrsiNCiklusaDretve(naredba.Vrijednost, _tofSustav);
                    break;
                case Akcija.VLASTITA_FUNKCIONALNOST:
                    var aktuator = _tofSustav.Aktuatori.Find(a => a.ExternalID == naredba.Vrijednost);
                    if (aktuator != null)
                    {
                        _model.VlastitaFunkcionalnost(aktuator);
                    } else
                    {
                        _model.IspisiPogresku(string.Format("Ne postoji aktuator s ID: {0}", naredba.Vrijednost));
                    }
                    break;
                case Akcija.PROSJECNA_ISPRAVNOST:
                    _model.PostaviPctIspravnostUredjaja(naredba.Vrijednost);
                    break;
                case Akcija.POMOC:
                    _model.IspisiPomoc();
                    break;
                case Akcija.IZLAZ:
                    _model.Izadji();
                    break;
                case Akcija.NEPOSTOJECA:
                default:
                    _model.IspisiPogresku();
                    break;
            }
        }

        enum Akcija : int
        {
            ISPIS_MJESTA,
            ISPIS_SENZORA,
            ISPIS_AKTUATORA,
            ISPIS_STATISTIKE,
            SPREMII_PODATKE,
            VRATI_PODATKE,
            CIKLUS_DRETVI,
            VLASTITA_FUNKCIONALNOST,
            PROSJECNA_ISPRAVNOST,
            POMOC,
            IZLAZ,
            NEPOSTOJECA
        }

        private class Naredba
        {
            public Akcija Komanda { get; set; }

            public int Vrijednost { get; set; }

            public Naredba(string input)
            {
                try
                {
                    var naredba = input.Split(' ');
                    if (naredba[0].ToUpper() == "S" && naredba.Length == 1)
                    {
                        Komanda = _akcije["ST"];
                    }
                    else
                    {
                        Komanda = _akcije[naredba[0].ToUpper()];
                    }

                    if (naredba.Length == 2)
                    {
                        Vrijednost = int.Parse(naredba[1]);
                    }
                }
                catch (Exception)
                {
                    Komanda = Akcija.NEPOSTOJECA;
                }

            }
        }

        private static Dictionary<string, Akcija> _akcije = new Dictionary<string, Akcija>
        {
            { "M", Akcija.ISPIS_MJESTA },
            { "S", Akcija.ISPIS_SENZORA },
            { "A", Akcija.ISPIS_AKTUATORA },
            { "ST", Akcija.ISPIS_STATISTIKE },
            { "SP", Akcija.SPREMII_PODATKE },
            { "VP", Akcija.VRATI_PODATKE },
            { "C", Akcija.CIKLUS_DRETVI },
            { "VF", Akcija.VLASTITA_FUNKCIONALNOST },
            { "PI", Akcija.PROSJECNA_ISPRAVNOST },
            { "H", Akcija.POMOC },
            { "I", Akcija.IZLAZ }
        };
    }
}
