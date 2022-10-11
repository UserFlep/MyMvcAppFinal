using Microsoft.EntityFrameworkCore;
using MyMvcAppFinal.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MyMvcAppFinal.Data
{
    public class UnitContext : DbContext
    {
        public DbSet<Unit> Units { get; set; }

        public UnitContext(DbContextOptions<UnitContext> options) : base(options)
        {
            /*Database.EnsureDeleted();*/
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasAlternateKey(x => x.Name);
                entity.HasOne(x => x.Parent)
                    .WithMany(x => x.SubUnits)
                    .HasForeignKey(x => x.ParentId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
