using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;
using Microsoft.EntityFrameworkCore;


namespace Apos_AquaProductManageApp.Services
{
    public class MortalityService
    {
        private readonly FishFarmDbContext _db;
        private readonly StockBalanceService _balanceService;
        private readonly TransferService _transferService;

        public MortalityService(FishFarmDbContext db, StockBalanceService balanceService, TransferService transferService)
        {
            _db = db;
            _balanceService = balanceService;
            _transferService = transferService;
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
            var balanceBefore = _transferService.CalculateBalance(cageId, date);

            if (existing != null)
            {
                int delta = quantity - existing.Quantity;
                if (delta > 0 && delta > balanceBefore)
                    throw new InvalidOperationException("Updated mortality exceeds available stock.");

                existing.Quantity = quantity;
            }
            else
            {
                if (quantity > balanceBefore)
                    throw new InvalidOperationException("Mortality exceeds available stock.");

                _db.Mortalities.Add(new Mortality { CageId = cageId, MortalityDate = date, Quantity = quantity });
            }

            _db.SaveChanges();
        }


        public void UpdateMortality(Mortality mortality)
        {
            var existing = _db.Mortalities.First(m => m.MortalityId == mortality.MortalityId);

            // Temporarily subtract the original value to simulate a "reset"
            var balanceBefore = _transferService.CalculateBalance(mortality.CageId, mortality.MortalityDate) + existing.Quantity;

            if (balanceBefore < mortality.Quantity)
                throw new InvalidOperationException("Updated mortality would cause negative stock balance.");

            _db.Mortalities.Update(mortality);
            _db.SaveChanges();
        }

        public void DeleteMortality(Mortality mortality)
        {

            _db.Mortalities.Remove(mortality);
            _db.SaveChanges();
        }

        public List<Mortality> GetAllMortalities()
        {
            return _db.Mortalities.Include(m => m.Cage).ToList();
        }

        public List<SetQuantityView> GetMergedCageMortalities(DateTime date)
        {
            var allCages = _db.Cages.Where(c => c.IsActive).ToList();
            var mortalities = _db.Mortalities
                .Where(m => m.MortalityDate == date)
                .ToList();

            var merged = from cage in allCages
                         join mort in mortalities on cage.CageId equals mort.CageId into mj
                         from submort in mj.DefaultIfEmpty()
                         select new SetQuantityView
                         {
                             CageId = cage.CageId,
                             CageName = cage.Name,
                             Quantity = submort?.Quantity ?? 0
                         };

            return merged.ToList();
        }


    }


}
