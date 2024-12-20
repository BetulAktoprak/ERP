using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using MO.Mapper;
using TS.Result;

namespace ERP.Server.Application.Features.Prescriptions.CreatePrescription;

internal sealed class CreatePrescriptionCommandHandler(
    IPrescriptionCommandRepository prescriptionCommandRepository,
    IPrescriptionQueryRepository prescriptionQueryRepository,
    OutboxService outboxService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreatePrescriptionCommand, Result<Prescription>>
{
    public async Task<Result<Prescription>> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
    {
        bool isPrescriptionHave = await prescriptionQueryRepository.IsPrescriptionHave(request.ProductId, cancellationToken);

        if (isPrescriptionHave)
        {
            return Result<Prescription>.Failure("Prescription already exist");
        }

        Prescription prescription = Mapper.Map<CreatePrescriptionCommand, Prescription>(request);

        await prescriptionCommandRepository.CreateAsync(prescription, cancellationToken);
        await outboxService.AddMatchDbAsync(TableNames.Prescription, OperationNames.Create, prescription.Id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return prescription;
    }
}

