using System.Text;

namespace Tof.Logger
{
    public enum VrstaLogZapisa
    {

        INFO = 1,
        DEBUG = 2,
        WARNING = 3,
        ERROR = 4
    }
    public interface ILogger
    {
        void Log(string message);

        void Log(string message, VrstaLogZapisa vrsta);

        void Ocisti();

        StringBuilder PovijestLogiranja { get; }

        VrstaLogZapisa MinimalniNivo { get; set; }
    }
}
