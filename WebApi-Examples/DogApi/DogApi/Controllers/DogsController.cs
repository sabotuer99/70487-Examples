using DogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DogApi.Controllers
{
    public class DogsController : ApiController
    {
        private static List<Dog> _repo = new List<Dog>();
        static DogsController()
        {
            _repo.Add(new Dog()
            {
                toys = { new Toy("bone"), new Toy("ball") },
                owner = new Owner() { name = "Bob" },
                name = "Rover",
                friends = new List<Dog>()
            });
            _repo.Add(new Dog()
            {
                toys = { new Toy("chew toy"), new Toy("shoe") },
                owner = new Owner() { name = "Bob" },
                name = "Fido",
                friends = new List<Dog>()
            });
            _repo.Add(new Dog()
            {
                toys = { new Toy("squeaky toy"), new Toy("stuffy") },
                owner = new Owner() { name = "Jane" },
                name = "Fifi",
                friends = new List<Dog>()
            });
            _repo.Add(new Dog()
            {
                toys = { new Toy("chair leg"), new Toy("socks") },
                owner = new Owner() { name = "Marge" },
                name = "Muffin",
                friends = new List<Dog>()
            });

            _repo[0].friends.Add(_repo[1]);
            _repo[0].friends.Add(_repo[2]);
            _repo[1].friends.Add(_repo[0]);
            _repo[1].friends.Add(_repo[3]);
            _repo[2].friends.Add(_repo[0]);
            _repo[2].friends.Add(_repo[3]);
            _repo[3].friends.Add(_repo[1]);
            _repo[3].friends.Add(_repo[2]);
        }

        public string GetSomethingElse(string a, string b)
        {
            return "You passed in " + a + " and " + b;
        }

        // GET: api/Dogs
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Dogs/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Dogs
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Dogs/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Dogs/5
        public void Delete(int id)
        {
        }
    }


}
