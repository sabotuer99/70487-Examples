using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace DogApi.Models
{
    public class DogMediaFormatter : BufferedMediaTypeFormatter
    {
        public DogMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/Dog"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == typeof(Dog))
            {
                return true;
            }
            else
            {
                Type enumerableType = typeof(IEnumerable<Dog>);
                return enumerableType.IsAssignableFrom(type);
            }
        }

        public override void WriteToStream(Type type, object value, 
            Stream writeStream, HttpContent content)
        {
            using (var writer = new StreamWriter(writeStream))
            {
                var dogs = value as IEnumerable<Dog>;
                if (dogs != null)
                {
                    foreach (var dog in dogs)
                    {
                        WriteItem(dog, writer);
                    }
                }
                else
                {
                    var singleProduct = value as Dog;
                    if (singleProduct == null)
                    {
                        throw new InvalidOperationException("Cannot serialize type");
                    }
                    WriteItem(singleProduct, writer);
                }
            }
        }

        private void WriteItem(Dog dog, StreamWriter writer)
        {
            writer.WriteLine("[Dog] name = {0}, owner = {1}, toy count = {2}, friend count = {3}", 
                dog.name,
                dog.owner.name,
                dog.toys.Count,
                dog.friends.Count);
        }
    }
}