using System.ServiceModel;

namespace ConcurrentServices.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = true)]
    public class Singleton_Multi_Service : BaseService
    {
    }
}
