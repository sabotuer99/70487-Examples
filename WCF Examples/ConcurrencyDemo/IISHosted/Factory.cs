using ConcurrentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web;

namespace IISHosted
{
    public class Factory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = new ServiceHost(serviceType, baseAddresses);

            //Add metadata behavior
            ServiceMetadataBehavior smb1 = new ServiceMetadataBehavior();
            smb1.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb1);

            //Add endpoint
            WSDualHttpBinding binding = new WSDualHttpBinding();
            binding.ReceiveTimeout = TimeSpan.FromMilliseconds(60000);
            binding.SendTimeout = TimeSpan.FromMilliseconds(60000);
            binding.CloseTimeout = TimeSpan.FromMilliseconds(60000);
            binding.OpenTimeout = TimeSpan.FromMilliseconds(60000);
            binding.Security.Mode = WSDualHttpSecurityMode.None;

            string address = "/status";
            host.AddServiceEndpoint(typeof(IService), binding, address);

            return host;
        }
    }
}