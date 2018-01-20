using System;
using Tof.Iznimke;

namespace Tof.Nasumicnjak
{
    public class SistemskiNasumicnjak : INasumicnjak
    {
        private Random _rnd;

        public SistemskiNasumicnjak(string strSjeme)
        {
            int sjeme;

            if(int.TryParse(strSjeme, out sjeme))
            {
                _rnd = new Random(sjeme);
            } else
            {
                throw new SjemeMoraBitiBroj();
            }
        }

        public SistemskiNasumicnjak(int sjeme)
        {
            _rnd = new Random(sjeme);
        }

        public SistemskiNasumicnjak()
        {

        }

        public void SetirajGenerator(int sjeme)
        {
            _rnd = new Random(sjeme);
        }

        public void SetirajGenerator(string strSjeme)
        {
            int sjeme;

            if (int.TryParse(strSjeme, out sjeme))
            {
                _rnd = new Random(sjeme);
            }
            else
            {
                throw new SjemeMoraBitiBroj();
            }
        }

        public int DajSlucajniBroj(int odBroja, int doBroja)
        {
            if(odBroja < doBroja)
            {
                return _rnd.Next(odBroja, doBroja);
            } else
            {
                return _rnd.Next(doBroja, odBroja);
            }
        }

        public float DajSlucajniBroj(float odBroja, float doBroja)
        {
            float broj = odBroja;
            if (odBroja < doBroja)
            {
                broj = _rnd.Next((int)Math.Floor(odBroja), (int)doBroja);
            }
            else
            {
                broj = _rnd.Next((int)Math.Floor(doBroja), (int)odBroja);
            }
            
            do
            {
                broj += (float)_rnd.NextDouble();
            } while (broj > odBroja && broj < doBroja);

            return broj;
        }
    }
}
