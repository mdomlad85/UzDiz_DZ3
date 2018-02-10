namespace Tof
{
    public enum Tip : int
    {
        VANJSKI = 0,
        UNUTARNJI = 1,
        VANJSKI_I_UNUTARNJI = 2
    }

    public enum Vrsta : int
    {
        CJELOBROJNO = 0,
        RAZLOMLJENO_1D = 1,
        RAZLOMLJENO_5D = 2,
        ISTINITOST = 3
    }

    enum TipTestera : int
    {
        SEKVENCIJALNI = 0,
        NASUMICNI = 1,
        HIBRIDNI = 2

    }
    enum ExitCode : int
    {
        Success = 0,
        InvalidLogin = 1,
        InvalidFilename = 2,
        UnknownError = 10
    }

    public enum VrstaRasporeda : int
    {
        RasporedUredjajaPoMjestima = 0,
        RasporedUredjajaPoAktuatorima = 1
    }

    public enum VrstaUredjaja : int
    {
        Aktuator = 1,
        Senzor = 0
    }
}
