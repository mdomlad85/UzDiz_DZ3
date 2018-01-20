using System;
using System.Text;

namespace Tof.Logger
{
    class TofLogger : ILogger
    {

        private StringBuilder _logiranePoruke = new StringBuilder();

        private VrstaLogZapisa _minNivo = VrstaLogZapisa.INFO;

        private object syncLock = new object();

        public VrstaLogZapisa MinimalniNivo
        {
            get
            {
                lock (syncLock)
                {
                    return _minNivo;
                }
            }

            set
            {
                lock (syncLock)
                {
                    _minNivo = value;
                }
            }
        }

        public StringBuilder PovijestLogiranja
        {
            get
            {
                lock (syncLock)
                {
                    return _logiranePoruke;
                }
            }
        }

        public void Log(string message)
        {
            lock (syncLock)
            {
                Log(message, VrstaLogZapisa.INFO);
            }
        }

        public void Log(string message, VrstaLogZapisa vrsta)
        {
            lock (syncLock)
            {
                var strVrsta = "INFO";

                if (vrsta < MinimalniNivo)
                {
                    return;
                }

                switch (vrsta)
                {
                    case VrstaLogZapisa.DEBUG:
                        strVrsta = "DEBUG";
                        break;
                    case VrstaLogZapisa.WARNING:
                        strVrsta = "WARNING";
                        break;
                    case VrstaLogZapisa.ERROR:
                        strVrsta = "ERROR";
                        break;
                }
                var msg = string.Format("[{0}]: {1}", strVrsta, message, DateTime.Now.ToString("g"));
                _logiranePoruke.AppendLine(msg);
                Console.WriteLine(msg);
            }
        }
    }
}
