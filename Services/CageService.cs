using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;


namespace Apos_AquaProductManageApp.Services
{
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
            var cage = _db.Cages.Where(c => c.Name == name).FirstOrDefault();
            if (cage != null)
            {
                MessageBox.Show("Cage with this name already exists.");
                return;
            }
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
            else
            {
                MessageBox.Show("Cage not found.");
            }
        }

    }
}
