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

        public List<Cage> GetAvailableCages(DateTime date)
        {
            return _balanceService.GetCages(date);
        }

        public void AddOrUpdateStocking(int cageId, DateTime date, int quantity)
        {
            var existing = _db.FishStockings
                .FirstOrDefault(s => s.CageId == cageId && s.StockingDate == date);

            var balanceBefore = _transferService.CalculateBalance(cageId, date);
            var existingQuantity = existing?.Quantity ?? 0;

            int simulatedBalance = balanceBefore - existingQuantity + quantity;

            if (simulatedBalance < 0)
                throw new InvalidOperationException("Stocking exceeds available balance.");

            if (existing != null)
            {
                existing.Quantity = quantity;
                _db.FishStockings.Update(existing);
            }
            else
            {
                //// Only check if cage is empty when adding new stocking
                //if (!_balanceService.GetEmptyCages(date).Any(c => c.CageId == cageId))
                //    throw new InvalidOperationException("Cage is not empty on this date.");

                var newStocking = new FishStocking
                {
                    CageId = cageId,
                    StockingDate = date,
                    Quantity = quantity
                };
                _db.FishStockings.Add(newStocking);
            }

            _db.SaveChanges();
        }

        public void DeleteStocking(int cageId, DateTime date)
        {
            var existing = _db.FishStockings
                .FirstOrDefault(s => s.CageId == cageId && s.StockingDate == date);

            if (existing == null)
                return; // Nothing to delete

            var balanceBefore = _transferService.CalculateBalance(cageId, date);

            if (balanceBefore < existing.Quantity)
                throw new InvalidOperationException("Deleting this stocking would result in negative stock.");

            _db.FishStockings.Remove(existing);
            _db.SaveChanges();
        }

        public List<SetQuantityView> GetMergedCageStockings(DateTime date)
        {
            var allCages = _db.Cages.Where(c => c.IsActive).ToList();
            var existingStockings = _db.FishStockings.Where(s => s.StockingDate == date).ToList();

            return allCages.Select(c => {
                var stocking = existingStockings.FirstOrDefault(s => s.CageId == c.CageId);
                return new SetQuantityView
                {
                    CageId = c.CageId,
                    CageName = c.Name,
                    Quantity = stocking?.Quantity ?? 0
                };
            }).ToList();
        }
    }
}
