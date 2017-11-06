using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DogApi.Models
{
    public class Toy
    {
        public string name;
        public Toy(string name)
        {
            this.name = name;
        }
    }
}