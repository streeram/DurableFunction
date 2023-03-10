using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace MyFunctionApp
{
    public static class BlobTriggerFunction
    {
        [FunctionName("BlobTrigger")]
        public static async Task Run(
            [BlobTrigger("mycontainer/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
            string name,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using (var reader = new StreamReader(myBlob))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var instanceId = await starter.StartNewAsync("ProcessLine", line);
                    log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
                }
            }
        }
    }
}