using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tof.Uzorci.Composite
{
    /// <summary>
    /// The 'Component' abstract class
    /// </summary>
    public abstract class UredjajComponent
    {
        protected Model.Uredjaj uredjaj;

        public Model.Uredjaj Uredjaj => uredjaj;

        public UredjajComponent(Model.Uredjaj uredjaj)
        {
            this.uredjaj = uredjaj;
        }

        public virtual void Remove(UredjajComponent c)
        {
            Singleton.AplikacijskiPomagac.Instanca.Logger.Log("Ne može se ukljanjati iz traženog mjesta!");
        }
        public virtual void Add(UredjajComponent c)
        {
            Singleton.AplikacijskiPomagac.Instanca.Logger.Log("Ne može se dodavati iz traženog mjesta!");
        }
        public virtual void AddRange(ICollection<UredjajComponent> components)
        {
            Singleton.AplikacijskiPomagac.Instanca.Logger.Log("Ne može se dodavati iz traženog mjesta!");
        }
        public virtual void Display(string uvlaka)
        {
            if (uredjaj.JeIspravan)
            {
                var vrsta = string.Empty;
                switch (uredjaj.Tip)
                {
                    case Tip.VANJSKI:
                        vrsta = "vanjski";
                        break;
                    case Tip.UNUTARNJI:
                        vrsta = "unutarnji";
                        break;
                    case Tip.VANJSKI_I_UNUTARNJI:
                        vrsta = "unutarnji i vanjski";
                        break;
                }

                if (!string.IsNullOrEmpty(vrsta))
                {
                    Singleton.AplikacijskiPomagac.Instanca.Logger.Log(string.Format("{0}Uredjaj {1} vrste {2}", uvlaka, uredjaj.Naziv, vrsta));
                }                 
            }
        }
    }
}
