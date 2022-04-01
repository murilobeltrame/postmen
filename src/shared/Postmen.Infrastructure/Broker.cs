using Postmen.Domain.Interfaces;

namespace Postmen.Infrastructure
{
    public class Broker : IBroker
    {
        private readonly string _connectionString;

        public Broker(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task Listen<T>(string topicName, string subsCriptionName, Func<T, Task> handler, Func<Exception, Task>? exceptionHandler)
        {
            throw new NotImplementedException();
        }

        public Task Publish<T>(string topicName, T payload)
        {
            throw new NotImplementedException();
        }
    }
}

