using ERP.Server.Domain.DTOs;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.PrescriptionDetails;
public sealed record GetPrescriptionWithDetailsQuery(
    Guid PrescriptionId) : IRequest<Result<GetPrescriptionWithDetailsQueryResponse>>;

internal sealed class GetPrescriptionWithDetailsQueryHandler(
    IPrescriptionQueryRepository prescriptionQueryRepository,
    IPrescriptionDetailQueryRepository prescriptionDetailQueryRepository,
    IProductQueryRepository productQueryRepository
    ) : IRequestHandler<GetPrescriptionWithDetailsQuery, Result<GetPrescriptionWithDetailsQueryResponse>>
{
    public async Task<Result<GetPrescriptionWithDetailsQueryResponse>> Handle(GetPrescriptionWithDetailsQuery request, CancellationToken cancellationToken)
    {
        var prescription = await prescriptionQueryRepository.GetByIdAsync(request.PrescriptionId, cancellationToken);

        if (prescription is null)
        {
            return Result<GetPrescriptionWithDetailsQueryResponse>.Failure("Prescription not found");
        }

        List<PrescriptionDetail> details = await prescriptionDetailQueryRepository.GetAllByPrescriptionId(request.PrescriptionId, cancellationToken);

        List<PrescriptionDetailDto> prescriptionDetailDtos = new();

        foreach (var item in details)
        {
            Product? product = await productQueryRepository.GetByIdAsync(item.ProductId, cancellationToken);

            if (product is null)
            {
                return Result<GetPrescriptionWithDetailsQueryResponse>.Failure("Product not found");
            }

            PrescriptionDetailDto prescriptionDetailDto = new()
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = product.Name,
                Quantity = item.Quantity
            };

            prescriptionDetailDtos.Add(prescriptionDetailDto);
        }

        GetPrescriptionWithDetailsQueryResponse response = new();

        Product? prescriptionProduct = await productQueryRepository.GetByIdAsync(prescription.ProductId, cancellationToken);

        if (prescriptionProduct is null)
        {
            return Result<GetPrescriptionWithDetailsQueryResponse>.Failure("Product not found");
        }

        response.Id = request.PrescriptionId;
        response.ProductId = prescription.ProductId;
        response.Details = prescriptionDetailDtos;
        response.ProductName = prescriptionProduct.Name;

        return response;
    }
}