using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Prescriptions.DeletePrescriptionById;

internal sealed class DeletePrescriptionByIdCommandHandler(
    IPrescriptionQueryRepository prescriptionQueryRepository,
    IPrescriptionCommandRepository prescriptionCommandRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService
    ) : IRequestHandler<DeletePrescriptionByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeletePrescriptionByIdCommand request, CancellationToken cancellationToken)
    {
        Prescription? prescription = await prescriptionQueryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (prescription is null)
        {
            return Result<string>.Failure("Prescription not found");
        }

        prescription.IsDeleted = true;

        prescriptionCommandRepository.Update(prescription);
        await outboxService.AddMatchDbAsync(TableNames.Prescription, OperationNames.Delete, prescription.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Prescription delete is successful";
    }
}