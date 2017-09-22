using System.ServiceModel;

namespace ConcurrentServices.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = true)]
    public class PerCall_Multi_Service : BaseService
    {
    }
}
