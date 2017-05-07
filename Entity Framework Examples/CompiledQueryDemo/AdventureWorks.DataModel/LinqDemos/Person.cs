using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    public class Person
    {
        public Person(string name, DateTime bday)
        {
            Name = name;
            Birthday = bday;
        }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }

        public override string ToString()
        {
            return Name + ": " + Birthday.ToShortDateString();
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Person p = (Person)obj;
            return (Name.Equals(p.Name)) && (Birthday.Equals(p.Birthday));
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Birthday.GetHashCode();
        }
    }
}
