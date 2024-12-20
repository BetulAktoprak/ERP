using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Server.Infrastructure.Repositories.Outboxes;

internal sealed class SendConfirmEmailOutboxRepository : CommandRepository<SendConfirmEmailOutBox>, ISendConfirmEmailOutboxRepository
{
    private readonly ApplicationDbContext _context;
    public SendConfirmEmailOutboxRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<SendConfirmEmailOutBox>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return
            await _context.SendConfirmEmailOutBoxes
            .Where(p => p.IsCompleted == false)
            .OrderBy(p => p.CreateAt)
            .ToListAsync(cancellationToken);
    }
}