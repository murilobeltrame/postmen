using System;

namespace Postmen.Domain
{
    public class Post
    {
        public string Description { get; private set; }
        public DateTime? DueDateTime { get; private set; }
        public bool Finished { get; private set; }

        public Post(string description, bool finished = false, DateTime? dueDateTime = null)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(description);

            Description = description;
            Finished = finished;
            DueDateTime = dueDateTime;
        }

        public bool Delayed(DateTime now) => DueDateTime.HasValue && DueDateTime.Value > now && !Finished;
    }
}
