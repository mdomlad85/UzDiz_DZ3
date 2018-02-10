using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tof
{
    public static class ANSI_VT100_Konstante
    {
        public const string ESC = "\x1B[";

        public static class Color
        {
            public const string GIGI = "30m";
            public const string RED = "31m";
            public const string GREEN = "32m";
            public const string YELLOW = "33m";
            public const string BLUE = "34m";
            public const string MAGENTA = "35m";
            public const string CYAN = "36m";
            public const string WHITE = "37m";
        }

        public static class Erase
        {
            public const string ENTIRE_DISPLAY = "2J";
        }
    }
}
