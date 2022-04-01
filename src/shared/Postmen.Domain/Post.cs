namespace Postmen.Domain
{
    public class Post
    {
        public string Message { get; private set; }
        public DateTime? ExpirationDateTime { get; private set; }
        public bool Expired { get { return ExpirationDateTime.HasValue && ExpirationDateTime.Value > DateTime.Now; } }

        public Post(string message, DateTime? expirationDateTime)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            Message = message;
            ExpirationDateTime = expirationDateTime;
        }
    }

}
