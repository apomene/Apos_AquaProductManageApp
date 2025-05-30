using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;
using Microsoft.EntityFrameworkCore;


namespace Apos_AquaProductManageApp.Services
{
    public class MortalityService
    {
        private readonly FishFarmDbContext _db;
        private readonly StockBalanceService _balanceService;

        public MortalityService(FishFarmDbContext db, StockBalanceService balanceService)
        {
            _db = db;
            _balanceService = balanceService;
        }

        public List<Cage> GetCagesEligibleForMortality(DateTime date)
        {
            return _balanceService.GetCagesWithFish(date);
        }

        public void AddMortality(int cageId, DateTime date, int quantity)
        {
            int available = _balanceService.GetStockBalance(cageId, date);
            if (quantity > available)
                throw new InvalidOperationException("Mortality exceeds available stock.");

            _db.Mortalities.Add(new Mortality { CageId = cageId, MortalityDate = date, Quantity = quantity });
            _db.SaveChanges();
        }

        public List<Mortality> GetMortalitiesByDate(DateTime date)
        {
            return _db.Mortalities.Include(m => m.Cage).Where(m => m.MortalityDate == date).ToList();
        }

        public void AddOrUpdateMortality(int cageId, DateTime date, int quantity)
        {
            var existing = _db.Mortalities.FirstOrDefault(m => m.CageId == cageId && m.MortalityDate == date);

            if (existing != null)
            {
                existing.Quantity = quantity;
            }
            else
            {
                _db.Mortalities.Add(new Mortality { CageId = cageId, MortalityDate = date, Quantity = quantity });
            }

            _db.SaveChanges();
        }

    }


}
