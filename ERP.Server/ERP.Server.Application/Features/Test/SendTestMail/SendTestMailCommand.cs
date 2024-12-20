using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Mails.SendExampleMail;
public sealed record SendTestMailCommand() : IRequest<Result<string>>;
