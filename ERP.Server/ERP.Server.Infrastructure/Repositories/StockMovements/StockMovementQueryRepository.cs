using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MongoDB.Driver;

namespace ERP.Server.Infrastructure.Repositories.StockMovements;

internal sealed class StockMovementQueryRepository : QueryRepository<StockMovement>, IStockMovementQueryRepository
{
    public StockMovementQueryRepository() : base("stock-movements")
    {
    }

    public async Task<List<StockMovement>> GetAllAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        FilterDefinition<StockMovement> filter = Builders<StockMovement>.Filter.And(
              Builders<StockMovement>.Filter.Eq("ProductId", productId),
              Builders<StockMovement>.Filter.Eq("IsDeleted", false));
        return await _collection.Find(filter).ToListAsync();
    }
}
