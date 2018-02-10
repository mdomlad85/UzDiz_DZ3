using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Iznimke;

namespace Tof.Model
{
    public class RasporedSenzoraPoAktuatorima : Raspored
    {
        public RasporedSenzoraPoAktuatorima(string linija)
        {
            try
            {
                IDSenzora = new List<int>();
                var args = linija.Split(';');
                VrstaRasporeda = (VrstaRasporeda)int.Parse(args[0]);
                IDAktuatora = int.Parse(args[1]);
                foreach (var senzorIdStr in args[2].Split(','))
                {
                    int senzorId;
                    if(int.TryParse(senzorIdStr, out senzorId))
                    {
                        IDSenzora.Add(senzorId);
                    }
                }
            }
            catch
            {
                throw new RasporedSenzoraPoAktuatorimaException();
            }
        }

        public int IDAktuatora { get; set; }
        public VrstaUredjaja VrstaUredjaja { get; set; }
        public List<int> IDSenzora { get; set; }
    }
}
