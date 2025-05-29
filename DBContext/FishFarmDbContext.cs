
using Microsoft.EntityFrameworkCore;
using Apos_AquaProductManageApp.Model;

namespace Apos_AquaProductManageApp.DBContext
{    
    public class FishFarmDbContext : DbContext
    {
        public FishFarmDbContext(DbContextOptions<FishFarmDbContext> options) : base(options) { }
        public DbSet<Cage> Cages { get; set; }
        public DbSet<FishStocking> FishStockings { get; set; }
    }
  
}
