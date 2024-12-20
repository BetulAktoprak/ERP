using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;

public interface IPrescriptionCommandRepository : ICommandRepository<Prescription>
{
}

public interface IPrescriptionQueryRepository : IQueryRepository<Prescription>
{
    Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default);
    Task<bool> IsPrescriptionHave(Guid productId, CancellationToken cancellationToken);
}