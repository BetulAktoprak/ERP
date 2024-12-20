using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using MO.Mapper;
using TS.Result;

namespace ERP.Server.Application.Features.Users.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService
    ) : IRequestHandler<CreateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isUserNameExists = await userQueryRepository.IsUserNameExistsAsync(request.UserName, cancellationToken);

        if (isUserNameExists)
        {
            return Result<string>.Failure("Username already exists");
        }

        bool isEmailExists = await userQueryRepository.IsEmailExistsAsync(request.Email, cancellationToken);

        if (isEmailExists)
        {
            return Result<string>.Failure("Email already exists");
        }

        HashingHelper.CreatePassword(request.Password, out byte[] passwordSalt, out byte[] passwordHash);

        User user = Mapper.Map<CreateUserCommand, User>(request);

        user.PasswordSalt = passwordSalt;
        user.PasswordHash = passwordHash;
        user.MailConfirmCode = Guid.NewGuid();

        await userCommandRepository.CreateAsync(user, cancellationToken);

        await outboxService.AddMatchDbAsync(TableNames.User, OperationNames.Create, user.Id, cancellationToken);

        string body = CreateSendConfirmEmailBody(user.FullName, user.MailConfirmCode);
        await outboxService.AddSendConfirmEmailAsync(user.Email, "Mail Onaylama", body, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "User create is successful";
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


