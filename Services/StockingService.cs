using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;
using Microsoft.EntityFrameworkCore;


namespace Apos_AquaProductManageApp.Services
{
    public class StockingService
    {
        private readonly FishFarmDbContext _db;
        private readonly StockBalanceService _balanceService;

        public StockingService(FishFarmDbContext db, StockBalanceService balanceService)
        {
            _db = db;
            _balanceService = balanceService;
        }

        public List<FishStocking> GetStockingsByDate(DateTime date)
        {
            return _db.FishStockings
                      .Include(s => s.Cage)
                      .Where(s => s.StockingDate == date)
                      .ToList();
        }

        public List<Cage> GetAvailableCagesForStocking(DateTime date)
        {
            return _balanceService.GetEmptyCages(date);
        }

        public void AddStocking(int cageId, DateTime date, int quantity)
        {
            if (!_balanceService.GetEmptyCages(date).Any(c => c.CageId == cageId))
                throw new InvalidOperationException("Cage is not empty on this date.");

            _db.FishStockings.Add(new FishStocking { CageId = cageId, StockingDate = date, Quantity = quantity });
            _db.SaveChanges();
        }
    }


}
