using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using MO.Mapper;
using TS.Result;

namespace ERP.Server.Application.Features.PrescriptionDetails;
public sealed record CreatePrescriptionDetailCommand(
    Guid PrescriptionId,
    Guid ProductId,
    decimal Quantity) : IRequest<Result<string>>;


internal sealed class CreatePrescriptionDetailCommandHandler(
    IPrescriptionDetailCommandRepository prescriptionDetailCommandRepository,
    IUnitOfWork unitOfWork,
    OutboxService outboxService) : IRequestHandler<CreatePrescriptionDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePrescriptionDetailCommand request, CancellationToken cancellationToken)
    {
        PrescriptionDetail prescriptionDetail =
            Mapper.Map<CreatePrescriptionDetailCommand, PrescriptionDetail>(request);

        await prescriptionDetailCommandRepository.CreateAsync(prescriptionDetail, cancellationToken);
        await outboxService.AddMatchDbAsync(TableNames.PrescriptionDetail, OperationNames.Create, prescriptionDetail.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Create is successful";
    }
}
