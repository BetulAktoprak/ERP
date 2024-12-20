using ERP.Server.Domain.Abstractions;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ERP.Server.Infrastructure.Context;
internal sealed class ApplicationDbContext(
    DbContextOptions options
    ) : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products { get; set; }
    public DbSet<MatchDbOutbox> MatchDbOutboxes { get; set; }
    public DbSet<SendConfirmEmailOutBox> SendConfirmEmailOutBoxes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity>> entires = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        var userIdCliam = httpContextAccessor.HttpContext?.User.Claims.Where(p => p.Type == "userId").FirstOrDefault();
        Guid? userId = null;
        if (userIdCliam is not null)
        {
            userId = Guid.Parse(userIdCliam.Value);
        }

        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity> entry in entires)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt)
                    .CurrentValue = DateTime.Now.AddHours(3);
                if (userId is not null)
                {
                    entry.Property(p => p.CreatedUserId)
                        .CurrentValue = (Guid)userId;
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt)
                    .CurrentValue = DateTime.Now.AddHours(3);
                    if (userId is not null)
                    {
                        entry.Property(p => p.DeletedUserId)
                            .CurrentValue = (Guid)userId;
                    }
                }
                else
                {
                    entry.Property(p => p.UpdateAt)
                        .CurrentValue = DateTime.Now.AddHours(3);
                    if (userId is not null)
                    {
                        entry.Property(p => p.UpdatedUserId)
                            .CurrentValue = (Guid)userId;
                    }
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
