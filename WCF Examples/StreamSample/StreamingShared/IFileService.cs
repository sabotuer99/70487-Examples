using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StreamingShared
{

    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        Stream getFile(string filename);
    }
}
