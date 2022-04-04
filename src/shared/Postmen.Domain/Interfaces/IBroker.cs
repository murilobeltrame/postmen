namespace Postmen.Domain.Interfaces
{
    public interface IBroker
    {
        Task Publish<T>(string topicName, T payload);

        Task Listen<T>(string topicName, string subscriptionName, Func<T, Task> handler, Func<Exception, Task>? exceptionHandler);
    }
}
