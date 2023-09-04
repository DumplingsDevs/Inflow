using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Inflow.Shared.Abstractions.Queries;
using Pipelines;

namespace Inflow.Shared.Infrastructure.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddPipeline()
            .AddInput(typeof(IQuery<>))
            .AddHandler(typeof(IQueryHandler<,>), assemblies.ToArray())
            .AddDispatcher<IQueryDispatcher>(typeof(SharedInfraMarker).Assembly)
            .WithDecorators(x =>
            {
                x.WithAttribute<DecoratorAttribute>();
            }, typeof(SharedInfraMarker).Assembly)
            .Build();

        return services;
    }
}