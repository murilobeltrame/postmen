using Newtonsoft.Json;
using Postmen.Domain;
using System;

namespace Postmen.Sender.Application
{
    public class PostRequest
    {
        public string Description { get; set; }
        public DateTime? DueDateTime { get; set; }

        public Post ToEntity()
        {
            return new Post(Description, false, DueDateTime);
        }

        public static PostRequest FromJson(string json)
        {
            return JsonConvert.DeserializeObject<PostRequest>(json);
        }
    }
}