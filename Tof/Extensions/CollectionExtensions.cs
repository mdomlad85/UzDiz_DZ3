using System.Collections.Generic;
using Tof.Iznimke;
using Tof.Model;
using Tof.Uzorci.Singleton;
using System.Linq;

namespace Tof
{
    public static class CollectionExtensions
    {
        public static string[] ConvertArgsFlags(this string[] args)
        {
            var kljucevi = new HashSet<string>();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (!kljucevi.Add(args[i]))
                    {
                        throw new DuplaZastavica(args[i]);
                    }
                    if (args[i].LastIndexOf("-") == 0 && args[i].Length > 2)
                    {
                        args[i] = string.Format("-{0}", args[i]);
                    }
                }
            }
            return args;
        }
        public static Uredjaj Zamijeni(this Uredjaj[] uredjaji, Uredjaj pokvarenUredjaj, List<Uredjaj> zamjenskiUredjaji)
        {
            var pokvareniIndex = uredjaji.ToList().IndexOf(pokvarenUredjaj);
            if (pokvareniIndex != -1)
            {
                var max = zamjenskiUredjaji.Where(x => x.Tip == pokvarenUredjaj.Tip).Count() - 1;

                var zamjenskiUredjaj = pokvarenUredjaj;

                do
                {
                    var zamjenskiIndex = AplikacijskiPomagac
                    .Instanca
                    .Nasumicnjak
                    .DajSlucajniBroj(0, max);

                    zamjenskiUredjaj = zamjenskiUredjaji[zamjenskiIndex];
                    uredjaji[pokvareniIndex] = zamjenskiUredjaj;
                } while (pokvarenUredjaj.Naziv == zamjenskiUredjaj.Naziv);

                return zamjenskiUredjaj;
            }
            return null;
        }

        public static bool DodajUredjajNaKraj(this Uredjaj[] uredjaji, Uredjaj uredjajZaDodati)
        {
            for (int i = 0; i < uredjaji.Length; i++)
            {
                if (uredjaji[i] == null)
                {
                    uredjaji[i] = uredjajZaDodati;
                    return true;
                }
            }
            return false;
        }
    }
}
