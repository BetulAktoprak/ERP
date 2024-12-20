using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Prescriptions.DeletePrescriptionById;
public sealed record DeletePrescriptionByIdCommand(
    Guid Id) : IRequest<Result<string>>;