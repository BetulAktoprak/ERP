using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Auth.SendConfirmEmail;
public sealed record SendConfirmEmailCommand(
    string Email) : IRequest<Result<string>>;
