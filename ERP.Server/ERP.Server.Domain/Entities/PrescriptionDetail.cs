using ERP.Server.Domain.Abstractions;

namespace ERP.Server.Domain.Entities;

public sealed class PrescriptionDetail : Entity
{
    public Guid PrescriptionId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
}

