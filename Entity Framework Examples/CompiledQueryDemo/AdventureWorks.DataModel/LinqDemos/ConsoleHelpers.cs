using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        private static string Cell(bool p)
        {
            return (p.ToString().PadRight(7)).PadLeft(10);
        }

        private static string PrettyPrint<T>(IEnumerable<T> sequence)
        {
            return PrettyPrint(sequence, false);
        }

        private static string PrettyPrint<T>(IEnumerable<T> sequence, bool newlines)
        {

            string newline = "";
            string indent = "";
            if (newlines)
            {
                newline = "\n";
                indent = "    ";
            }
            string seperator = ", " + newline;

            return "[" + newline + string.Join(seperator, sequence.Select(x => indent + x.ToString()).ToArray()) + newline + "]";
        }
    }
}
