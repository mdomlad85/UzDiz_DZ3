using System;
using System.Collections.Generic;
using Tof.Iznimke;
using Tof.Logger;
using Tof.Uzorci.Singleton;

namespace Tof.Model
{
    [Serializable]
    public class Uredjaj : RootElementPrototype
    {
        public Vrsta Vrsta { get; set; }

        public string Komentar { get; set; }
        public List<Uredjaj> PovezaniUredjaji = new List<Uredjaj>();

        public Uredjaj() : base("Tof.Model.Uredjaj")
        {
        }

        public Uredjaj(string[] attrs) : base("Tof.Model.Uredjaj")
        {
            try
            {
                ID = int.Parse(attrs[0]);
                Naziv = attrs[1];
                Tip = (Tip)int.Parse(attrs[2]);
                Vrsta = (Vrsta)int.Parse(attrs[3]);
                Min = double.Parse(attrs[4].Replace('.', ','));
                Max = double.Parse(attrs[5].Replace('.', ','));
                Komentar = attrs[6];
                TrenutnaVrijednost = Min;
            }
            catch
            {
                throw new LosRedakIzDatoteke();
            }
        }

        public bool DosloDoPromjene => _dosloDoPromjene;

        public void Inicijaliziraj(string poruka)
        {
            AplikacijskiPomagac.Instanca.Logger.Log(poruka, VrstaLogZapisa.INFO);
            JeZdrav(1);
        }

        /// <summary>
        /// Ovisno o vrsti polje će zaprimiti takvu vrijednost (cjelobrojno, boolean, ...) 
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Ovisno o vrsti polje će zaprimiti takvu vrijednost (cjelobrojno, boolean, ...) 
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Ovisno o izvršavanju akcije aktuator će poprimiti vrijednost prema 
        /// pravilima izvršavanja dok će se senzoru automatski pridjeliti
        /// </summary>
        public double TrenutnaVrijednost { get; set; }

        public int ExternalID { get; set; }

        public int MjestoID { get; set; }

        /// <summary>
        /// Ako je vrijednost istinita onda ide od max prema min, vrijedi i obrat tvrdnje
        /// </summary>
        public bool Obrnuto
        {
            get
            {
                if(TrenutnaVrijednost == Max) {
                    _obrnuto = true;
                }
                if(TrenutnaVrijednost == Min)
                {
                    _obrnuto = false;
                }
                return _obrnuto;
            }
        }

        private int _neispravanZaRedom;

        private bool _jeIspravan = true;
        private bool _dosloDoPromjene;
        private bool _obrnuto;

        public bool JeIspravan => _jeIspravan;

        public int JeZdrav(int maxNezdrav = 3)
        {
            if (_jeIspravan)
            {
                if (Math.Ceiling((new Random()).NextDouble() * 100) <= 90)
                {
                    _neispravanZaRedom = 0;
                    return 1;
                }
                else
                {
                    _neispravanZaRedom++;
                    if (_neispravanZaRedom == maxNezdrav)
                    {
                        _jeIspravan = false;
                    }
                    return _jeIspravan ? 1 : 0;
                }
            }
            return 0;
        }

        public virtual void OdradiPosao()
        {
            _dosloDoPromjene = Math.Ceiling((new Random()).NextDouble() * 100) <= 90;
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Vrijednost {0} je {1} {2} (ID: {3})", Naziv, TrenutnaVrijednost, Komentar, ExternalID));
        }

        public override RootElementPrototype Clone()
        {
            return (Uredjaj)this.MemberwiseClone();
        }
    }
}
