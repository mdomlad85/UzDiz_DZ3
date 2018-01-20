using System.Collections.Generic;
using Tof.Uzorci.Iterator;
using Tof.Uzorci.Singleton;

/// <summary>
/// A 'ConcreteProduct' class
/// </summary>
namespace Tof.Uzorci.FactoryMethod
{
    class SekvencijalniTester : ITesterUredjaja
    {
        public void ProvjeriMjesta(KolekcijaMjesta mjesta)
        {
            AplikacijskiPomagac.Instanca.Logger.Log("Start provjere mjesta sekvencijalnim testerom");
            var keys = new HashSet<int>();

            var iteratorMjesta = mjesta.CreateIterator();
            while (!iteratorMjesta.IsDone)
            {
                iteratorMjesta.CurrentItem.Provjeri();
                iteratorMjesta.Next();
            }

            AplikacijskiPomagac.Instanca.Logger.Log("Kraj provjere mjesta sekvencijalnim testerom");
        }
    }
}
