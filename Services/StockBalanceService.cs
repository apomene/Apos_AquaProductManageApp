using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;


namespace Apos_AquaProductManageApp.Services
{
    public class StockBalanceService
    {
        private readonly FishFarmDbContext _db;

        public StockBalanceService(FishFarmDbContext db)
        {
            _db = db;
        }

        public int GetStockBalance(int cageId, DateTime date)
        {
            var stocked = _db.FishStockings
                .Where(s => s.CageId == cageId && s.StockingDate <= date)
                .Sum(s => (int?)s.Quantity) ?? 0;

            var mortalities = _db.Mortalities
                .Where(m => m.CageId == cageId && m.MortalityDate <= date)
                .Sum(m => (int?)m.Quantity) ?? 0;

            return stocked - mortalities;
        }

        public bool CageHasFishOnDate(int cageId, DateTime date)
        {
            return GetStockBalance(cageId, date) > 0;
        }

        public List<Cage> GetCagesWithFish(DateTime date)
        {
            return _db.Cages
                .ToList()
                .Where(c => CageHasFishOnDate(c.CageId, date))
                .ToList();
        }

        public List<Cage> GetEmptyCages(DateTime date)
        {
            return _db.Cages
                .ToList()
                .Where(c => GetStockBalance(c.CageId, date) == 0)
                .ToList();
        }

        public List<Cage> GetCages(DateTime date)
        {
            return _db.Cages
                .ToList();              
        }
    }

}
