using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace MyFunctionApp
{
    public static class ProcessLineFunction
    {
        [FunctionName("ProcessLine")]
        public static async Task ProcessLine(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [CosmosDB(
                databaseName: "my-database",
                containerName: "my-container",
                Connection = "CosmosDBConnection")] IAsyncCollector<dynamic> documentsOut)
        {
            var line = context.GetInput<string>();

            // Process the line and create a document to persist into Cosmos DB
            var document = new { id = Guid.NewGuid(), data = line };

            await documentsOut.AddAsync(document);
        }
    }
}