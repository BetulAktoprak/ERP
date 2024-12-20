namespace ERP.Server.Application.Features.Products.GetAllProducts;

public sealed record GetAllProductsQueryResponse(
    Guid Id,
    string Name,
    string TypeName,
    DateTime CreateAt,
    DateTime? UpdateAt);