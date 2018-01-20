using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Tof
{
    public static class StringExtensions
    {
        public static int BrojLinija(this StringBuilder sb)
        {
            return sb.ToString().Split(Environment.NewLine.ToCharArray()).Length;
        }
    }
}
