using System.Collections.Generic;
using Tof.Uzorci.Singleton;
using Tof.Uzorci.Iterator;

/// <summary>
/// A 'ConcreteProduct' class
/// </summary>
namespace Tof.Uzorci.FactoryMethod
{
    class HibridniTester : ITesterUredjaja
    {
        public void ProvjeriMjesta(KolekcijaMjesta mjesta)
        {
            AplikacijskiPomagac.Instanca.Logger.Log("Start provjere mjesta hibridnim testerom");
            var keys = new HashSet<int>();
            int brojMjesta = mjesta.Count / 2;

            do
            {
                var val = 1;
                do
                {
                    val = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0, brojMjesta);
                } while (!keys.Add(val));

                mjesta[val].Provjeri();

            } while (keys.Count != brojMjesta);

            for (int i = 0; i < mjesta.Count; i++)
            {
                if (keys.Add(i))
                {
                    mjesta[i].Provjeri();
                }
            }

            AplikacijskiPomagac.Instanca.Logger.Log("Kraj provjere mjesta hibridnim testerom");
        }
    }
}
