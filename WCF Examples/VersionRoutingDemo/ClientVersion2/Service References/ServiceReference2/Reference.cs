﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientVersion2.ServiceReference2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference2.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="OperationC", ReplyAction="http://tempuri.org/IService/OperationCResponse")]
        string OperationC();
        
        [System.ServiceModel.OperationContractAttribute(Action="OperationC", ReplyAction="http://tempuri.org/IService/OperationCResponse")]
        System.Threading.Tasks.Task<string> OperationCAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="OperationB", ReplyAction="http://tempuri.org/IService/OperationBResponse")]
        string OperationB();
        
        [System.ServiceModel.OperationContractAttribute(Action="OperationB", ReplyAction="http://tempuri.org/IService/OperationBResponse")]
        System.Threading.Tasks.Task<string> OperationBAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : ClientVersion2.ServiceReference2.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<ClientVersion2.ServiceReference2.IService>, ClientVersion2.ServiceReference2.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string OperationC() {
            return base.Channel.OperationC();
        }
        
        public System.Threading.Tasks.Task<string> OperationCAsync() {
            return base.Channel.OperationCAsync();
        }
        
        public string OperationB() {
            return base.Channel.OperationB();
        }
        
        public System.Threading.Tasks.Task<string> OperationBAsync() {
            return base.Channel.OperationBAsync();
        }
    }
}
