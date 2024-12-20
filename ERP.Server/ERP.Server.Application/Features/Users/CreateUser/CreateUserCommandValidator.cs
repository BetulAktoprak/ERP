using FluentValidation;

namespace ERP.Server.Application.Features.Users.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(p => p.FirstName).MinimumLength(3);
        RuleFor(p => p.LastName).MinimumLength(3);
        RuleFor(p => p.UserName).MinimumLength(3);
        RuleFor(p => p.Email).EmailAddress();
    }
}
