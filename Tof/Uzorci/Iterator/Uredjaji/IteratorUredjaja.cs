using Tof.Model;

namespace Tof.Uzorci.Iterator
{
    public class IteratorUredjaja : ApstraktniIterator<Uredjaj>
    {
        private KolekcijaUredjaja _kolekcija;
        private int _trenutni = 0;
        private int _korak = 1;
        
        public IteratorUredjaja(KolekcijaUredjaja kolekcija)
        {
            _kolekcija = kolekcija;
        }
        
        public override Uredjaj First()
        {
            _trenutni = 0;
            return _kolekcija[_trenutni] as Uredjaj;
        }

        public override Uredjaj Next()
        {
            _trenutni += _korak;
            if (!IsDone)
                return _kolekcija[_trenutni] as Uredjaj;
            else

                return null;
        }
        
        public int Step
        {
            get { return _korak; }
            set { _korak = value; }
        }

        public override Uredjaj CurrentItem
        {
            get { return _kolekcija[_trenutni] as Uredjaj; }
        }

        public override bool IsDone
        {
            get { return _trenutni >= _kolekcija.Count; }
        }
    }
}