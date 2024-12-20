using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.StockMovements;
public sealed record GetAllStockMovementsQuery(
    Guid ProductId) : IRequest<Result<List<GetAllStockMovementQueryResponse>>>;

public sealed record GetAllStockMovementQueryResponse
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string TypeName { get; set; } = default!;
    public Guid TypeId { get; set; }
    public decimal Giris { get; set; }
    public decimal Cikis { get; set; }
    public decimal Bakiye { get; set; }
}

internal sealed class GetAllStockMovementsQueryHandler(
    IStockMovementQueryRepository stockMovementQueryRepository) : IRequestHandler<GetAllStockMovementsQuery, Result<List<GetAllStockMovementQueryResponse>>>
{
    public async Task<Result<List<GetAllStockMovementQueryResponse>>> Handle(GetAllStockMovementsQuery request, CancellationToken cancellationToken)
    {
        var stockMovements = (await stockMovementQueryRepository.GetAllAsync(request.ProductId, cancellationToken)).OrderBy(p => p.Date).ToList();

        List<GetAllStockMovementQueryResponse> response = new();

        for (var i = 0; i < stockMovements.Count; i++)
        {
            var item = stockMovements[i];

            decimal giris = 0;
            decimal cikis = 0;

            if (item.Type == StockMovementTypeEnum.AlisFaturasi)
            {
                giris = item.Quantity;
            }
            else if (item.Type == StockMovementTypeEnum.SatisFaturasi)
            {
                cikis = item.Quantity;
            }
            else if (item.Type == StockMovementTypeEnum.Uretim)
            {
                giris = item.Quantity;
            }
            else if (item.Type == StockMovementTypeEnum.IadeFaturasi)
            {
                giris = item.Quantity;
            }
            else if (item.Type == StockMovementTypeEnum.Devir)
            {
                giris = item.Quantity;
            }

            decimal bakiye = 0;

            if (i > 0)
            {
                bakiye = response[i - 1].Bakiye;
            }

            GetAllStockMovementQueryResponse movementQueryResponse = new()
            {
                Id = item.Id,
                Date = item.Date,
                TypeName = item.Type.Name,
                TypeId = item.TypeId,
                Giris = giris,
                Cikis = cikis,
                Bakiye = (cikis - giris) + bakiye
            };

            response.Add(movementQueryResponse);
        }

        return response;
    }
}
