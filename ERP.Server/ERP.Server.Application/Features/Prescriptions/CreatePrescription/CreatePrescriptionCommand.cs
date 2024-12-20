using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Prescriptions.CreatePrescription;
public sealed record CreatePrescriptionCommand(
    Guid ProductId
    ) : IRequest<Result<Prescription>>;