using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void InitSeq()
        {
            var repeat = Enumerable.Repeat("A", 5);
            var empty = Enumerable.Empty<String>();

            Console.WriteLine("Repeat: " + PrettyPrint(repeat));
            Console.WriteLine("Empty: " + PrettyPrint(empty));
        }
    }
}
