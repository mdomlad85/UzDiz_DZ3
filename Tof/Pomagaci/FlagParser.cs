﻿using CommandLine;
using System;

namespace Tof.Pomagaci
{
    internal static class FlagParser
    {
        internal static Postavke GetOptions(string[] args)
        {
            var options = new Postavke();
            Action<ParserSettings> parserAction = ParserAction;
            Parser parser = new Parser(parserAction);
            parser.ParseArgumentsStrict(args.ConvertArgsFlags(), options);

            if (!options.JesuPostavkeIspravne())
            {
                throw new Iznimke.NeispravniUlazniArgumenti();
            }
            return options;
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
