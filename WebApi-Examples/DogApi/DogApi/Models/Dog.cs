using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DogApi.Models
{
    public class Dog
    {
        public string name;
        public Owner owner = new Owner();
        public List<Toy> toys = new List<Toy>();
        public List<Dog> friends = new List<Dog>();
    }
}