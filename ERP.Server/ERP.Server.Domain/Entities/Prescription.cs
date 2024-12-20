using ERP.Server.Domain.Abstractions;

namespace ERP.Server.Domain.Entities;
public sealed class Prescription : Entity
{
    public Guid ProductId { get; set; }
}

