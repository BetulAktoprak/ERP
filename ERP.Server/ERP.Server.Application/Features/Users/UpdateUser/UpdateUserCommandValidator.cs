using FluentValidation;

namespace ERP.Server.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(p => p.FirstName).MinimumLength(3);
        RuleFor(p => p.LastName).MinimumLength(3);
        RuleFor(p => p.UserName).MinimumLength(3);
        RuleFor(p => p.Email).EmailAddress();
    }
}