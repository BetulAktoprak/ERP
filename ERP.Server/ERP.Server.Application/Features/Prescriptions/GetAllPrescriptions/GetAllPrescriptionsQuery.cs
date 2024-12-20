using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Prescriptions.GetAllPrescriptions;
public sealed record GetAllPrescriptionsQuery()
    : IRequest<Result<List<GetAllPrescriptionsQueryResponse>>>;