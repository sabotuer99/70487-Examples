using System.ServiceModel;

namespace ConcurrentServices.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Single,
                     UseSynchronizationContext = true)]
    public class PerSession_Single_Service : BaseService
    {
    }
}
