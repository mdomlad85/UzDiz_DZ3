using System.IO;
using System.Linq;

namespace Tof
{
    public static class FileExtensions
    {
        public static string[] ReadAllLinesExceptFirstN(this string filename, int N = 1)
        {
            var lines = File.ReadAllLines(filename);
            if (lines.Count() >= N)
            {
                lines = lines.Skip(N).ToArray();
            }
            return lines;
        }
    }
}
