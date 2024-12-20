using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;
public interface IMatchDbOutboxRepository : ICommandRepository<MatchDbOutbox>
{
    Task<List<MatchDbOutbox>> GetAllAsync(CancellationToken cancellationToken = default);
}
