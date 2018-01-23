using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Batch;

namespace BatchTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            //create the requests to be batched
            string baseUrl = "http://localhost:34245/api/Dogs/";
            var getDog1 = new HttpRequestMessage(HttpMethod.Get, baseUrl + "1");
            var getDog3 = new HttpRequestMessage(HttpMethod.Get, baseUrl + "3");
            var getGizmo = new HttpRequestMessage(HttpMethod.Get, baseUrl + "Gizmo");
            getGizmo.Headers.Add("Accept", @"application/Dog");

            //create the batch container
            MultipartContent content = 
                new MultipartContent("mixed", "batch_" + Guid.NewGuid().ToString());
            content.Add(new HttpMessageContent(getDog1));
            content.Add(new HttpMessageContent(getDog3));
            content.Add(new HttpMessageContent(getGizmo));

            //create the batch request
            HttpRequestMessage batchRequest = 
                new HttpRequestMessage(HttpMethod.Post, "http://localhost:34245/api/batch");
            batchRequest.Content = content;

            //send the batch request with the client and parse the response
            HttpResponseMessage response = client.SendAsync(batchRequest).Result;
            MultipartMemoryStreamProvider responseContents = 
                response.Content.ReadAsMultipartAsync().Result;

            foreach(var c in responseContents.Contents)
            {
                Console.WriteLine(c.ReadAsStringAsync().Result + "\n\n");
            }

            Console.ReadLine();
        }
    }
}
