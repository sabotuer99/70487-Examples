using DogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DogApi.Controllers
{
    public class DogsController : ApiController
    {
        private List<Dog> _repo = new List<Dog>();
        public DogsController()
        {
            _repo.Add(new Dog()
            {
                toys = new List<Toy> { new Toy("bone"), new Toy("ball") },
                owner = new Owner() { name = "Bob" },
                name = "Rover",
                friends = new List<Dog>()
            });
            _repo.Add(new Dog()
            {
                toys = new List<Toy> { new Toy("chew toy"), new Toy("shoe") },
                owner = new Owner() { name = "Bob" },
                name = "Fido",
                friends = new List<Dog>()
            });
            _repo.Add(new Dog()
            {
                toys = new List<Toy> { new Toy("squeaky toy"), new Toy("stuffy"), new Toy("bone") },
                owner = new Owner() { name = "Jane" },
                name = "Fifi",
                friends = new List<Dog>()
            });
            _repo.Add(new Dog()
            {
                toys = new List<Toy> { new Toy("chair leg"), new Toy("socks"), new Toy("shoe") },
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
        public IEnumerable<Dog> Get()
        {
            return _repo;
        }

        // GET: api/Dogs/5
        public HttpResponseMessage Get(int id)
        {
            if(id < _repo.Count)
            {
                var dog = _repo[id];
                return Request.CreateResponse(HttpStatusCode.OK, dog);
            }
                
            return Request.CreateResponse(HttpStatusCode.NotFound);
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
