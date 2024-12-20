using ERP.Server.Application.Features.StockMovements;
using MediatR;

namespace ERP.Server.WebAPI.Middlewares;

public static class StockMovementEndpoints
{
    public static IEndpointRouteBuilder AddStockMovementEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/StockMovements/GetAll", async (Guid productId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            GetAllStockMovementsQuery request = new(productId);
            var response = await mediator.Send(request, cancellationToken);
            return Results.Ok(response);
        })
        .WithTags("StockMovements")
        .RequireAuthorization();

        return app;
    }
}
