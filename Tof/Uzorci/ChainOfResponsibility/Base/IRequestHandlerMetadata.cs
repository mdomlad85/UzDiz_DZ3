using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tof.Uzorci.ChainOfResponsibility
{
    //The metadata to tie a handler to next successor
    public interface IRequestHandlerMetadata
    {
        Type SuccessorOf { get; }
    }
}
