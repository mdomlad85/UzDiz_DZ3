using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Model;

namespace Tof.Uzorci.Visitor
{
    class RadniVisitor : IUredjajVisitor
    {
        public void Visit(Uredjaj uredjaj)
        {
            uredjaj.OdradiPosao();
        }
    }
}
