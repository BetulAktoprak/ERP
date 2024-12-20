using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;

namespace ERP.Server.Infrastructure.Repositories.StockMovements;
internal sealed class StockMovementCommandRepository : CommandRepository<StockMovement>, IStockMovementCommandRepository
{
    public StockMovementCommandRepository(ApplicationDbContext context) : base(context)
    {
    }
}
