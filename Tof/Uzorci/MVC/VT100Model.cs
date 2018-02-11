using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Tof.Model;
using Tof.Uzorci.Builder;
using Tof.Uzorci.ChainOfResponsibility;
using Tof.Uzorci.ChainOfResponsibility.Staging;
using Tof.Uzorci.Singleton;
using Tof.Vendor;

namespace Tof.Uzorci.MVC
{
    public class VT100Model : IModel
    {
        public override void IspisPodatakaAktuatora(Uredjaj aktuator)
        {
            _writer.Ocisti(); 
            _writer.Log(string.Format("Informacije o aktuatoru s ID:{0}", aktuator.ExternalID));
            _writer.Log(string.Format("Naziv:{0}", aktuator.Naziv));
            _writer.Log(string.Format("Tip:{0}", Enum.GetName(typeof(Tip), aktuator.Tip)));
            _writer.Log(string.Format("Vrsta:{0}", Enum.GetName(typeof(Vrsta), aktuator.Vrsta)));
            _writer.Log(string.Format("Trenutna Vrijednost:{0}", aktuator.TrenutnaVrijednost));
            _writer.Log(string.Format("Komentar:{0}", aktuator.Komentar));
            _writer.Log(string.Format("MjestoID: {0}", aktuator.MjestoID));
            _writer.Log(string.Format("Broj povezanih senzora:{0}", aktuator.PovezaniUredjaji.Count()));
            foreach (var senzor in aktuator.PovezaniUredjaji)
            {
                _writer.Log(string.Format("--Informacije o senzoru s ID:{0}", senzor.ExternalID));
                _writer.Log(string.Format("--Naziv:{0}", senzor.Naziv));
                _writer.Log(string.Format("--Tip:{0}", Enum.GetName(typeof(Tip), senzor.Tip)));
                _writer.Log(string.Format("--Vrsta:{0}", Enum.GetName(typeof(Vrsta), senzor.Vrsta)));
                _writer.Log(string.Format("--Trenutna Vrijednost:{0}", senzor.TrenutnaVrijednost));
                _writer.Log(string.Format("--Komentar:{0}", senzor.Komentar));
            }
            Notify();
        }
        public override void IspisPodatakaMjesta(Mjesto mjesto)
        {
            _writer.Ocisti();
            _writer.Log(string.Format("Informacije o mjestu s ID:{0}", mjesto.ID));
            _writer.Log(string.Format("Naziv:{0}", mjesto.Naziv));
            _writer.Log(string.Format("Tip:{0}", Enum.GetName(typeof(Tip), mjesto.Tip)));
            _writer.Log(string.Format("Broj povezanih senzora:{0}", mjesto.Senzori.Count()));
            foreach (var senzor in mjesto.Senzori)
            {
                _writer.Log(string.Format("--ID:{0}", senzor.ExternalID));
                _writer.Log(string.Format("--Naziv:{0}", senzor.Naziv));
            }
            Notify();
        }
        public override void IspisPodatakaSenzora(Uredjaj senzor)
        {            
            _writer.Ocisti();
            _writer.Log(string.Format("Informacije o senzoru s ID:{0}", senzor.ExternalID));
            _writer.Log(string.Format("Naziv:{0}", senzor.Naziv));
            _writer.Log(string.Format("Tip:{0}", Enum.GetName(typeof(Tip), senzor.Tip)));
            _writer.Log(string.Format("Vrsta:{0}", Enum.GetName(typeof(Vrsta), senzor.Vrsta)));
            _writer.Log(string.Format("Trenutna Vrijednost:{0}", senzor.TrenutnaVrijednost));
            _writer.Log(string.Format("Komentar:{0}", senzor.Komentar));
            _writer.Log(string.Format("MjestoID: {0}", senzor.MjestoID));
            Notify();
        }
        public override void IspisStatistike()
        {
            var table = new ConsoleTable("#Uspješnih ciklusa", "#Neuspješnih ciklusa", "#Prosječno izvršavanje");

            
            table.AddRow(
                AplikacijskiPomagac.Instanca.Statistika.UspjesnihCiklusa,
                AplikacijskiPomagac.Instanca.Statistika.NeuspjesnihCiklusa,
                AplikacijskiPomagac.Instanca.Statistika.ProsjecnoTrajanjeCiklusa.ToString("N2") + " sec"
                );

            _writer.Log(string.Format(" Statistika{0}{1}", Environment.NewLine, table.ToStringAlternative()));
            Notify();
        }
        public override void Izadji()
        {
            // Nije potrebno jer ionako zavrsava ako nema notify no zlu ne trebalo
            Environment.Exit((int)ExitCode.Success);
        }
        public override void PostaviPctIspravnostUredjaja(int pct)
        {
            _writer.Ocisti();
            if (pct > 0 && pct <= 100)
            {
                Postavke.Instanca.ProsjecnaIspravnost = pct;
                _writer.Log(string.Format("Prosječna ispravnost je na {0}%", pct));
                Notify();
            }
            else
            {
                _writer.Log(string.Format("Prosječna ispravnost mora biti između 1 i 100. Vi ste unijeli {0}", pct));
            }
        }
        public override void SpremiPodatke(TofSustav sustav)
        {
            _writer.Ocisti();
            try
            {
                using (Stream stream = new FileStream(FILE_NAME, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, sustav);
                }
                _writer.Log("Podaci su uspješno spremljeni.");
            }
            catch
            {
                _writer.Log("Podaci nisu uspješno spremljeni.");
            }
            Notify();
        }
        public override TofSustav VratiSpremljenePodatke()
        {
            _writer.Ocisti();
            try
            {
                using (Stream stream = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var data = (TofSustav)formatter.Deserialize(stream);
                    _writer.Log("Podaci su uspješno učitani.");
                    return data;
                }                
            }
            catch
            {
                _writer.Log("Podaci nisu uspješno učitani.");
                return null;
            }
        }
        public override void IspisiPomoc()
        {
            _writer.Ocisti();
            _writer.Log("M x - ispis podataka mjesta x");
            _writer.Log("S x - ispis podataka senzora x");
            _writer.Log("A x - ispis podataka aktuatora x");
            _writer.Log("S - ispis statistike");
            _writer.Log("SP - spremi podatke (mjesta, uređaja)");
            _writer.Log("VP - vrati spremljene podatke (mjesta, uređaja)");
            _writer.Log("C n - izvršavanje n ciklusa dretve (1-100)");
            _writer.Log("VF n - za aktuator s ID n provjeriti u kojoj je fazi vrijednosti");
            _writer.Log("PI n - prosječni % ispravnosti uređaja (0-100)");
            _writer.Log("H - pomoć, ispis dopuštenih komandi i njihov opis");
            _writer.Log("I - izlaz.");
            Notify();
        }
        public override void IzvrsiNCiklusaDretve(int brojCiklusa, TofSustav sustav)
        {
            Postavke.Instanca.BrojCiklusaDretve = brojCiklusa;
            sustav.Pokreni(this);
        }

        public override void VlastitaFunkcionalnost(Uredjaj uredjaj)
        {
            AplikacijskiPomagac.Instanca.Logger.Ocisti();
            var request = new AktuatorRequest { Aktuator = uredjaj };
            var gw = new RequestHandlerGateway();
            if (!gw.HandleRequest(request))
            {
                _writer.Log("Ne postoji upravitelj trazenog zahtjeva!", Logger.VrstaLogZapisa.ERROR);
            } else
            {
                foreach (var line in AplikacijskiPomagac.Instanca.Logger.PovijestLogiranja.ToArray())
                {
                    _writer.PovijestLogiranja.AppendLine(line);
                }
                AplikacijskiPomagac.Instanca.Logger.PovijestLogiranja.Clear();
            }
            Notify();
        }

        private string FILE_NAME = "sustav.bin";

        private IFormatter formatter = new BinaryFormatter();
    }
}
