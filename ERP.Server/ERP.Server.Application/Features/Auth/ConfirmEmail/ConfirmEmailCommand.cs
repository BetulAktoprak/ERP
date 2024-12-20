using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Auth.ConfirmEmail;
public sealed record ConfirmEmailCommand(
    Guid Code) : IRequest<Result<string>>;
