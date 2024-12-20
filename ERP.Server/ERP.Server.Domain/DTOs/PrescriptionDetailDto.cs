namespace ERP.Server.Domain.DTOs;
public sealed record PrescriptionDetailDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal Quantity { get; set; }
}

public class GetPrescriptionWithDetailsQueryResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public List<PrescriptionDetailDto> Details { get; set; } = new();
};
