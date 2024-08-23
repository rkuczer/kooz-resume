using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Newtonsoft.Json.Linq;

namespace Company.Function
{
    public static class UpdateCountFunction
    {
        [FunctionName("UpdateCount")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "AzureResume",
                containerName: "Counter",
                Id = "1",
                PartitionKey = "1",
                Connection = "AzureResumeConnectionString")] JObject document,
            [CosmosDB(
                databaseName: "AzureResume",
                containerName: "Counter",
                Connection = "AzureResumeConnectionString")] IAsyncCollector<JObject> updatedDocumentCollector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function to update count in Cosmos DB.");

            // Increment the count
            document["count"] = (int)document["count"] + 1;

            // Add the updated document to the collector to replace the item in Cosmos DB
            await updatedDocumentCollector.AddAsync(document);

            // Log the updated count
            string resultMessage = $"Count updated to {document["count"]}";
            log.LogInformation(resultMessage);

            // Return the result
            return new OkObjectResult(resultMessage);
        }
    }
}
