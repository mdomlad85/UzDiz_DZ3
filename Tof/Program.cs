using CommandLine.Text;
using System;
using Tof.Uzorci.Builder;
using Tof.Pomagaci;
using Tof.Uzorci.Singleton;
using Tof.Vendor;
using Tof.Logger;
using Tof.Uzorci.MVC;

namespace Tof
{
    class Program
    {
        static void Main(string[] args)
        {
            var postavke = new Postavke();
            try
            {
                postavke = FlagParser.GetOptions(args);
                var model = new VT100Model();
                var view = new VT100View();
                model.AddObserver(view);
                var ctrl = new VT100Controller(model, view);
                view.SetController(ctrl);
                ctrl.Inicijaliziraj(postavke);
            }
            catch (Iznimke.NeispravniUlazniArgumenti)
            {
                Console.WriteLine(ANSI_VT100_Konstante.ESC + ANSI_VT100_Konstante.Color.RED + "Došlo je do pogreške u postavkama.");
                var helptext = HelpText.AutoBuild(postavke, (HelpText current) => HelpText.DefaultParsingErrorsHandler(postavke, current));
                Console.WriteLine(helptext.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Pogreška: {0}", ex.Message));
            }
        }
    }
}
