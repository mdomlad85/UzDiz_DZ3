using System;
using System.IO;
using Tof.Logger;
using Tof.Model;
using Tof.Nasumicnjak;

namespace Tof.Uzorci.Singleton
{
    public class AplikacijskiPomagac
    {

        private ILogger _logger;

        private INasumicnjak _nasumicnjak;

        private Statistika _statistika;

        public ILogger Logger => _logger;

        public INasumicnjak Nasumicnjak => _nasumicnjak;

        public Statistika Statistika => _statistika;

        public void PostaviLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void PostaviNasumicnjak(INasumicnjak nasumicnjak)
        {
            _nasumicnjak = nasumicnjak;
        }

        public AplikacijskiPomagac()
        {
            _logger = new TofLogger();
            _nasumicnjak = new SistemskiNasumicnjak();
            _statistika = new Statistika();
        }

        public AplikacijskiPomagac(INasumicnjak nasumicnjak)
        {
            _logger = new TofLogger();
            _nasumicnjak = nasumicnjak;

        }

        public AplikacijskiPomagac(ILogger logger)
        {
            _logger = logger;
            _nasumicnjak = new SistemskiNasumicnjak();

        }

        public AplikacijskiPomagac(ILogger logger, INasumicnjak nasumicnjak)
        {
            _logger = logger;
            _nasumicnjak = nasumicnjak;
        }

        private static object staticSyncLock = new object();

        private static AplikacijskiPomagac _instanca;

        public static AplikacijskiPomagac Instanca
        {
            get
            {
                // Dvostruko zaključavanje zbog sigurnog pristupa u višedretvenom pristupu
                if (_instanca == null)
                {
                    lock (staticSyncLock)
                    {
                        if (_instanca == null)
                        {
                            _instanca = new AplikacijskiPomagac();
                        }
                    }
                }

                return _instanca;
            }

        }
    }
}
