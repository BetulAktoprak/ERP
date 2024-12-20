using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;

namespace ERP.Server.Infrastructure.Repositories.Prescriptions;

internal sealed class PrescriptionDetailCommandRepository : CommandRepository<PrescriptionDetail>, IPrescriptionDetailCommandRepository
{
    public PrescriptionDetailCommandRepository(ApplicationDbContext context) : base(context)
    {
    }
}
