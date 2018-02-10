using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Iznimke;

namespace Tof.Model
{
    public class RasporedUredjajaPoMjestima : Raspored
    {
        public RasporedUredjajaPoMjestima(string linija)
        {
            try
            {
                var args = linija.Split(';');
                VrstaRasporeda = (VrstaRasporeda)int.Parse(args[0]);
                IDMjesta = int.Parse(args[1]);
                VrstaUredjaja = (VrstaUredjaja)int.Parse(args[2]);
                IDModelaUredjaja = int.Parse(args[3]);
                IDUredjaja = int.Parse(args[4]);
            } catch
            {
                throw new RasporedUredjajaPoMjestimaException();
            }
        }

        public int IDMjesta { get; set; }
        public VrstaUredjaja VrstaUredjaja { get; set; }

        public int IDModelaUredjaja { get; set; }

        public int IDUredjaja { get; set; }
    }
}
