using System.Collections.Generic;
using Tof.Logger;
using Tof.Model;
using Tof.Uzorci.Builder;
using Tof.Uzorci.Memento;

namespace Tof.Uzorci.MVC
{
    public abstract class IModel
    {
        protected ILogger _writer = new TofLogger();

        private TofSustav _tofSustav;

        public List<IView> _observers = new List<IView>();

        public ILogger Logger { get { return _writer; } set { _writer = value; } }

        public virtual void AddObserver(IView view)
        {
            _observers.Add(view);
        }

        public virtual void RemoveObserver(IView view)
        {
            _observers.Remove(view);
        }

        public virtual void Notify()
        {
            foreach (IView view in _observers)
            {
                view.Update(this);
            }
        }

        // Memento polje
        public TofSustav TofState
        {
            get { return _tofSustav; }
            set

            {
                _tofSustav = value;
            }
        }

        // Kreiranje mementa
        public TofMemento CreateMemento()
        {
            return new TofMemento(_tofSustav.Clone());
        }

        // vraćanje starog stanja
        public void SetMemento(TofMemento memento)
        {
            _tofSustav = memento.TofState;
        }

        public abstract void IspisPodatakaMjesta(Mjesto mjesto);

        public abstract void IspisPodatakaSenzora(Uredjaj senzor);

        public abstract void IspisPodatakaAktuatora(Uredjaj aktuator);

        public abstract void IspisStatistike();

        public abstract void IzvrsiNCiklusaDretve(int brojCiklusa);

        public abstract void PostaviPctIspravnostUredjaja(int pct);

        public abstract void Izadji();

        //Provjera u 
        public abstract void VlastitaFunkcionalnost(Uredjaj uredjaj);

        public abstract void IspisiPomoc();

        public virtual void IspisiPogresku(string msg = "")
        {
            _writer.Log(msg, VrstaLogZapisa.ERROR);
            Notify();
        }

        public virtual string[] DohvatiLinije()
        {
            return _writer.PovijestLogiranja.ToArray();
        }
    }
}
