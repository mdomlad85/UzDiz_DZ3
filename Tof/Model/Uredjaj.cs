using System;
using System.Collections.Generic;
using Tof.Iznimke;
using Tof.Logger;
using Tof.Uzorci.Singleton;
using Tof.Uzorci.Visitor;

namespace Tof.Model
{
    public class Uredjaj : UredjajElement
    {
        public string Naziv { get; set; }

        public Tip Tip { get; set; }

        public Vrsta Vrsta { get; set; }

        public string Komentar { get; set; }
        public List<Uredjaj> PovezaniUredjaji = new List<Uredjaj>();

        public Uredjaj()
        {
        }

        public Uredjaj(string[] attrs)
        {
            try
            {
                Naziv = attrs[0];
                Tip = (Tip)int.Parse(attrs[1]);
                Vrsta = (Vrsta)int.Parse(attrs[2]);
                Min = double.Parse(attrs[3].Replace('.', ','));
                Max = double.Parse(attrs[4].Replace('.', ','));
                Komentar = attrs[5];
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

        /// <summary>
        /// Ako je vrijednost istinita onda ide od max prema min, vrijedi i obrat tvrdnje
        /// </summary>
        public bool Obrnuto => TrenutnaVrijednost == Max;

        private int _neispravanZaRedom;

        private bool _jeIspravan = true;
        private bool _dosloDoPromjene;

        public bool JeIspravan => _jeIspravan;

        public int ID { get; internal set; }

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
                    return 0;
                }
            }
            return 0;
        }

        public virtual void OdradiPosao()
        {
            _dosloDoPromjene = Math.Ceiling((new Random()).NextDouble() * 100) <= 90;
            AplikacijskiPomagac.Instanca.Logger.Log(string.Format("Vrijednost {0} je {1} {2}", Naziv, TrenutnaVrijednost, Komentar));
        }

        public override void Accept(IUredjajVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
