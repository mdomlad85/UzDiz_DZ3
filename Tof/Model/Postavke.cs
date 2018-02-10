using CommandLine;
using CommandLine.Text;
using System;
using System.IO;
using Tof.Uzorci.FactoryMethod;
using Tof.Nasumicnjak;
using Tof.Uzorci.Singleton;

namespace Tof
{
    [Serializable]
    public class Postavke
    {
        public const string CREATOR = "mdomladov_";
        public Postavke()
        {
            BrojRedaka = 24;
            BrojStupaca = 80;
            BrojRedakaKomandi = 2;
            ProsjecnaIspravnost = 50;
            Sjeme = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;
            TrajanjeDretveSek = (new Random()).Next(1, 17);
        }

        public int BrojRedakaIspis => BrojRedaka - BrojRedakaKomandi;

        [Option("br", Required = false,
           HelpText = " broj redaka na ekranu (24-40). Ako nije upisana opcija, uzima se 24.")]
        public int BrojRedaka { get; set; }

        [Option("bs", Required = false,
            HelpText = "broj stupaca na ekranu (80-160). Ako nije upisana opcija, uzima se 80.")]
        public int BrojStupaca { get; set; }

        [Option("brk", Required = false,
            HelpText = "broj redaka na ekranu za unos komandi (2-5). Ako nije upisana opcija, uzima se 2.")]
        public int BrojRedakaKomandi { get; set; }

        [Option("pi", Required = false,
            HelpText = "prosječni % ispravnosti uređaja (0-100). Ako nije upisana opcija, uzima se 50.")]
        public int ProsjecnaIspravnost { get; set; }

        [Option('g', Required = false,
            HelpText = "sjeme za generator slučajnog broja (u intervalu 100 - 65535). Ako nije upisana opcija, uzima se broj milisekundi u trenutnom vremenu na bazi njegovog broja sekundi i broja milisekundi.")]
        public int Sjeme { get; set; }

        [Option('m', Required = true,
           HelpText = "naziv datoteke mjesta.")]
        public string DatotekaMjesta { get; set; }

        [Option('s', Required = true,
           HelpText = "naziv datoteke senzora.")]
        public string DatotekaSenzora { get; set; }

        [Option('a', Required = true,
          HelpText = "naziv datoteke aktuatora.")]
        public string DatotekaAktuatora { get; set; }

        [Option('r', Required = true,
          HelpText = "naziv datoteke aktuatora.")]
        public string DatotekaRasporeda { get; set; }

        [Option("tcd", Required = false,
         HelpText = "trajanje ciklusa dretve u sek. Ako nije upisana opcija, uzima se slučajni broj u intervalu 1 - 17.")]
        public int TrajanjeDretveSek { get; set; }

        public bool JesuPostavkeIspravne()
        {
            return FilesExists(DatotekaAktuatora, DatotekaMjesta, DatotekaSenzora, DatotekaRasporeda);
        }

        private bool FilesExists(params string[] vals)
        {
            foreach (var val in vals)
            {
                if (!File.Exists(val))
                {
                    return false;
                }
            }
            return true;
        }

        private static object staticSyncLock = new object();

        private static Postavke _instanca;

        public static Postavke Instanca
        {
            get
            {
                // Dvostruko zaključavanje zbog sigurnog pristupa u višedretvenom pristupu
                if (_instanca == null)
                {
                    lock (staticSyncLock)
                    {
                        if (_instanca == null)
                        {
                            _instanca = new Postavke();
                        }
                    }
                }

                return _instanca;
            }

        }
        public int BrojCiklusaDretve { get; set; }
    }
}
