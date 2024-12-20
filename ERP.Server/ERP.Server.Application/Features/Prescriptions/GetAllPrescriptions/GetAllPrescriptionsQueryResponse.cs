namespace ERP.Server.Application.Features.Prescriptions.GetAllPrescriptions;

public sealed record GetAllPrescriptionsQueryResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    DateTime CreateAt,
    DateTime? UpdateAt,
    bool IsDeleted
    );