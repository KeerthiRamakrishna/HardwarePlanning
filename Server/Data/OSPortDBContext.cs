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

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}