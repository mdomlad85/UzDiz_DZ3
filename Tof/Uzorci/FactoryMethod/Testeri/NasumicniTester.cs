using System;
using System.Collections.Generic;
using Tof.Model;
using Tof.Uzorci.Singleton;

/// <summary>
/// A 'ConcreteProduct' class
/// </summary>
namespace Tof.Uzorci.FactoryMethod
{
    class NasumicniTester : ITesterUredjaja
    {
        public void ProvjeriMjesta(Iterator.KolekcijaMjesta mjesta)
        {
            AplikacijskiPomagac.Instanca.Logger.Log("Start provjere mjesta nasumičnim testerom");
            var keys = new HashSet<int>();
            int brojMjesta = mjesta.Count;

            do
            {
                var val = 1;
                do
                {
                    val = AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0, brojMjesta);
                } while (!keys.Add(val));

                mjesta[val].Provjeri();

            } while (keys.Count != brojMjesta);

            AplikacijskiPomagac.Instanca.Logger.Log("Kraj provjere mjesta nasumičnim testerom");
        }
    }
}

