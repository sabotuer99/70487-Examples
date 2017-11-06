using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DogApi.Models
{
    public class Dog
    {
        public string name;
        public Owner owner;
        public List<Toy> toys;
        public List<Dog> friends;
    }
}