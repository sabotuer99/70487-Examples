using System.ServiceModel;

namespace ConService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CallService : BaseContract
    {
    }
}
