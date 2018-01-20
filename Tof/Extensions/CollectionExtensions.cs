using System.Collections.Generic;
using Tof.Uzorci.Iterator;
using Tof.Iznimke;
using Tof.Model;
using Tof.Uzorci.Singleton;

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
        public static Uredjaj Zamijeni(this KolekcijaUredjaja uredjaji, Uredjaj pokvarenUredjaj, KolekcijaUredjaja zamjenskiUredjaji)
        {
            var pokvareniIndex = uredjaji.IndexOd(pokvarenUredjaj);
            if (pokvareniIndex != -1)
            {
                var max = zamjenskiUredjaji.DohvatiPoTipu(pokvarenUredjaj.Tip).Count - 1;

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
    }
}
