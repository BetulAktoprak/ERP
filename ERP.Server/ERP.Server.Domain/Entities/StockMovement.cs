using ERP.Server.Domain.Abstractions;
using ERP.Server.Domain.Enums;

namespace ERP.Server.Domain.Entities;

public sealed class StockMovement : Entity
{
    public Guid ProductId { get; set; }
    public DateOnly Date { get; set; }
    public StockMovementTypeEnum Type { get; set; } = StockMovementTypeEnum.Uretim;
    public Guid TypeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
}
