using System;
using Tof.Model;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.FactoryMethod
{
    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    class UredjajRazlomljeno1Izvrsavanje : IUredjajAkcija
    {
        public void Izvrsi(Uredjaj uredjaj)
        {
            if (uredjaj.Obrnuto)
            {               
                uredjaj.TrenutnaVrijednost -= dohvatiVrijednost(uredjaj.TrenutnaVrijednost - uredjaj.Min);
            } else
            {
                uredjaj.TrenutnaVrijednost += dohvatiVrijednost(uredjaj.Max - uredjaj.TrenutnaVrijednost);
            }
        }

        private double dohvatiVrijednost(double max)
        {
            return Math.Round(AplikacijskiPomagac.Instanca.Nasumicnjak.DajSlucajniBroj(0.0f, (float)max), 1);
        }
    }
}
