using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using System.Text;

namespace Company.Function
{
    public static class HttpTriggerCSharp1
    {
        [FunctionName("HttpTriggerCSharp1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            QueueClient queueClient = new QueueClient(connectionString, "myqueue-items");
            await queueClient.CreateAsync();

            Encoding enc = Encoding.GetEncoding("UTF-8");
            string base64String = Convert.ToBase64String(enc.GetBytes("start"));

            for (int i = 0; i < 10; i++)
            {
                await queueClient.SendMessageAsync(base64String);
                await Task.Delay(100);
            }

            return new OkObjectResult("Ok");
        }
    }
}
