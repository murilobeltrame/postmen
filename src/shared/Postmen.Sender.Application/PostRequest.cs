using Postmen.Domain;
using System;

namespace Postmen.Sender.Application
{
    public class PostRequest
    {
        public string Description { get; set; }
        public DateTime? DueDateTime { get; set; }

        internal Post ToEntity()
        {
            return new Post(Description, false, DueDateTime);
        }
    }
}