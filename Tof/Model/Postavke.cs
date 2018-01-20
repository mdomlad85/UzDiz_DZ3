using CommandLine;
using CommandLine.Text;
using System;
using System.IO;
using Tof.Uzorci.FactoryMethod;
using Tof.Nasumicnjak;
using Tof.Uzorci.Singleton;

namespace Tof
{
    public class Postavke
    {
        public const string CREATOR = "mdomladov_";
        public Postavke()
        {
            Sjeme = -1;
            TrajanjeDretveSek = -1;
            BrojCiklusaDretve = -1;
            BrojLinijaIzlaza = -1;
        }

        public ITesterUredjaja AlgoritamProvjere { get; set; }

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

        [Option("alg", Required = true,
          HelpText = "puni naziv klase algoritma provjere koja se dinamički učitava.")]
        public string AlgoritamProvjereNaziv { get; set; }

        [Option("tcd", Required = false,
         HelpText = "trajanje ciklusa dretve u sek. Ako nije upisana opcija, uzima se slučajni broj u intervalu 1 - 17.")]
        public int TrajanjeDretveSek { get; set; }

        [Option("bcd", Required = false,
          HelpText = "broj ciklusa dretve. Ako nije upisana opcija, uzima se slučajni broj u intervalu 1 - 23.")]
        public int BrojCiklusaDretve { get; set; }

        [Option('i', Required = false,
          HelpText = "naziv datoteke u koju se sprema izlaz programa. Ako nije upisana opcija, uzima se vlastito korisničko ime kojem se dodaje trenutni podaci vremena po formatu _ggggmmdd_hhmmss.txt npr. mdomladov_20171105_203128.txt")]
        public string IzlaznaDatoteka { get; set; }

        [Option("brl", Required = false,
         HelpText = "broj linija u spremniku za upis u datoteku za izlaz. Ako nije upisana opcija, uzima se slučajni broj u intervalu 100 - 999.")]
        public int BrojLinijaIzlaza { get; set; }

        [HelpOption('h', "help",
            HelpText = "Pomoć")]
        public string DohvatiPomoc()
        {
            var helpText = HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            helpText.Copyright = "mdomladov";
            helpText.Heading = "Pomoć";

            return helpText;
        }

        [Option('v', "verbose", DefaultValue = true,
                 HelpText = "Ispiši sve poruke na standardni izlaz.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        public void PopuniPredefiniraneVrijednosti()
        {
            if (Sjeme == -1)
            {
                Sjeme = new Random().Next(100, 65535);
            }
            AplikacijskiPomagac.Instanca.PostaviNasumicnjak(new SistemskiNasumicnjak(Sjeme));
            
            if(TrajanjeDretveSek == -1)
            {
                TrajanjeDretveSek = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(1, 17);             
            }

            if (BrojCiklusaDretve == -1)
            {
                BrojCiklusaDretve = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(1, 23);
            }

            if (BrojLinijaIzlaza == -1)
            {
                BrojLinijaIzlaza = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(1, 17);
            }

            if (TrajanjeDretveSek == -1)
            {
                TrajanjeDretveSek = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(1, 17);
            }

            if (string.IsNullOrWhiteSpace(IzlaznaDatoteka))
            {
                IzlaznaDatoteka = string.Format("{0}{1}", CREATOR, DateTime.Now.ToString("_yyyyMMdd_hhmmss"));
            }
        }

        public bool JesuPostavkeIspravne()
        {
            return FilesExists(DatotekaAktuatora, DatotekaMjesta, DatotekaSenzora)
                && TesterClassExists(AlgoritamProvjereNaziv);
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
        private bool TesterClassExists(string className)
        {
            try
            {
                AlgoritamProvjere = TofTvornicaTestera.Instanca.ProizvediTestera(className);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
