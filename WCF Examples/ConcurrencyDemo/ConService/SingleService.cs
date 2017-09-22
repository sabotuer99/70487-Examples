using System.ServiceModel;

namespace ConService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SingleService : BaseContract
    {
    }
}
