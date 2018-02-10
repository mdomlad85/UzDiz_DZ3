namespace Tof.Model
{
    public class Statistika
    {
        public int UspjesnihCiklusa { get; set; }

        public int NeuspjesnihCiklusa { get; set; }

        public double ProsjecnoTrajanjeCiklusa { get; set; }
        public int BrojZamijenjenihSenzora { get; internal set; }
        public int BrojZamijenjenihAktuatora { get; internal set; }
        public int BrojObradenihSenzora { get; internal set; }
        public int BrojObradenihAktuatora { get; internal set; }
    }
}
