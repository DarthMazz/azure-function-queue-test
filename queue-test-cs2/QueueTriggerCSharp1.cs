using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;
using System.Text;

namespace Company.Function
{
    public static class QueueTriggerCSharp1
    {
        [FunctionName("QueueTriggerCSharp1")]
        public static async void Run([QueueTrigger("myqueue-items2", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            //2020-07-11T09:59:46.000Z,20200711185959
            Encoding enc = Encoding.GetEncoding("UTF-8");
            string base64String = Convert.ToBase64String(enc.GetBytes(myQueueItem));
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            QueueClient queueClient = new QueueClient(connectionString, "myqueue-items");
            await queueClient.CreateAsync();
            await queueClient.SendMessageAsync(base64String);
        }
    }
}

