using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Uzorci.Builder;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.MVC
{
    public abstract class IController
    {
        protected IModel _model;

        protected TofSustav _tofSustav;

        public IController(IModel model)
        {
            _model = model;
        }

        public abstract void ObradiZahtjev(string zahtejv);

        public abstract void Inicijaliziraj(Postavke postavke);
    }
}
