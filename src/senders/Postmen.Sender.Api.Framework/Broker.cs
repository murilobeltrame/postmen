using System;

namespace Postmen.Sender.Api.Framework
{
    public class Broker: Infrastructure.Broker
    {
        public Broker() : base(Environment.GetEnvironmentVariable("ConnectionStrings:ServiceBus")) { }
    }
}