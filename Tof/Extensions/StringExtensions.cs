using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tof
{
    public static class StringExtensions
    {
        public static string[] ToArray(this StringBuilder sb)
        {
            var lines =  sb.ToString().Split(Environment.NewLine.ToCharArray());
            var retVal = new HashSet<string>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    retVal.Add(line);
                }
            }
            return retVal.ToArray();
        }
    }
}
