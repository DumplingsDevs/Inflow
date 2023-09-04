using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Inflow.Shared.Abstractions.Events;
using Pipelines;

namespace Inflow.Shared.Infrastructure.Events;

public static class Extensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddPipeline()
            .AddInput(typeof(IEvent))
            .AddHandler(typeof(IEventHandler<>), assemblies.ToArray())
            .AddDispatcher<IEventDispatcher>(typeof(SharedInfraMarker).Assembly)
            .WithDecorators(x =>
            {
                x.WithAttribute<DecoratorAttribute>();
            }, typeof(SharedInfraMarker).Assembly)
            .Build();

        return services;
    }
}