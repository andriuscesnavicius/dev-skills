namespace Account.DataAccess
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Utility.DataAccess.Entity;

    public abstract class AuditableDbContext : DbContext
    {
        public AuditableDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetDefaultValues();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetDefaultValues();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public void SetDefaultValues()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity is IAuditable))
            {
                UpdateAuditable(entry, entry.Entity as IAuditable);
            }
        }

        public void UpdateAuditable(EntityEntry entityEntry, IAuditable auditableEntry)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    auditableEntry.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    auditableEntry.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entityEntry.State = EntityState.Modified;
                    auditableEntry.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
