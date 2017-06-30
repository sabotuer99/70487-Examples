using System;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.Azure;  // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureStorageDemo
{
    public class PictureEntity : TableEntity
    {
        public PictureEntity(string name)
        {
            PartitionKey = name;
            RowKey = DateTime.Now.Ticks.ToString();
        }

        public PictureEntity() { }

        public string BlobUri { get; set; }
        public long FileSize { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {


            var blob = RunBlobCode();
            var picture = RunTableCode(blob);
            RunQueueCode(picture);


            Console.WriteLine("Press ENTER to exit...");
            Console.Read();
        }

        private static void RunQueueCode(PictureEntity picture)
        {
            Console.WriteLine("\n\nStarting Queue Demo...");
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Get or create the queue
            CloudQueue queue = queueClient.GetQueueReference("notifications");
            queue.CreateIfNotExists();

            // Peek the queue
            CloudQueueMessage peekedMessage = queue.PeekMessage();
            Console.WriteLine("Top of queue before new message: " + (peekedMessage == null ? "" : peekedMessage.AsString));

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage("Created picture metadata:\n" + 
                "    Partition Key :" + picture.PartitionKey + "\n" + 
                "    Row Key: " + picture.RowKey + "\n");
            queue.AddMessage(message);
            Console.WriteLine("Added message to queue: " + message.AsString);
        }

        private static PictureEntity RunTableCode(CloudBlockBlob blob)
        {
            Console.WriteLine("\n\nStarting Table Demo...");
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Get or create the table
            CloudTable table = tableClient.GetTableReference("pictures");
            table.CreateIfNotExists();

            PictureEntity picture = new PictureEntity(blob.Name);
            picture.FileSize = blob.Properties.Length;
            picture.BlobUri = blob.Uri.ToString();

            TableOperation insert = TableOperation.Insert(picture);

            table.Execute(insert);

            Console.WriteLine("'Picture' TableEntity properties: ");
            foreach (var prop in picture.GetType().GetProperties())
            {
                Console.WriteLine("   " + prop.Name + ": " + prop.GetValue(picture, null));
            }

            Console.WriteLine("\nWrote metadata entity into " + table.Name + " table.");
            TableQuery<PictureEntity> query = new TableQuery<PictureEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, picture.PartitionKey));


            Console.WriteLine("Entries for this picture: " + table.ExecuteQuery(query).Count());

            return picture;
        }

        private static CloudBlockBlob RunBlobCode()
        {
            Console.WriteLine("\n\nStarting Blob Demo...");
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get or create container
            CloudBlobContainer container = blobClient.GetContainerReference("failedturing");
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("trollface");

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(@"trollface.png"))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            Console.WriteLine("Blob properties: ");
            foreach(var prop in blockBlob.GetType().GetProperties())
            {
                Console.WriteLine("   " + prop.Name + ": " + prop.GetValue(blockBlob,null));
            }
            Console.WriteLine();
            return blockBlob;
        }
    }
}
