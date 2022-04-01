using Postmen.Domain;

namespace Postmen.Sender.Application
{
    public class PostRequest
    {
        public string Message { get; set; }
        public DateTime? ExpirationDateTime { get; set; }

        internal Post ToEntity() => new Post(Message, ExpirationDateTime);
    }
}
