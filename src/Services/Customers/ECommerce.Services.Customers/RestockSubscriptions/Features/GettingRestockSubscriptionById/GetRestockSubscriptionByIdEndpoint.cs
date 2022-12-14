using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using ECommerce.Services.Customers.Shared;

namespace ECommerce.Services.Customers.RestockSubscriptions.Features.GettingRestockSubscriptionById;

public class GetRestockSubscriptionByIdEndpoint : IQueryMinimalEndpoint<long, IResult>
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet(
                $"{RestockSubscriptionsConfigs.RestockSubscriptionsUrl}/{{id}}", HandleAsync)
            .WithTags(RestockSubscriptionsConfigs.Tag)
            .RequireAuthorization(CustomersConstants.Role.Admin)
            .Produces<GetRestockSubscriptionByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetRestockSubscriptionById")
            .WithDisplayName("Get RestockSubscription By Id.")
            .WithApiVersionSet(RestockSubscriptionsConfigs.VersionSet)
            .HasApiVersion(1.0);

        return builder;
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        long id,
        IQueryProcessor queryProcessor,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(id, nameof(id));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(GetRestockSubscriptionByIdEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("RestockSubscriptionId", id))
        {
            var result = await queryProcessor.SendAsync(new GetRestockSubscriptionById(id), cancellationToken);

            return Results.Ok(result);
        }
    }
}
