using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetService
{
    [ServiceContract]
    public interface ICatService
    {
        [OperationContract]
        string speak();

        [OperationContract]
        Kitten haveKitten();

        [XmlSerializerFormat]
        [OperationContract]
        XKitten haveXKitten();

        [OperationContract]
        SKitten haveSKitten();

        [OperationContract]
        void throwUncaught();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void throwCaught();

        [OperationContract]
        [FaultContract(typeof(KittyPoop))]
        void throwCustom();

        [OperationContract]
        Task<Kitten> taskKitten();

        [OperationContract]
        Kitten syncKitten();
    }

    [ServiceBehavior(IncludeExceptionDetailInFaults = false, 
                     InstanceContextMode = InstanceContextMode.PerSession,
                     UseSynchronizationContext = true,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CatService : ICatService
    {
        public Kitten haveKitten()
        {
            return new Kitten("Maxine", "Calico");
        }

        public XKitten haveXKitten()
        {
            return new XKitten("Sebastian", "Grey");
        }

        public SKitten haveSKitten()
        {
            return new SKitten("Dingus", "Grumpy");
        }

        public string speak()
        {
            return "Meow!";
        }

        public void throwUncaught()
        {
            throw new Exception("OH NOES YOU DIDN'T CATCH ME!!!");
        }

        public void throwCaught()
        {
            var ex = new Exception("Well, I exploded, but at least you wrapped me...");
            throw new FaultException<Exception>(ex, "Reason...");
        }

        public void throwCustom()
        {
            var ex = new KittyPoop("stanky!", "small");
            throw new FaultException<KittyPoop>(ex, "Kitty pooped on the carpet... ");
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

        public async Task<Kitten> taskKitten()
        {
            return await Task.Factory.StartNew(() => {
                int interval = 0;
                interval = Rand(200, 2000);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Sleeping for " + interval + "ms on thread" +
                    Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(interval);
                return new Kitten("AsyncKitty (" + Thread.CurrentThread.ManagedThreadId + ")", 
                    "so lazy, slept for " + interval + "ms");
            }, TaskCreationOptions.LongRunning);
        }

        public Kitten syncKitten()
        {
            int interval = 0;
            interval = Rand(200, 2000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sleeping for " + interval + "ms on thread" + 
                Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(interval);
            
            return new Kitten("SyncKitty (" + Thread.CurrentThread.ManagedThreadId + ")", 
                "so lazy, slept for " + interval + "ms and blocked!");
        }
    }

    [Serializable]
    public class KittyPoop
    {
        public KittyPoop(string smell, string size)
        {
            this.smell = smell;
            this.size = size;
        }

        public string smell;
        public string size;
    }
}
