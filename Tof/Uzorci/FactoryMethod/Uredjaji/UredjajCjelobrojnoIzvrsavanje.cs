using Tof.Model;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.FactoryMethod
{
    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    class UredjajCjelobrojnoIzvrsavanje : IUredjajAkcija
    {
        public void Izvrsi(Uredjaj uredjaj)
        {
            if (uredjaj.Obrnuto)
            {
                int max = (int)uredjaj.TrenutnaVrijednost - (int)uredjaj.Min + 1;
                uredjaj.TrenutnaVrijednost -= AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0, max);
            } else
            {
                int max = (int)uredjaj.Max - (int)uredjaj.TrenutnaVrijednost + 1;
                uredjaj.TrenutnaVrijednost += AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0, max);
            }
        }
    }
}
