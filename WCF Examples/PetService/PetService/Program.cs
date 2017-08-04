using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;

namespace PetService
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ServiceHost(typeof(CatService));
            var dogservice = new ServiceHost(typeof(DogService));

            //service.AddServiceEndpoint(typeof(ICatService), new WSHttpBinding());
            service.AddServiceEndpoint(typeof(ICatService), new WSHttpBinding(), "http://localhost:8081/pets/wsCat");
            dogservice.AddServiceEndpoint(typeof(DogService), new BasicHttpBinding(), "http://localhost:8081/pets/dog");

            // Enable metadata exchange.  
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.HttpGetUrl = new Uri("http://localhost:8081/pets/mex");
            service.Description.Behaviors.Add(smb);

            ServiceMetadataBehavior smb1 = new ServiceMetadataBehavior();
            smb1.HttpGetEnabled = true;
            smb1.HttpGetUrl = new Uri("http://localhost:8081/dogs/mex");
            dogservice.Description.Behaviors.Add(smb1);

            service.Open();
            dogservice.Open();
            Console.WriteLine("The service is ready.");
            Console.WriteLine("Press <ENTER> to terminate service.");
            Console.WriteLine();
            Console.ReadLine();

            service.Close();


        }
    }
}
