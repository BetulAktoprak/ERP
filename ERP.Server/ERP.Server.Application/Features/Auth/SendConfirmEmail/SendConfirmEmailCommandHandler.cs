using ERP.Server.Application.Services;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Auth.SendConfirmEmail;

internal sealed class SendConfirmEmailCommandHandler(
    IUserQueryRepository userQueryRepository,
    OutboxService outboxService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<SendConfirmEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        User? user = await userQueryRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("User not found");
        }

        if (user.IsEmailConfirmed)
        {
            return Result<string>.Failure("Email already confirmed");
        }

        string body = CreateSendConfirmEmailBody(user.FullName, user.MailConfirmCode);
        await outboxService.AddSendConfirmEmailAsync(user.Email, "Mail Onaylama", body, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Confirm email send is succesful";
    }

    private string CreateSendConfirmEmailBody(string fullName, Guid code)
    {
        string body = @$"<h3>Merhaba, <b>{fullName}</b></h3>
        <p>Mail adresinizi aşağıdaki linke tıklayarak onaylayabilirsiniz.</p>
<a href=""http://localhost:4200/confirm-email/{code}"" target=""_blank"">Mail Adresini Onayla</a>
";

        return body;
    }
}
