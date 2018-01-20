using CommandLine.Text;
using System;
using Tof.Uzorci.Builder;
using Tof.Pomagaci;
using Tof.Uzorci.Singleton;
using Tof.Vendor;
using Tof.Logger;

namespace Tof
{
    class Program
    {
        static void Main(string[] args)
        {
            var postavke = new Postavke();
            try
            {
                postavke = FlagParser.GetOptions(args);
                AplikacijskiPomagac.Instanca.PostaviNasumicnjak(new Nasumicnjak.SistemskiNasumicnjak(postavke.Sjeme));
                MaticniPodaci.Ucitaj(postavke);
                var dzTofDirektor = new TofSustavDirector(new Dz2TofSustavBuilder(), postavke);
                dzTofDirektor.KreirajTofSustav();
                dzTofDirektor.TofSustav.Pokreni();
            }
            catch (Iznimke.NeispravniUlazniArgumenti)
            {
                Console.WriteLine("Došlo je do pogreške u postavkama.");
                var helptext = HelpText.AutoBuild(postavke, (HelpText current) => HelpText.DefaultParsingErrorsHandler(postavke, current));
                Console.WriteLine(helptext.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Pogreška: {0}", ex.Message));
            } finally
            {
                WriteStatistics();
            }
            AplikacijskiPomagac.Instanca.SpremiLog(postavke.IzlaznaDatoteka, postavke.BrojLinijaIzlaza);
        }
        private static void WriteStatistics()
        {
            var table = new ConsoleTable("#Uspješnih ciklusa", "#Neuspješnih ciklusa", "#Prosječno izvršavanje");

            table.AddRow(
                AplikacijskiPomagac.Instanca.Statistika.UspjesnihCiklusa,
                AplikacijskiPomagac.Instanca.Statistika.NeuspjesnihCiklusa,
                AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa.ToString("N2") + " sec"
                );

            AplikacijskiPomagac.Instanca.Logger.Log(string.Format(" Statistika{0}{1}", 
                Environment.NewLine, table.ToStringAlternative()), VrstaLogZapisa.INFO);

            table.Write(Format.Alternative);
        }
    }
}
