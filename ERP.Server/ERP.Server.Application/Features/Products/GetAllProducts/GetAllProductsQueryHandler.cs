using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.GetAllProducts;

internal sealed class GetAllProductsQueryHandler(
    IProductQueryRepository productQueryRepository) : IRequestHandler<GetAllProductsQuery, Result<List<GetAllProductsQueryResponse>>>
{
    public async Task<Result<List<GetAllProductsQueryResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await productQueryRepository.GetAllAsync(cancellationToken);

        return products.OrderBy(p => p.Name).Select(s => new GetAllProductsQueryResponse(
            s.Id,
            s.Name,
            s.Type.Name,
            s.CreateAt,
            s.UpdateAt)).ToList();
    }
}