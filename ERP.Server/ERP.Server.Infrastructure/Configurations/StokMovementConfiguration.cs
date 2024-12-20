using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Server.Infrastructure.Configurations;

internal sealed class StokMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.Property(p => p.Type).HasConversion(type => type.Value, value => StockMovementTypeEnum.FromValue(value));
        builder.Property(p => p.Quantity).HasColumnType("decimal(7,2)");
        builder.Property(p => p.Price).HasColumnType("money");
    }
}
