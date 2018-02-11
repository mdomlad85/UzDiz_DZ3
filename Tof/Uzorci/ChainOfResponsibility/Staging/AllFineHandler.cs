using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.ChainOfResponsibility.Staging
{
    // Concrete Request Handler - AllFineHandler
    // Kada je vrijednost aktuatora u prvoj trećini smatra se da je sve u redu
    [ExportHandler]
    public class AllFineHandler : IRequestHandler
    {
        public IRequestHandler Successor { get; set; }

        public bool HandleRequest(IRequest request)
        {
            var req = (AktuatorRequest)request;
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator ID {0} AllFineHandler",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.INFO);
            var val = req.Aktuator.TrenutnaVrijednost / req.Aktuator.Max;
            if (val <= 0.33)
            {
                if (req.Aktuator.Obrnuto)
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Sve je u redu s aktuatorom ID {0} i vrijednost mu se smanjuje.",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.DEBUG);
                }
                else
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Sve je u redu s aktuatorom ID {0} ali mu se vrijednost povećava.",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.DEBUG);
                }
                return true;
            }

            return false;
        }
    }
}
