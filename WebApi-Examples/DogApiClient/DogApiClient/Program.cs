using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DogApiClient();

            foreach(string breed in client.GetBreedsListAsync().Result)
            {
                Console.WriteLine(breed);
            }

            Console.ReadLine();
        }
    }
}
