using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentServices
{
    public class BaseService : IService
    {
        public void Process(int id)
        {
            ICallback client = OperationContext.Current.GetCallbackChannel<ICallback>();
            try
            {
                client.NotifyBegin(id);
                Thread.Sleep(Rand(2000, 6000));
                client.NotifyEnd(id);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        readonly ThreadLocal<Random> random =
              new ThreadLocal<Random>(() => new Random(GetSeed()));

        int Rand(int min, int max)
        {
            return random.Value.Next(min, max);
        }

        static int GetSeed()
        {
            return Environment.TickCount * Thread.CurrentThread.ManagedThreadId;
        }
    }
}
