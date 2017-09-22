using System.ServiceModel;

namespace ConcurrentServices.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Reentrant,
                     UseSynchronizationContext = true)]
    public class Singleton_Reentrant_Service : BaseService
    {
    }
}
