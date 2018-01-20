using System.Collections.Generic;
using Tof.Uzorci.Iterator;

namespace Tof.Uzorci.Visitor
{
    /// <summary>
    /// 'ObjectStructure' klasa
    /// </summary>
    class UredjajiObjectStructure
    {
        private Dictionary<int, UredjajElement> _uredjajElementi = new Dictionary<int, UredjajElement>();
        public UredjajiObjectStructure(KolekcijaUredjaja uredjaji)
        {
            var uredjajiIterator = uredjaji.CreateIterator();
            while (!uredjajiIterator.IsDone)
            {
                _uredjajElementi[uredjajiIterator.CurrentItem.ID] = uredjajiIterator.CurrentItem;
                uredjajiIterator.Next();
            }
        }

        public UredjajiObjectStructure()
        {

        }

        public void Attach(int id, UredjajElement uredjaj)
        {
            _uredjajElementi[id] = uredjaj;
        }

        public void Detach(int id)
        {
            _uredjajElementi.Remove(id);
        }

        public void Accept(IUredjajVisitor visitor)
        {
            foreach (var uredjaj in _uredjajElementi.Values)
            {
                uredjaj.Accept(visitor);
            }
        }
    }
}
