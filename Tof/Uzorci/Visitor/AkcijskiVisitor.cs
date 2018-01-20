using System;
using Tof.Uzorci.FactoryMethod;
using Tof.Model;

namespace Tof.Uzorci.Visitor
{
    class AkcijskiVisitor : IUredjajVisitor
    {
        public void Visit(Uredjaj uredjaj)
        {
            TofTvornicaUredjaja
                .Instanca
                .ProizvediDinamikuUredjaja(uredjaj.Vrsta)
                .Izvrsi(uredjaj);
        }
    }
}
