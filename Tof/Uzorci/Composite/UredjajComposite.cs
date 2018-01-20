using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tof.Model;

namespace Tof.Uzorci.Composite
{
    class UredjajComposite : UredjajComponent
    {
        private List<UredjajComponent> _children = new List<UredjajComponent>();

        public UredjajComposite(Uredjaj uredjaj) : base(uredjaj)
        {
        }

        public override void Add(UredjajComponent c)
        {
            _children.Add(c);
        }

        public override void AddRange(ICollection<UredjajComponent> components)
        {
            _children.AddRange(components);
        }

        public override void Remove(UredjajComponent c)
        {
            _children.Remove(c);
        }

        public override void Display(string uvlaka)
        {
            base.Display(uvlaka);

            foreach (var child in _children)
            {
                child.Display(string.Format("{0}{1}", uvlaka, uvlaka));
            }
        }
    }
}
