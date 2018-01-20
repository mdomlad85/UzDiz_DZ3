using System.Collections.Generic;
using Tof.Model;
using Tof.Pomagaci;

namespace Tof.Uzorci.Iterator
{
    /// <summary>
    /// Konkretna implementacija 'ConcreteAggregate'
    /// </summary>
    public class KolekcijaMjesta : ApstraktnaKolekcija<Mjesto>
    {
        private List<Mjesto> _mjesta = new List<Mjesto>();

        private object lockObject = new object();

        private GeneratorIdeva _idGenerator = new GeneratorIdeva();

        public override ApstraktniIterator<Mjesto> CreateIterator()
        {
            return new IteratorMjesta(this);
        }

        public int Count
        {
            get { return _mjesta.Count; }
        }

        public Mjesto this[int index]
        {
            get { return _mjesta[index]; }
            set
            {
                value.ID = _idGenerator.DajSljedeciId();
                _mjesta.Add(value);
            }
        }
    }
}
