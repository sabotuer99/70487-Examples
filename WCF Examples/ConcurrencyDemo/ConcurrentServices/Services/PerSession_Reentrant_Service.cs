using System.ServiceModel;

namespace ConcurrentServices.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Reentrant,
                     UseSynchronizationContext = true)]
    public class PerSession_Reentrant_Service : BaseService
    {
    }
}
