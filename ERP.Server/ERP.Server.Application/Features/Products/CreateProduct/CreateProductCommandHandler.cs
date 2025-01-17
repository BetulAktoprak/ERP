﻿using ERP.Server.Application.Services;
using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Repositories;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Products.CreateProduct;

internal sealed class CreateProductCommandHandler(
    OutboxService outboxService,
    IProductCommandRepository productCommandRepository,
    IProductQueryRepository productQueryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        bool isNameExists = await productQueryRepository.IsNameExistsAsync(request.Name, cancellationToken);

        if (isNameExists)
        {
            return Result<string>.Failure("Product name already exist");
        }

        Product product = new()
        {
            Name = request.Name,
            Type = ProductTypeEnum.FromValue(request.TypeValue)
        };

        await productCommandRepository.CreateAsync(product, cancellationToken);

        #region Outbox
        await outboxService.AddMatchDbAsync(TableNames.Product, OperationNames.Create, product.Id, cancellationToken);
        #endregion

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Product create is successful";
    }
}