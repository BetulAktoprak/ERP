using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;

namespace ERP.Server.Application.Services;
internal sealed class OutboxService(
    IMatchDbOutboxRepository matchDbOutboxRepository,
    ISendConfirmEmailOutboxRepository sendConfirmEmailRepository
    )
{
    public async Task AddMatchDbAsync(string tableName, string operatioName, Guid id, CancellationToken cancellationToken)
    {
        MatchDbOutbox outbox = new()
        {
            TableName = tableName,
            OperationName = operatioName,
            RecordId = id,
        };

        await matchDbOutboxRepository.CreateAsync(outbox, cancellationToken);
    }

    public async Task AddSendConfirmEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        SendConfirmEmailOutBox outBox = new()
        {
            To = to,
            Subject = subject,
            Body = body,
        };

        await sendConfirmEmailRepository.CreateAsync(outBox, cancellationToken);
    }
}
