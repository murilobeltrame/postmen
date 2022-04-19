using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Postmen.Domain.Interfaces;
using Postmen.Sender.Application;
using Postmen.Sender.Application.Interfaces;
using System.Reflection;

namespace Postmen.Sender.Api.Framework
{
    public static class ApplicationDependencyResolver
    {
        public static IContainer Container()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Broker>().As<IBroker>();
            builder.RegisterType<ApplicationService>().As<IApplicationService>();

            return builder.Build();
        }
    }
}