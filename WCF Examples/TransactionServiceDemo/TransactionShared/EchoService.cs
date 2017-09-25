using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TransactionShared
{
    public class EchoService : IEchoService
    {
        [OperationBehavior(TransactionScopeRequired = true, 
                           TransactionAutoComplete = false)]
        public string Echo(string message)
        {
            var ti = Transaction.Current.TransactionInformation;
            var id = ti.DistributedIdentifier.ToString();

            //If "TransactionAutoComplete = true", this line isn't necessary
            OperationContext.Current.SetTransactionComplete();
            
            return "Id(" + id + "), Status(" + ti.Status + "): " + message;
        }
    }
}
