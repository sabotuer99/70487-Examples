using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    class HeroType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public HeroType(int id, string typeName)
        {
            Id = id;
            TypeName = typeName;
        }

        public override string ToString()
        {
            return TypeName + " (Id: " + Id + ")";
        }
    }
}
