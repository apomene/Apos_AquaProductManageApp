using Apos_AquaProductManageApp.Model;
using Microsoft.EntityFrameworkCore;


namespace Apos_AquaProductManageApp.DBContext
{
    public class StockingService
    {
        private readonly FishFarmDbContext _db;
        public StockingService(FishFarmDbContext db) { _db = db; }

        public List<FishStocking> GetStockingsByDate(DateTime date)
        {
            return _db.FishStockings.Include(s => s.Cage).Where(s => s.StockingDate == date).ToList();
        }

        public List<Cage> GetAvailableCagesForStocking(DateTime date)
        {
            // First, retrieve all cages that are active
            var allCages = _db.Cages.Where(c => c.IsActive).ToList();

            // Then, find cages that have already been stocked on the selected date
            var stockedCageIds = _db.FishStockings
                .Where(s => s.StockingDate == date)
                .Select(s => s.CageId)
                .Distinct()
                .ToList();

            // Return cages that are not in the stocked list
            return allCages.Where(c => !stockedCageIds.Contains(c.CageId)).ToList();
        }


        public void AddStocking(int cageId, DateTime date, int quantity)
        {
            _db.FishStockings.Add(new FishStocking { CageId = cageId, StockingDate = date, Quantity = quantity });
            _db.SaveChanges();
        }
    }

}
