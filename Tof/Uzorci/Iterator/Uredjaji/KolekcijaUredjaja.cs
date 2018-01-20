using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tof.Model;
using Tof.Pomagaci;

namespace Tof.Uzorci.Iterator
{
    /// <summary>
    /// Konkretna implementacija 'ConcreteAggregate'
    /// </summary>
    public class KolekcijaUredjaja : ApstraktnaKolekcija<Uredjaj>
    {
        private ArrayList _uredjaji = new ArrayList();

        private GeneratorIdeva _idGenerator = new GeneratorIdeva();
        private int _maxCount = -1;

        public KolekcijaUredjaja(int maxCount)
        {
            _maxCount = maxCount;
        }

        public KolekcijaUredjaja()
        {

        }

        public override ApstraktniIterator<Uredjaj> CreateIterator()
        {
            return new IteratorUredjaja(this);
        }

        public int Count
        {
            get { return _uredjaji.Count; }
        }

        public int MaxCount
        {
            get { return _maxCount; }
        }

        public Uredjaj this[int index]
        {
            get { return (Uredjaj)_uredjaji[index]; }
            set
            {
                value.ID = _idGenerator.DajSljedeciId();
                _uredjaji.Insert(index, value);
            }
        }

        public List<Uredjaj> DohvatiPoTipu(Tip tip)
        {
            return _uredjaji.OfType<Uredjaj>().ToList()
                .FindAll(x => x.Tip == tip || x.Tip == Tip.VANJSKI_I_UNUTARNJI)
                .ToList();
        }

        internal int IndexOd(Uredjaj pokvarenUredjaj)
        {
            return _uredjaji.OfType<Uredjaj>().ToList().IndexOf(pokvarenUredjaj);
        }

        internal void PrekopirajPoljeSNovim(List<Uredjaj> uredjaji)
        {
            _uredjaji = new ArrayList(uredjaji);
        }

        internal List<Uredjaj> DohvatiIspravne()
        {
            return _uredjaji.OfType<Uredjaj>().ToList().Where(x => x.JeIspravan).ToList();
        }
    }
}
