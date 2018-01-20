using Tof.Model;

namespace Tof.Uzorci.Iterator
{
    public class IteratorMjesta : ApstraktniIterator<Mjesto>
    {
        private KolekcijaMjesta _kolekcija;
        private int _trenutni = 0;
        private int _korak = 1;
        
        public IteratorMjesta(KolekcijaMjesta kolekcija)
        {
            _kolekcija = kolekcija;
        }
        
        public override Mjesto First()
        {
            _trenutni = 0;
            return _kolekcija[_trenutni] as Mjesto;
        }
        
        public override Mjesto Next()
        {
            _trenutni += _korak;
            if (!IsDone)
                return _kolekcija[_trenutni] as Mjesto;
            else

                return null;
        }
        
        public int Step
        {
            get { return _korak; }
            set { _korak = value; }
        }
        
        public override Mjesto CurrentItem
        {
            get { return _kolekcija[_trenutni] as Mjesto; }
        }
        
        public override bool IsDone
        {
            get { return _trenutni >= _kolekcija.Count; }
        }
    }
}