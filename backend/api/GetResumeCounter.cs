using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, new[] { "get", "post" }, Route = null)] HttpRequest req,
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
            int currentCount = (int)document["count"] + 1;
            document["count"] = currentCount;

            // Add the updated document to the collector to replace the item in Cosmos DB
            await updatedDocumentCollector.AddAsync(document);

            // Log the updated count
            log.LogInformation($"Count updated to {currentCount}");

            // Create and return a properly formatted JSON response
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"{{\"count\": {currentCount}}}", System.Text.Encoding.UTF8, "application/json")
            };
            response.Headers.Add("Access-Control-Allow-Origin", "*"); // Allow all origins
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            return response;
        }
    }
}
