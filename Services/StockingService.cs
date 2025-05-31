using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;
using Microsoft.EntityFrameworkCore;


namespace Apos_AquaProductManageApp.Services
{
    public class StockingService
    {
        private readonly FishFarmDbContext _db;
        private readonly StockBalanceService _balanceService;
        private readonly TransferService _transferService;

        public StockingService(FishFarmDbContext db, StockBalanceService balanceService, TransferService transferService)
        {
            _db = db;
            _balanceService = balanceService;
            _transferService = transferService;
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

        public void UpdateStocking(FishStocking stocking)
        {
            var currentStocking = _db.FishStockings.First(s => s.StockingId == stocking.StockingId);

            var balanceBefore = _transferService.CalculateBalance(stocking.CageId, stocking.StockingDate);

            if (balanceBefore > currentStocking.Quantity)
                throw new InvalidOperationException("Deleting this stocking would result in negative stock.");
            _db.FishStockings.Update(stocking);
            _db.SaveChanges();
        }

        public void DeleteStocking(FishStocking stocking)
        {
            var currentStocking = _db.FishStockings.First(s => s.StockingId == stocking.StockingId);

            var balanceBefore = _transferService.CalculateBalance(stocking.CageId, stocking.StockingDate);

            if (balanceBefore < currentStocking.Quantity)
                throw new InvalidOperationException("Deleting this stocking would result in negative stock.");

            _db.FishStockings.Remove(stocking);
            _db.SaveChanges();
        }

    }


}
