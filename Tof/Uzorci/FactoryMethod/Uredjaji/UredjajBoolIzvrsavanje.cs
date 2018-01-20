using Tof.Model;

namespace Tof.Uzorci.FactoryMethod
{
    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    class UredjajBoolIzvrsavanje : IUredjajAkcija
    {
        public void Izvrsi(Uredjaj uredjaj)
        {
            uredjaj.TrenutnaVrijednost = uredjaj.TrenutnaVrijednost == 0 ? 1 : 0;
        }
    }
}
