using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MongoDB.Driver;

namespace ERP.Server.Infrastructure.Repositories.Prescriptions;

internal sealed class PrescriptionDetailQueryRepository : QueryRepository<PrescriptionDetail>, IPrescriptionDetailQueryRepository
{
    public PrescriptionDetailQueryRepository() : base("prescription-details")
    {
    }

    public async Task<List<PrescriptionDetail>> GetAllByPrescriptionId(Guid prescriptionId, CancellationToken cancellationToken)
    {
        FilterDefinition<PrescriptionDetail> filter = Builders<PrescriptionDetail>.Filter.And(
              Builders<PrescriptionDetail>.Filter.Eq("PrescriptionId", prescriptionId),
              Builders<PrescriptionDetail>.Filter.Eq("IsDeleted", false)
);
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }
}