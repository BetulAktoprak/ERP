using AutoMapper;
using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    OutboxService outboxService
    ) : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userQueryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("User not found");
        }

        if (user.UserName != request.UserName)
        {
            bool isUserNameExists = await userQueryRepository.IsUserNameExistsAsync(request.UserName, cancellationToken);

            if (isUserNameExists)
            {
                return Result<string>.Failure("Username already exists");
            }
        }

        if (user.Email != request.Email)
        {
            bool isEmailExists = await userQueryRepository.IsEmailExistsAsync(request.Email, cancellationToken);

            if (isEmailExists)
            {
                return Result<string>.Failure("Email already exists");
            }

            user.MailConfirmCode = Guid.NewGuid();
            user.IsEmailConfirmed = false;
            string body = CreateSendConfirmEmailBody(user.FullName, user.MailConfirmCode);
            await outboxService.AddSendConfirmEmailAsync(request.Email, "Mail Onaylama", body, cancellationToken);
        }

        mapper.Map(request, user);

        userCommandRepository.Update(user);
        await outboxService.AddMatchDbAsync(TableNames.User, OperationNames.Update, user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "User update is successful";
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
