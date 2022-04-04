using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Postmen.Domain;
using Postmen.Domain.Abstractions;

namespace Postmen_Receiver_Functions.Functions
{
    public static class PostedHandler
    {
        [FunctionName("PostedHandler")]
        public static void Run([ServiceBusTrigger("postcreated", "postcreatedsubscriptionfunctions", Connection = "ServiceBusConnectionString")]string postMessage, ILogger log)
        {
            var post = JsonSerializableEntity<Post>.FromJson(postMessage);
            log.LogInformation("Received message.", post);
        }
    }
}
