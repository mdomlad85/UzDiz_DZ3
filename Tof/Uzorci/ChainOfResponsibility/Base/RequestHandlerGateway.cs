using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tof.Uzorci.ChainOfResponsibility
{
    //A gateway which stitches together the handlers,
    //to accept a request to chain through the handlers
    //Note that this does the composition using MEF
    public class RequestHandlerGateway
    {
        [ImportMany(typeof(IRequestHandler))]
        public IEnumerable<Lazy<IRequestHandler, IRequestHandlerMetadata>> Handlers { get; set; }

        private IRequestHandler first = null;

        public RequestHandlerGateway()
        {
            ComposeHandlers();

            //Let us find and keep the first handler
            //i.e, the handler which is not a successor of any other handlers
            first = Handlers.First
                    (handler => handler.Metadata.SuccessorOf == null).Value;
        }

        //Compose the handlers
        void ComposeHandlers()
        {
            //A catalog that can aggregate other catalogs
            var aggrCatalog = new AggregateCatalog();
            //An assembly catalog to load information about part from this assembly
            var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            aggrCatalog.Catalogs.Add(asmCatalog);

            //Create a container
            var container = new CompositionContainer(aggrCatalog);
            //Composing the parts
            container.ComposeParts(this);
        }

        //Try to handle the request, pass to successor if required
        bool TryHandle(IRequestHandler handler, IRequest req)
        {
            var s =
                Handlers.FirstOrDefault(
                    h => h.Metadata.SuccessorOf == handler.GetType());

            if (handler.HandleRequest(req))
                return true;
            else if (s != null)
            {
                handler.Successor = s.Value;
                return TryHandle(handler.Successor, req);
            }
            else
                return false;
        }

        //Main gateway method for invoking the same from the driver
        public bool HandleRequest(IRequest request)
        {
            return TryHandle(first, request);
        }
    }
}
