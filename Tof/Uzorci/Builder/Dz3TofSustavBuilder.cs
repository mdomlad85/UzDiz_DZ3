using System;
using System.Collections.Generic;
using System.Linq;
using Tof.Iznimke;
using Tof.Model;
using Tof.Uzorci.Singleton;
using Tof.Logger;

namespace Tof.Uzorci.Builder
{
    /// <summary>
    /// The 'ConcreteBuilder' class
    /// </summary>
    public class Dz3TofSustavBuilder : ITofSustavBuilder
    {
        TofSustav _tofSustav = new TofSustav();

        private bool _sustavSpreman = false;

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
            UcitajRaspored();
        }

        #region Ucitaj podatke pomocne metode

        private void UcitajRaspored()
        {
            string[] linije = _tofSustav.Postavke.DatotekaRasporeda.ReadAllLinesExceptFirstN(3);
            for (int i = 0; i < linije.Length; i++)
            {
                var vrstaRasporeda = (VrstaRasporeda)int
                    .Parse(linije[i][0].ToString());

                switch (vrstaRasporeda)
                {
                    case VrstaRasporeda.RasporedUredjajaPoMjestima:
                        try
                        {
                            UcitajRasporedZaMjesto(new RasporedUredjajaPoMjestima(linije[i]));
                        }
                        catch
                        {
                            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Linija je neispravna: {0}", linije[i]), VrstaLogZapisa.ERROR);
                        }
                        break;
                    case VrstaRasporeda.RasporedUredjajaPoAktuatorima:
                        try
                        {
                            UcitajSenzoreZaAktuatore(new RasporedSenzoraPoAktuatorima(linije[i]));
                        }
                        catch
                        {
                            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Linija je neispravna: {0}", linije[i]), VrstaLogZapisa.ERROR);
                        }
                        break;
                    default:
                        AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Nepoznata vrsta rasporeda: {0}", linije[i]), VrstaLogZapisa.ERROR);
                        break;
                }
            }
        }
        private void UcitajSenzoreZaAktuatore(RasporedSenzoraPoAktuatorima raspored)
        {
            foreach (var mjesto in _tofSustav.Mjesta)
            {
                foreach (var aktuator in mjesto.Aktuatori)
                {
                    if (aktuator.ExternalID == raspored.IDAktuatora)
                    {
                        aktuator.PovezaniUredjaji = mjesto.Senzori.Where(s => raspored.IDSenzora.Contains(s.ExternalID)).ToList();

                        foreach (var senzor in aktuator.PovezaniUredjaji)
                        {
                            senzor.PovezaniUredjaji.Add(aktuator);
                        }

                        return;
                    }
                }
            }
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Nepostojeći ili iskorišteni senzor/i za aktuator ID: {0}", raspored.IDAktuatora, VrstaLogZapisa.INFO));
        }

        private void UcitajRasporedZaMjesto(RasporedUredjajaPoMjestima raspored)
        {
            UcitajRasporedPoMjesta(raspored);
        }

        private void UcitajRasporedPoMjesta(RasporedUredjajaPoMjestima raspored)
        {
            var mjesto = (Mjesto)MaticniPodaci.Mjesta.Find(x => x.ID == raspored.IDMjesta).Clone();
            Uredjaj uredjaj;
            switch (raspored.VrstaUredjaja)
            {
                case VrstaUredjaja.Aktuator:
                    uredjaj = (Uredjaj)MaticniPodaci.Aktuatori.Find(x => x.ID == raspored.IDModelaUredjaja).Clone();
                    uredjaj.MjestoID = mjesto.ID;
                    if (uredjaj.Tip == mjesto.Tip || mjesto.Tip == Tip.VANJSKI_I_UNUTARNJI || uredjaj.Tip == Tip.VANJSKI_I_UNUTARNJI)
                    {
                        _tofSustav.Aktuatori.Add(uredjaj);
                        uredjaj.ExternalID = raspored.IDUredjaja;
                        mjesto.Aktuatori.DodajUredjajNaKraj(uredjaj);
                    } else
                    {
                        AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Neispravan tip aktuatora ID: {0} za mjesto ID: {1}", raspored.IDUredjaja, raspored.IDMjesta, VrstaLogZapisa.INFO));
                    }
                    break;
                case VrstaUredjaja.Senzor:
                    uredjaj = (Uredjaj)MaticniPodaci.Senzori.Find(x => x.ID == raspored.IDModelaUredjaja).Clone();
                    uredjaj.MjestoID = mjesto.ID;
                    if (uredjaj.Tip == mjesto.Tip || mjesto.Tip == Tip.VANJSKI_I_UNUTARNJI || uredjaj.Tip == Tip.VANJSKI_I_UNUTARNJI)
                    {
                        _tofSustav.Senzori.Add(uredjaj);
                        uredjaj.ExternalID = raspored.IDUredjaja;
                        mjesto.Senzori.DodajUredjajNaKraj(uredjaj);
                    }
                    else
                    {
                        AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Neispravan tip senzora ID: {0} za mjesto ID: {1}", raspored.IDUredjaja, raspored.IDMjesta, VrstaLogZapisa.INFO));
                    }
                    break;
                default:
                    break;
            }
            _tofSustav.Mjesta.Add(mjesto);
        }

        #endregion

        public void InicijalizirajSustav()
        {
            foreach (var mjesto in _tofSustav.Mjesta)
            {
                AplikacijskiPomagac.Instanca
                       .Logger.Log(string.Format("Počinje inicijalizacija uređaja mjesta {0}", mjesto.Naziv));
                InicijalizirajUredjaje(mjesto.Senzori);
                InicijalizirajUredjaje(mjesto.Aktuatori);
            }

            _sustavSpreman = true;
        }

        #region Inicijaliziraj sustav helperi
        private void InicijalizirajUredjaje(Uredjaj[] uredjaji)
        {
            foreach (var uredjaj in uredjaji)
            {
                if (uredjaj == null) continue;

                uredjaj.Inicijaliziraj(string.Format("Inicijalizacija uređaja {0}", uredjaj.Naziv));
                if (uredjaj.JeIspravan)
                {
                    AplikacijskiPomagac.Instanca
                        .Logger.Log(string.Format("Uređaj {0} je ispravan", uredjaj.Naziv));
                }
                else
                {
                    AplikacijskiPomagac.Instanca
                        .Logger.Log(string.Format("Uređaj {0} je neispravan", uredjaj.Naziv));
                }
            }
        }
        #endregion
    }
}
