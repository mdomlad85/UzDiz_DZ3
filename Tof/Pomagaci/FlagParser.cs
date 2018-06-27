using CommandLine;
using System;

namespace Tof.Pomagaci
{
    internal static class FlagParser
    {
        internal static Postavke GetOptions(string[] args)
        {
            Action<ParserSettings> parserAction = ParserAction;
            Parser parser = new Parser(parserAction);
            parser.ParseArgumentsStrict(args.ConvertArgsFlags(), Postavke.Instanca);

            if (!Postavke.Instanca.JesuPostavkeIspravne())
            {
                throw new Iznimke.NeispravniUlazniArgumenti();
            }
            Postavke.Instanca.InicijalizirajOpcionalnePostavke();

            return Postavke.Instanca;
        }
        static void ParserAction(ParserSettings settings)
        {
            if (settings == null)
            {
                settings = new ParserSettings(true, true, false, Console.Error);
            }
            else
            {
                settings.HelpWriter = Console.Error;
            }
        }
    }
}
