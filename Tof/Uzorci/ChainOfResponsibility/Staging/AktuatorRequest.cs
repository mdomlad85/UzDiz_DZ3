using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Model;

namespace Tof.Uzorci.ChainOfResponsibility.Staging
{
    //Concrete Request
    public class AktuatorRequest : IRequest
    {
        public Uredjaj Aktuator { get; set; }
    }
}
