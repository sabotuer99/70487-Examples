using System.ServiceModel;

namespace ConcurrentServices.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Reentrant,
                     UseSynchronizationContext = true)]
    public class PerCall_Reentrant_Service : BaseService
    {
    }
}
