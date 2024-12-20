using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;

public interface IStockMovementCommandRepository : ICommandRepository<StockMovement>
{
}

public interface IStockMovementQueryRepository : IQueryRepository<StockMovement>
{
    Task<List<StockMovement>> GetAllAsync(Guid productId, CancellationToken cancellationToken = default);
}