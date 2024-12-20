using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;

namespace ERP.Server.Infrastructure.Repositories.Prescriptions;
internal sealed class PrescriptionCommandRepository : CommandRepository<Prescription>, IPrescriptionCommandRepository
{
    public PrescriptionCommandRepository(ApplicationDbContext context) : base(context)
    {
    }
}