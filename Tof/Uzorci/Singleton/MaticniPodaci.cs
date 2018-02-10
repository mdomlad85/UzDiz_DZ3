using System;
using System.Collections.Generic;
using Tof.Logger;
using Tof.Model;

namespace Tof.Uzorci.Singleton
{
    public static class MaticniPodaci
    {
        static MaticniPodaci()
        {
            Senzori = new List<Uredjaj>();
            Aktuatori = new List<Uredjaj>();
            Mjesta = new List<Mjesto>();
        }

        private static object syncLock = new object();

        private static List<Uredjaj> _aktuatori;
        internal static List<Uredjaj> Aktuatori
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

        private static List<Uredjaj> _senzori;
        internal static List<Uredjaj> Senzori
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

        private static List<Mjesto> _mjesta;
        internal static List<Mjesto> Mjesta
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

        internal static void Ucitaj(Postavke options)
        {
            lock (syncLock)
            {
                _senzori = UcitajUredjaje(options.DatotekaSenzora);
                _aktuatori = UcitajUredjaje(options.DatotekaAktuatora);
                _mjesta = UcitajMjesta(options.DatotekaMjesta);
            }
        }

        private static List<Mjesto> UcitajMjesta(string datoteka)
        {
            var kolekcija = new List<Mjesto>();
            string[] linije = datoteka.ReadAllLinesExceptFirstN();

            for (int i = 0; i < linije.Length; i++)
            {
                try
                {
                    kolekcija.Add(new Mjesto(linije[i].Split(';')));
                }
                catch (Exception ex)
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(ex.Message, VrstaLogZapisa.ERROR);
                }
            }
            return kolekcija;
        }

        private static List<Uredjaj> UcitajUredjaje(string datoteka)
        {
            var kolekcija = new List<Uredjaj>();
            string[] linije = datoteka.ReadAllLinesExceptFirstN();

            for (int i = 0; i < linije.Length; i++)
            {
                try
                {
                    kolekcija.Add(new Uredjaj(linije[i].Split(';')));
                }
                catch (Exception ex)
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(ex.Message, VrstaLogZapisa.ERROR);
                }
            }
            return kolekcija;
        }
    }
}
