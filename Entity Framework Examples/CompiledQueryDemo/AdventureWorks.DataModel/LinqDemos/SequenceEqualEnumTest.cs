using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        class Widget
        {
            public Widget(int prop)
            {
                Prop = prop;
            }

            private int _prop;
            public int Prop { 
                get 
                {
                    Console.WriteLine("Getting Prop: " + _prop);
                    return _prop;
                } 
                set { _prop = value; } }

            public override bool Equals(object obj)
            {
                // Check for null values and compare run-time types.
                if (obj == null || GetType() != obj.GetType())
                    return false;

                var p = (Widget)obj;
                return p.Prop == Prop;
            }

            public override int GetHashCode()
            {
                return Prop.GetHashCode();
            }
        }

        public static void TestSequenceEqualEnumeration(){
            var seq1 = new List<Widget>(){
                new Widget(1),
                new Widget(2),
                new Widget(3)
            };
            var seq2 = new List<Widget>(){
                new Widget(1),
                new Widget(3),
                new Widget(2)
            };

            bool eq = seq1.SequenceEqual(seq2);

            Console.WriteLine("Test result: " + eq);
        }
    }
}
