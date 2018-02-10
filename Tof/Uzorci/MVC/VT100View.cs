using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.MVC
{
    public class VT100View : IView
    {
        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point()
            {
                X = 1;
                Y = 1;
            }
        }
        
        public override void Update(IModel model)
        {
            Izvrsi(ANSI_VT100_Konstante.Erase.ENTIRE_DISPLAY);

            var pozicija = new Point { X = 1, Y = 1 };
            var modelData = model.DohvatiLinije();
            foreach (var linija in modelData)
            {
                Postavi(pozicija);
                Izvrsi(GetBoja(linija), linija);
                pozicija.X++;

               if(pozicija.X >= Postavke.Instanca.BrojRedakaIspis - 1)
                {
                    string input;
                    do
                    {
                        PostaviNaKrajReda();
                        Izvrsi(ANSI_VT100_Konstante.Color.WHITE, string.Empty);
                        Izvrsi("*", "UPopunjen je ekran. Za nastavak pristisnite n/N");
                        input = Console.ReadLine();
                    } while (!input.ToLower().Equals("n"));
                    Izvrsi(ANSI_VT100_Konstante.Erase.ENTIRE_DISPLAY);
                    pozicija.X = 1;
                }
            }
            model._writer.Ocisti();
            PostaviNaKrajReda();
            Izvrsi(ANSI_VT100_Konstante.Color.WHITE, "Naredba: ");
            _controller.ObradiZahtjev(Console.ReadLine().Trim());
        }

        private string GetBoja(string linija)
        {
            if (linija.Contains("DEBUG")) return ANSI_VT100_Konstante.Color.GIGI;
            if (linija.Contains("WARNING")) return ANSI_VT100_Konstante.Color.YELLOW;
            if (linija.Contains("ERROR")) return ANSI_VT100_Konstante.Color.RED;

            return ANSI_VT100_Konstante.Color.WHITE;
        }

        private void PostaviNaKrajReda()
        {
            Postavi(new Point { X = Postavke.Instanca.BrojRedakaIspis, Y = 1 });
        }

        private void Postavi(Point pozicija)
        {
            Izvrsi(string.Format("{0};{1}f", pozicija.X, pozicija.Y));
        }

        private void Izvrsi(string komanda, string dodatak = "")
        {
            Console.WriteLine(string.Format("{0}{1}{2}", 
                ANSI_VT100_Konstante.ESC, komanda, dodatak));
        }
    }
}
