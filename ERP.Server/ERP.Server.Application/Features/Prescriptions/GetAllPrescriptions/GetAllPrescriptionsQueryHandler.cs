using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Prescriptions.GetAllPrescriptions;

internal sealed class GetAllPrescriptionsQueryHandler(
    IPrescriptionQueryRepository prescriptionQueryRepository) : IRequestHandler<GetAllPrescriptionsQuery, Result<List<GetAllPrescriptionsQueryResponse>>>
{
    public async Task<Result<List<GetAllPrescriptionsQueryResponse>>> Handle(GetAllPrescriptionsQuery request, CancellationToken cancellationToken)
    {
        var response = await prescriptionQueryRepository.GetAllAsync<GetAllPrescriptionsQueryResponse>(cancellationToken);
        return response;
    }
}
