using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.ChainOfResponsibility.Staging
{
    // Concrete Request Handler - DangerHandler
    // Ukoliko je vrijednost u zadnjoj trećini smatra se da postoji opasnost za senzor
    [ExportHandler(SuccessorOf = typeof(WarningHandler))]
    public class DangerHandler : IRequestHandler
    {
        public IRequestHandler Successor { get; set; }

        public bool HandleRequest(IRequest request)
        {
            var req = (AktuatorRequest)request;
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator ID {0} DangerHandler",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.INFO);
            var val = req.Aktuator.TrenutnaVrijednost / req.Aktuator.Max;
            if (val >= 0.67)
            {
                if (req.Aktuator.Obrnuto)
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator ID {0} u crvenoj zoni, ali vrijednost mu se smanjuje.",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.ERROR);
                } else
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("OPASNOST!!! Aktuator ID {0} u crvenoj zoni, i vrijednost mu se povećava.",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.ERROR);
                }
                return true;
            }

            return false;
        }
    }
}
