using ERP.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Server.Infrastructure.Configurations;

internal sealed class PrescriptionDetailConfiguration : IEntityTypeConfiguration<PrescriptionDetail>
{
    public void Configure(EntityTypeBuilder<PrescriptionDetail> builder)
    {
        builder.Property(p => p.Quantity).HasColumnType("decimal(7,2)");
    }
}
