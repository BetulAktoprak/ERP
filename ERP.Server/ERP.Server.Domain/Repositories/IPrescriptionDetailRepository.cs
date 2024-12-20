using ERP.Server.Domain.Entities;

namespace ERP.Server.Domain.Repositories;
public interface IPrescriptionDetailCommandRepository : ICommandRepository<PrescriptionDetail>
{
}

public interface IPrescriptionDetailQueryRepository : IQueryRepository<PrescriptionDetail>
{
    Task<List<PrescriptionDetail>> GetAllByPrescriptionId(Guid prescriptionId, CancellationToken cancellationToken);
}