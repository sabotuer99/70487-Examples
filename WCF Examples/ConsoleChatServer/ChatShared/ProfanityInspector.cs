using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ChatShared
{
    class ProfanityInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var replacements = new Dictionary<string, string>();
            replacements.Add("fuck", "floob");
            replacements.Add("god", "glob");
            replacements.Add("ass", "butt");
            replacements.Add("fart", "poot");
            replacements.Add("bitch", "dingus");
            replacements.Add("shit", "poo");

            request = ChangeString(request, replacements);
            Console.WriteLine("Inspector Called...");

            //no need for correlationState, so return null
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        public Message ChangeString(Message oldMessage, Dictionary<string,string> replacements)
        {
            Message newMessage = null;
            //load the old message into XML
            MessageBuffer msgbuf = oldMessage.CreateBufferedCopy(int.MaxValue);
            Message tmpMessage = msgbuf.CreateMessage();
            XmlDictionaryReader xdr = tmpMessage.GetReaderAtBodyContents();
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xdr);
            xdr.Close();


            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("a", "http://failedturing.com/chatdemo");
            nsmgr.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");

            XmlNode node = xdoc.SelectSingleNode("//a:Message", nsmgr);
        
            if (node != null) {
                //node.InnerText = "[Modified in SimpleMessageInspector]" + node.InnerText;
                string body = node.InnerText;

                foreach (string from in replacements.Keys)
                {
                    string to = replacements[from];
                    body = Regex.Replace(body, from, to, RegexOptions.IgnoreCase);
                }

                node.InnerText = body;
            }

            MemoryStream ms = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(ms);
            xdoc.Save(xw);
            xw.Flush();
            xw.Close();
            ms.Position = 0;
            XmlReader xr = XmlReader.Create(ms);


            newMessage = Message.CreateMessage(oldMessage.Version, null, xr);
            newMessage.Headers.CopyHeadersFrom(oldMessage);
            newMessage.Properties.CopyProperties(oldMessage.Properties);
            return newMessage;
        }
    }

    public class ProfanityInterceptorBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            
        }

        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/extending/how-to-inspect-and-modify-messages-on-the-service
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher chDisp in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher epDisp in chDisp.Endpoints)
                {
                    epDisp.DispatchRuntime.MessageInspectors.Add(new ProfanityInspector());
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            
        }
    }

    public class ProfanityInterceptorBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(ProfanityInterceptorBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new ProfanityInterceptorBehavior();
        }
    }
}
