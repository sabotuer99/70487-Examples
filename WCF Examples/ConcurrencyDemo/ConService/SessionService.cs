using System.ServiceModel;

namespace ConService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SessionService : BaseContract
    {
    }
}
