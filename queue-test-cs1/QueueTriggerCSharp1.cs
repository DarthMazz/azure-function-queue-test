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
        public async static void Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            string dateTimeNowString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

            if (myQueueItem == "start")
            {
                Encoding enc = Encoding.GetEncoding("UTF-8");
                string base64String = Convert.ToBase64String(enc.GetBytes(dateTimeNowString));

                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                QueueClient queueClient = new QueueClient(connectionString, "myqueue-items2");
                await queueClient.CreateAsync();
                await queueClient.SendMessageAsync(base64String);
            }
            else
            {
                DateTime dt1 = DateTime.Parse(dateTimeNowString);
                DateTime dt2 = DateTime.Parse(myQueueItem);
                log.LogInformation($"Processing time : {dt1 - dt2}");
            }
        }
    }
}
