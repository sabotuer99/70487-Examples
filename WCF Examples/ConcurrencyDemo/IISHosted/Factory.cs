using ConcurrentServices;
using ConcurrentServices.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Web;

namespace IISHosted
{
    public class Factory : ServiceHostFactory
    {

        Dictionary<string, int> map = new Dictionary<string, int>();
        Dictionary<string, Type> types = new Dictionary<string, Type>();

        public Factory()
        {
            map.Add("ConcurrentServices.Services.PerCall_Multi_Service", 12001);
            map.Add("ConcurrentServices.Services.PerCall_Single_Service", 12002);
            map.Add("ConcurrentServices.Services.PerSession_Multi_Service", 12003);
            map.Add("ConcurrentServices.Services.PerSession_Single_Service", 12004);
            map.Add("ConcurrentServices.Services.Singleton_Multi_Service", 12005);
            map.Add("ConcurrentServices.Services.Singleton_Single_Service", 12006);
            map.Add("ConcurrentServices.Services.PerCall_Reentrant_Service", 12007);
            map.Add("ConcurrentServices.Services.PerSession_Reentrant_Service", 12008);
            map.Add("ConcurrentServices.Services.Singleton_Reentrant_Service", 12009);

            types.Add("ConcurrentServices.Services.PerCall_Multi_Service", typeof(PerCall_Multi_Service));
            types.Add("ConcurrentServices.Services.PerCall_Single_Service", typeof(PerCall_Single_Service));
            types.Add("ConcurrentServices.Services.PerSession_Multi_Service", typeof(PerSession_Multi_Service));
            types.Add("ConcurrentServices.Services.PerSession_Single_Service", typeof(PerSession_Single_Service));
            types.Add("ConcurrentServices.Services.Singleton_Multi_Service", typeof(Singleton_Multi_Service));
            types.Add("ConcurrentServices.Services.Singleton_Single_Service", typeof(Singleton_Single_Service));
            types.Add("ConcurrentServices.Services.PerCall_Reentrant_Service", typeof(PerCall_Reentrant_Service));
            types.Add("ConcurrentServices.Services.PerSession_Reentrant_Service", typeof(PerSession_Reentrant_Service));
            types.Add("ConcurrentServices.Services.Singleton_Reentrant_Service", typeof(Singleton_Reentrant_Service));
        }


        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            Trace.WriteLine("Called with constructor string: " + constructorString);

            Type serviceType = types[constructorString];
            return CreateServiceHost(serviceType, baseAddresses);
        }
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Trace.WriteLine("Creating service of type " + serviceType.FullName + "...");
            int port = map[serviceType.FullName];
            Trace.WriteLine("...on port " + port);
            var host = new ServiceHost(serviceType, baseAddresses.Where(b => b.Port == port).FirstOrDefault());

            //Add metadata behavior
            ServiceMetadataBehavior smb1 = new ServiceMetadataBehavior();
            //smb1.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb1);

            //Add endpoint
            NetTcpBinding binding = new NetTcpBinding();
            binding.ReceiveTimeout = TimeSpan.FromMilliseconds(60000);
            binding.SendTimeout = TimeSpan.FromMilliseconds(60000);
            binding.CloseTimeout = TimeSpan.FromMilliseconds(60000);
            binding.OpenTimeout = TimeSpan.FromMilliseconds(60000);
            binding.Security.Mode = SecurityMode.None;

            string address = "status";
            host.AddServiceEndpoint(typeof(IService), binding, address);
            //host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            PrintServiceDescription(host);

            return host;
        }

        private static void PrintServiceDescription(ServiceHost host)
        {
            Trace.WriteLine(string.Format("{0} is up and running with the following endpoints:",
                host.Description.ServiceType.Name));
            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                Trace.WriteLine(string.Format("Address: {0}  ({1})",
                    endpoint.Address.ToString(), endpoint.Binding.Name));
            }

            Trace.WriteLine("Instance Mode:" + host.Description.Behaviors.OfType<ServiceBehaviorAttribute>().FirstOrDefault().InstanceContextMode);
            Trace.WriteLine("Concurrency Mode: " + host.Description.Behaviors.OfType<ServiceBehaviorAttribute>().FirstOrDefault().ConcurrencyMode);
            Trace.WriteLine("---------------------------------------------------------------------------------------");
        }
    }
}