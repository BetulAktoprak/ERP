using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.PrescriptionDetails;
public sealed record DeletePrescriptionDetailByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeletePrescriptionDetailByIdCommandHandler(
    IPrescriptionDetailCommandRepository prescriptionDetailCommandRepository,
    IPrescriptionDetailQueryRepository prescriptionDetailQueryRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService
    ) : IRequestHandler<DeletePrescriptionDetailByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeletePrescriptionDetailByIdCommand request, CancellationToken cancellationToken)
    {
        PrescriptionDetail? prescriptionDetail =
            await prescriptionDetailQueryRepository
            .GetByIdAsync(request.Id, cancellationToken);

        if (prescriptionDetail is null)
        {
            return Result<string>.Failure("Prescription detail not found");
        }

        prescriptionDetailCommandRepository.Delete(prescriptionDetail);
        await outboxService
            .AddMatchDbAsync(TableNames.PrescriptionDetail, OperationNames.Delete, request.Id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Delete is successful";

    }
}
