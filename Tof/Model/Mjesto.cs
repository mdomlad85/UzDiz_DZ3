using System;
using System.Collections.Generic;
using Tof.Iznimke;
using Tof.Uzorci.Singleton;

namespace Tof.Model
{
    [Serializable]
    public class Mjesto : RootElementPrototype
    {
        public Uredjaj[] Senzori { get; set; }
        public Uredjaj[] Aktuatori { get; set; }

        public Mjesto() : base("Tof.Model.Mjesto") { }

        public Mjesto(string[] attrs) : base("Tof.Model.Mjesto")
        {
            try
            {
                ID = int.Parse(attrs[0]);
                Naziv = attrs[1];
                Tip = (Tip)int.Parse(attrs[2]);
                Senzori = new Uredjaj[int.Parse(attrs[3])];
                Aktuatori = new Uredjaj[int.Parse(attrs[4])];
            }
            catch
            {
                throw new LosRedakIzDatoteke();
            }
        }

        public override RootElementPrototype Clone()
        {
            return (Mjesto)this.MemberwiseClone();
        }
    }
}
