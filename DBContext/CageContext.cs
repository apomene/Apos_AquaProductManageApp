
using Microsoft.EntityFrameworkCore;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.DBContext
{    
    public class FishFarmDbContext : DbContext
    {
        public FishFarmDbContext(DbContextOptions<FishFarmDbContext> options) : base(options) { }
        public DbSet<Cage> Cages { get; set; }
        

    }

    public class CageService
    {
        private readonly FishFarmDbContext _db;
        public CageService(FishFarmDbContext db) { _db = db; }

        public List<Cage> GetAllCages()
        {
            return _db.Cages.Any() ? _db.Cages.ToList() : new List<Cage>();
        }

        public void AddCage(string name, bool isActive)
        {
            _db.Cages.Add(new Cage { Name = name, IsActive = isActive });
            _db.SaveChanges();
        }

        public void DeleteCage(int id)
        {
            var cage = _db.Cages.Find(id);
            if (cage != null)
            {
                _db.Cages.Remove(cage);
                _db.SaveChanges();
            }
        }

        public void UpdateCage(int id, string name, bool isActive)
        {
            var cage = _db.Cages.Find(id);
            if (cage != null)
            {
                cage.Name = name;
                cage.IsActive = isActive;
                _db.SaveChanges();
            }
        }
    }


}
