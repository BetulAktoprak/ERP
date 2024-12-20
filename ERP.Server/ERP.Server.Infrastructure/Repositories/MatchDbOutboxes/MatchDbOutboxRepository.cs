using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Server.Infrastructure.Repositories.Outboxes;
internal sealed class MatchDbOutboxRepository : CommandRepository<MatchDbOutbox>, IMatchDbOutboxRepository
{
    private readonly ApplicationDbContext _context;
    public MatchDbOutboxRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<MatchDbOutbox>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return
            await _context.MatchDbOutboxes
            .Where(p => p.IsCompleted == false)
            .OrderBy(p => p.CreateAt)
            .ToListAsync(cancellationToken);
    }
}