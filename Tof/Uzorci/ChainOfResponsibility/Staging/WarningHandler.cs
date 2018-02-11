using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Uzorci.Singleton;

namespace Tof.Uzorci.ChainOfResponsibility.Staging
{
    // Concrete Request Handler - WarningeHandler
    // Kada je vrijednost u drugoj trećini i još ako vrijednost raste potreban je oprez
    [ExportHandler(SuccessorOf = typeof(AllFineHandler))]
    public class WarningHandler : IRequestHandler
    {
        public IRequestHandler Successor { get; set; }

        public bool HandleRequest(IRequest request)
        {
            var req = (AktuatorRequest)request;
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator ID {0} WarningHandler",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.INFO);
            var val = req.Aktuator.TrenutnaVrijednost / req.Aktuator.Max;
            if (val < 0.67 && val > 0.33)
            {
                if (req.Aktuator.Obrnuto)
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Aktuator s ID {0} u žutoj zoni, ali vrijednost mu se smanjuje.",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.WARNING);
                } else
                {
                    AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Pažnja!!! Aktuator s ID {0} u žutoj zoni i mu se povećava.",
                        req.Aktuator.ExternalID), Logger.VrstaLogZapisa.WARNING);
                }
                return true;
            }

            return false;
        }
    }
}
