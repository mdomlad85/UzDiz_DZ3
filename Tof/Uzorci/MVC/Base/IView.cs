using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tof.Uzorci.MVC
{
    public abstract class IView
    {
        protected IController _controller;
        public abstract void Update(IModel model);

        public virtual void SetController(IController controller)
        {
            _controller = controller;
        }
    }
}
