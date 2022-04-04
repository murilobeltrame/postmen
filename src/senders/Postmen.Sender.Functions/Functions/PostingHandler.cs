using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Postmen.Sender.Application;

namespace Postmen_Sender_Functions.Functions
{
    public static class PostingHandler
    {
        [FunctionName(nameof(PostingHandler))]
        [return: ServiceBus("postcreated", Connection = "ServiceBusConnectionString")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request  = PostRequest.FromJson(requestBody);
            return request.ToEntity().ToJson();
        }
    }
}
