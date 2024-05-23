using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Osporting.Server.Models.OSPortDB;

namespace Osporting.Server.Data
{
    public partial class OSPortDBContext : DbContext
    {
        public OSPortDBContext()
        {
        }

        public OSPortDBContext(DbContextOptions<OSPortDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Osporting.Server.Models.OSPortDB.HardwarePlanning>()
              .HasOne(i => i.Architecture)
              .WithMany(i => i.HardwarePlannings)
              .HasForeignKey(i => i.ArchitectureID)
              .HasPrincipalKey(i => i.ArchitectureID);

            builder.Entity<Osporting.Server.Models.OSPortDB.HardwarePlanning>()
              .HasOne(i => i.Derivative)
              .WithMany(i => i.HardwarePlannings)
              .HasForeignKey(i => i.DerivativeID)
              .HasPrincipalKey(i => i.DerivativeID);

            builder.Entity<Osporting.Server.Models.OSPortDB.HardwarePlanning>()
            .HasOne(i => i.Status)
            .WithMany(i => i.HardwarePlannings)
            .HasForeignKey(i => i.StatusID)
            .HasPrincipalKey(i => i.StatusID);

            builder.Entity<Osporting.Server.Models.OSPortDB.HardwarePlanning>()
              .HasOne(i => i.TestPc)
              .WithMany(i => i.HardwarePlannings)
              .HasForeignKey(i => i.TestPCID)
              .HasPrincipalKey(i => i.TestPCID);

            builder.Entity<Osporting.Server.Models.OSPortDB.HardwarePlanning>()
            .HasOne(i => i.Person)
            .WithMany(i => i.HardwarePlannings)
            .HasForeignKey(i => i.PersonID)
            .HasPrincipalKey(i => i.PersonID);

            //builder.Entity<Osporting.Server.Models.OSPortDB.Person>()
            //  .HasOne(i => i.Person1)
            //  .WithMany(i => i.People1)
            //  .HasForeignKey(i => i.PersonID)
            //  .HasPrincipalKey(i => i.PersonID);
            //this.OnModelBuilding(builder);
        }

        public DbSet<Osporting.Server.Models.OSPortDB.Architecture> Architectures { get; set; }

        public DbSet<Osporting.Server.Models.OSPortDB.Derivative> Derivatives { get; set; }

        public DbSet<Osporting.Server.Models.OSPortDB.HardwarePlanning> HardwarePlannings { get; set; }

        public DbSet<Osporting.Server.Models.OSPortDB.Person> People { get; set; }

        public DbSet<Osporting.Server.Models.OSPortDB.Status> Statuses { get; set; }

        public DbSet<Osporting.Server.Models.OSPortDB.TestPc> TestPcs { get; set; }

        public DbSet<AuditTrail> AuditLogs { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            OnBeforeSaveChanges(userId);

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = userId;
                    //((AuditableEntity)entityEntry.Entity).CreatedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
                    //((AuditableEntity)entityEntry.Entity).CreatedBy = this.httpContextAccessor?.Identity?.Name ?? "MyApp";
                }
                else
                {
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    //Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                ((AuditableEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
                ((AuditableEntity)entityEntry.Entity).ModifiedBy = userId;
                //((AuditableEntity)entityEntry.Entity).ModifiedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }

            var result = await base.SaveChangesAsync();
            return result;
        }
        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditTrail || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    


    //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    //    {
    //        var entries = ChangeTracker
    //            .Entries()
    //            .Where(e => e.Entity is AuditableEntity &&
    //                        (e.State == EntityState.Added || e.State == EntityState.Modified));

    //        foreach (var entityEntry in entries)
    //        {
    //            if (entityEntry.State == EntityState.Added)
    //            {
    //                ((AuditableEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
    //                //((AuditableEntity)entityEntry.Entity).CreatedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
    //                //((AuditableEntity)entityEntry.Entity).CreatedBy = this.httpContextAccessor?.Identity?.Name ?? "MyApp";
    //            }
    //            else
    //            {
    //                Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
    //                //Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
    //            }

    //            ((AuditableEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
    //            //((AuditableEntity)entityEntry.Entity).ModifiedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
    //        }

    //        return await base.SaveChangesAsync(cancellationToken);
    //    }


    }
}