using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingShared
{
    public class FileService : IFileService
    {
        public Stream getFile(string filename)
        {
            Console.WriteLine("Requested file: " + filename);
            return new FileStream(filename, FileMode.Open);
        }
    }
}
