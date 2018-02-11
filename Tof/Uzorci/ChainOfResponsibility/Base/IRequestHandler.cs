using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tof.Uzorci.ChainOfResponsibility
{
    //Abstract Request Handler
    public interface IRequestHandler
    {
        bool HandleRequest(IRequest req);
        IRequestHandler Successor { get; set; }
    }
}
