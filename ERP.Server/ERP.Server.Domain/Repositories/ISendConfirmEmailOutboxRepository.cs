using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;

public interface ISendConfirmEmailOutboxRepository : ICommandRepository<SendConfirmEmailOutBox>
{
    Task<List<SendConfirmEmailOutBox>> GetAllAsync(CancellationToken cancellationToken = default);
}