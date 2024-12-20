using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Auth.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler(
    IUserQueryRepository userQueryRepository,
    OutboxService outboxService,
    IUnitOfWork unitOfWork) : IRequestHandler<ConfirmEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        User? user = await userQueryRepository.GetUserByConfirmEmailCodeAsync(request.Code, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("User not found");
        }

        if (user.IsEmailConfirmed)
        {
            return Result<string>.Failure("User email already confirmed");
        }

        user.IsEmailConfirmed = true;

        await outboxService.AddMatchDbAsync(TableNames.User, OperationNames.Update, user.Id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Your e-mail address has been successfully confirmed. You can log in after 1 minute";
    }
}
