using System;
using Tof.Logger;
using Tof.Model;
using Tof.Uzorci.Iterator;

namespace Tof.Uzorci.Singleton
{
    public static class MaticniPodaci
    {
        static MaticniPodaci()
        {
            Senzori = new KolekcijaUredjaja();
            Aktuatori = new KolekcijaUredjaja();
        }

        private static object syncLock = new object();

        private static KolekcijaUredjaja _aktuatori;
        internal static KolekcijaUredjaja Aktuatori
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

        internal static void Ucitaj(Postavke options)
        {
            lock (syncLock)
            {
                _senzori = DohvatiUredjaje(options.DatotekaSenzora);
                _aktuatori = DohvatiUredjaje(options.DatotekaAktuatora);
            }
        }

        private static KolekcijaUredjaja DohvatiUredjaje(string datoteka)
        {
            var kolekcija = new KolekcijaUredjaja();
            string[] linije = datoteka.ReadAllLinesExceptFirstN();

            for (int i = 0; i < linije.Length; i++)
            {
                try
                {
                    kolekcija[i] = new Uredjaj(linije[i].Split(';'));
                }
                catch (Exception ex)
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(ex.Message, VrstaLogZapisa.ERROR);
                }
            }
            return kolekcija;
        }

        private static KolekcijaUredjaja _senzori;
        internal static KolekcijaUredjaja Senzori
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
    }
}
