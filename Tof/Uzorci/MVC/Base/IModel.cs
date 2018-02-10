using System.Collections.Generic;
using Tof.Logger;
using Tof.Model;

namespace Tof.Uzorci.MVC
{
    public abstract class IModel
    {
        public ILogger _writer = new TofLogger();

        public List<IView> _observers = new List<IView>();

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

        public abstract void IspisPodatakaMjesta(Mjesto mjesto);

        public abstract void IspisPodatakaSenzora(Uredjaj senzor);

        public abstract void IspisPodatakaAktuatora(Uredjaj aktuator);

        public abstract void IspisStatistike();

        public abstract void SpremiPodatke(Builder.TofSustav sustav);

        public abstract Builder.TofSustav VratiSpremljenePodatke();

        public abstract void IzvrsiNCiklusaDretve(int brojCiklusa, Builder.TofSustav sustav);

        public abstract void PostaviPctIspravnostUredjaja(int pct);

        public abstract void Izadji();

        //TODO: osmisli funkcionalnost
        public abstract void VlastitaFunkcionalnost();

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
