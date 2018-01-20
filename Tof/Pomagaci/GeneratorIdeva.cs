using System.Collections.Generic;
using Tof.Iznimke;
using Tof.Uzorci.Singleton;

namespace Tof.Pomagaci
{
    public class GeneratorIdeva
    {
        public readonly int MIN = 1;

        public readonly int MAX = 1000;

        private HashSet<int> _popunjeniIdjevi = new HashSet<int>();

        public GeneratorIdeva(int min, int max)
        {
            MIN = min;
            MAX = max;
        }

        public GeneratorIdeva(int max)
        {
            MAX = max;
        }

        public GeneratorIdeva(int num, bool isMax)
        {
            if (isMax)
            {
                MAX = num;
            } else
            {
                MIN = num;
            }
        }

        public GeneratorIdeva()
        {

        }

        public int DajSljedeciId()
        {
            if(_popunjeniIdjevi.Count == MAX)
            {
                throw new NemaViseMjestaUKolekciji();
            }

            bool added = false;
            int id = -1;
            do
            {
                id = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(MIN, MAX);
                added = _popunjeniIdjevi.Add(id);
            } while (!added);

            return id;
        }
    }
}
