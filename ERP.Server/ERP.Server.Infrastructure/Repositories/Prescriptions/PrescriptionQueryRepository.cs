using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ERP.Server.Infrastructure.Repositories.Prescriptions;

internal sealed class PrescriptionQueryRepository : QueryRepository<Prescription>, IPrescriptionQueryRepository
{
    public PrescriptionQueryRepository() : base("prescriptions")
    {
    }

    public async Task<List<GetAllPrescriptionsQueryResponse>> GetAllAsync<GetAllPrescriptionsQueryResponse>(CancellationToken cancellationToken = default)
    {
        IMongoCollection<Product> productCollection =
            _database.GetCollection<Product>("products");
        var pipeline = new BsonDocument[]
       {
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "products" },
                { "localField", "ProductId" },
                { "foreignField", "_id" },
                { "as", "ProductInfo" }
            }),
            new BsonDocument("$unwind", "$ProductInfo"),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 1 },
                { "ProductId", 1 },
                { "ProductName", "$ProductInfo.Name" },
                { "CreateAt", 1 },
                { "UpdateAt", 1 },
                { "IsDeleted", 1 }
            }),
            new BsonDocument("$match", new BsonDocument
            {
                {"IsDeleted", false }
            })
       };

        var result = await _collection
            .Aggregate<GetAllPrescriptionsQueryResponse>(pipeline)
            .ToListAsync();

        return result;
    }
    public async Task<bool> IsPrescriptionHave(Guid productId, CancellationToken cancellationToken)
    {
        var filter = Builders<Prescription>.Filter.Eq("ProductId", productId);
        return await _collection.Find(filter).AnyAsync(cancellationToken);
    }
}
