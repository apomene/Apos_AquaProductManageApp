
using Microsoft.EntityFrameworkCore;
using Apos_AquaProductManageApp.Model;

namespace Apos_AquaProductManageApp.DBContext
{    
    public class FishFarmDbContext : DbContext
    {
        public FishFarmDbContext(DbContextOptions<FishFarmDbContext> options) : base(options) { }
        public DbSet<Cage> Cages { get; set; }
        public DbSet<FishStocking> FishStockings { get; set; }
        public DbSet<Mortality> Mortalities { get; set; }

        public DbSet<FishTransfer> FishTransfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FishTransfer>()
                .HasOne(t => t.FromCage)
                .WithMany()
                .HasForeignKey(t => t.FromCageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FishTransfer>()
                .HasOne(t => t.ToCage)
                .WithMany()
                .HasForeignKey(t => t.ToCageId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }

}
