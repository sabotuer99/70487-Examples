using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DogApiClient
{
    public class DogApiClient
    {
        private static HttpClient client = new HttpClient();
        private const string baseUrl = "https://dog.ceo/api/";

        public async Task<IEnumerable<string>> GetBreedsListAsync()
        {

            IEnumerable<string> list = null;
            string path = baseUrl + "breeds/list/all";
            HttpResponseMessage response = await client.GetAsync(path);

            var definition = new { status = "", message = new Dictionary<string,List<string>>() };

            if (response.IsSuccessStatusCode)
            {
                string rawJson = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeAnonymousType(rawJson, definition)
                       .message
                       .SelectMany(b => b.Value.DefaultIfEmpty()
                       .Select(s => b.Key + " " + s));
            }
            return list;
        }

        //same method without all the async-iness...
        public IEnumerable<string> GetBreedsList()
        {
            IEnumerable<string> list = null;
            string path = baseUrl + "breeds/list/all";
            HttpResponseMessage response = client.GetAsync(path).Result;

            var definition = new { status = "", message = new Dictionary<string, List<string>>() };

            if (response.IsSuccessStatusCode)
            {
                string rawJson = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeAnonymousType(rawJson, definition)
                       .message
                       .SelectMany(b => b.Value.DefaultIfEmpty()
                       .Select(s => b.Key + " " + s));
            }
            return list;
        }

        //if we wanted to be super lazy we could do it this way...
        public IEnumerable<string> GetBreedsList2()
        {
            return GetBreedsListAsync().Result;
        }
    }
}
