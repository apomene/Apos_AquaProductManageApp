using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;

namespace Apos_AquaProductManageApp.Services
{
    public class TransferService
    {
        private readonly FishFarmDbContext _db;
        private readonly StockBalanceService _balanceService;

        public TransferService(FishFarmDbContext db, StockBalanceService balanceService)
        {
            _db = db;
            _balanceService = balanceService;
        }

        public void TransferFish(int fromCageId, int toCageId, DateTime date, int quantity)
        {
            int fromStock = _balanceService.GetStockBalance(fromCageId, date);
            if (quantity > fromStock)
                throw new InvalidOperationException("Transfer quantity exceeds available stock.");

            _db.FishTransfers.Add(new FishTransfer
            {
                FromCageId = fromCageId,
                FromCage = _db.Cages.Where(c => c.CageId == fromCageId).FirstOrDefault(),
                ToCageId = toCageId,
                ToCage = _db.Cages.Where(c => c.CageId == toCageId).FirstOrDefault(),
                TransferDate = date,
                Quantity = quantity
            });

            _db.SaveChanges();
        }
    }

}
