namespace Tof.Uzorci.Builder
{
    /// <summary>
    /// The 'Director' class
    /// </summary>
    class TofSustavDirector
    {
        private readonly ITofSustavBuilder _tofBuilder;

        private readonly Postavke _ulazniPodaci;

        public TofSustavDirector(ITofSustavBuilder tofBuilder, Postavke postavke)
        {
            _tofBuilder = tofBuilder;
            _ulazniPodaci = postavke;
        }

        public void KreirajTofSustav()
        {
            _tofBuilder.UcitajPostavke(_ulazniPodaci);
            _tofBuilder.UcitajPodatke();
            _tofBuilder.InicijalizirajSustav();
            _tofBuilder.Opremi();
        }

        public TofSustav TofSustav => _tofBuilder.TofSUstav;
    }
}
