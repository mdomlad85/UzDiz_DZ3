using System;
using System.Collections.Generic;
using Tof.Uzorci.Iterator;
using Tof.Iznimke;
using Tof.Uzorci.Singleton;

namespace Tof.Model
{
    public class Mjesto
    {
        public string Naziv { get; set; }

        public Tip Tip { get; set; }

        public KolekcijaUredjaja Senzori { get; set; }
        public KolekcijaUredjaja Aktuatori { get; set; }

        public int ID { get; set; }

        public Mjesto() { }

        public Mjesto(string[] attrs)
        {
            try
            {
                Naziv = attrs[0];
                Tip = (Tip)int.Parse(attrs[1]);
                Senzori = new KolekcijaUredjaja(int.Parse(attrs[2]));
                Aktuatori = new KolekcijaUredjaja(int.Parse(attrs[3]));
            }
            catch
            {
                throw new LosRedakIzDatoteke();
            }
        }

        internal void Provjeri()
        {
            ProvjeriUredjaje(Senzori, MaticniPodaci.Senzori, "Senzor");
            ProvjeriUredjaje(Aktuatori, MaticniPodaci.Aktuatori, "Aktuator");
        }

        private void ProvjeriUredjaje(KolekcijaUredjaja koristeniUredjaji, KolekcijaUredjaja raspoloziviUredjaji, string akcija)
        {
            var iteratorKoristenihUredjaj = koristeniUredjaji.CreateIterator();
            while (!iteratorKoristenihUredjaj.IsDone)
            {
                if (iteratorKoristenihUredjaj.CurrentItem.JeZdrav() == 1)
                {
                    AplikacijskiPomagac.Instanca
                        .Logger.Log(string.Format("Uređaj {0}({1}) je ispravan", iteratorKoristenihUredjaj.CurrentItem.Naziv, akcija));
                }
                else
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Uređaj {0} je neispravan", iteratorKoristenihUredjaj.CurrentItem.Naziv));
                    var zamjenski = koristeniUredjaji.Zamijeni(iteratorKoristenihUredjaj.CurrentItem, raspoloziviUredjaji);
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Uređaj {0} je zamijenjen sa {1}", iteratorKoristenihUredjaj.CurrentItem.Naziv, zamjenski.Naziv));
                }
                iteratorKoristenihUredjaj.Next();
            }
        }
    }
}
