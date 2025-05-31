using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;
using Microsoft.EntityFrameworkCore;

namespace Apos_AquaProductManageApp.Services
{
    public class TransferService
    {
        private readonly FishFarmDbContext _context;
        private readonly StockBalanceService _balanceService;

        public TransferService(FishFarmDbContext context, StockBalanceService balanceService)
        {
            _context = context;
            _balanceService = balanceService;
        }

        public List<FishTransfer> GetTransfersByDate(DateTime date)
        {
            return _context.FishTransfers
                .Where(t => t.TransferDate.Date == date.Date)
                .Include(t => t.FromCage)
            .Include(t => t.ToCage)
                .ToList();
        }

        public void AddTransfer(FishTransfer transfer)
        {
            int fromStock = _balanceService.GetStockBalance(transfer.FromCageId, transfer.TransferDate);
            if (transfer.FromCageId == transfer.ToCageId)
                throw new InvalidOperationException("Cannot transfer fish to the same cage.");
            if (transfer.Quantity <= 0)
                throw new InvalidOperationException("Transfer quantity must be greater than zero.");

            if (transfer.Quantity > fromStock)
                throw new InvalidOperationException("Transfer quantity exceeds available stock.");

            var fromCage = _context.Cages.FirstOrDefault(c => c.CageId == transfer.FromCageId)
                ?? throw new InvalidOperationException($"From cage with ID {transfer.FromCageId} not found.");

            var toCage = _context.Cages.FirstOrDefault(c => c.CageId == transfer.ToCageId)
                ?? throw new InvalidOperationException($"To cage with ID {transfer.ToCageId} not found.");

            var balance = CalculateBalance(transfer.FromCageId, transfer.TransferDate);

            if (balance < transfer.Quantity)
                throw new InvalidOperationException("Transfer would result in negative stock balance.");

            _context.FishTransfers.Add(transfer);
            _context.SaveChanges();
        }

        public int CalculateBalance(int cageId, DateTime date)
        {
            var stocked = _context.FishStockings
                .Where(s => s.CageId == cageId && s.StockingDate <= date)
                .Sum(s => (int?)s.Quantity) ?? 0;

            var dead = _context.Mortalities
                .Where(m => m.CageId == cageId && m.MortalityDate <= date)
                .Sum(m => (int?)m.Quantity) ?? 0;

            var transfersOut = _context.FishTransfers
                .Where(t => t.FromCageId == cageId && t.TransferDate <= date)
                .Sum(t => (int?)t.Quantity) ?? 0;

            var transfersIn = _context.FishTransfers
                .Where(t => t.ToCageId == cageId && t.TransferDate <= date)
                .Sum(t => (int?)t.Quantity) ?? 0;

            return stocked - dead - transfersOut + transfersIn;
        }


        public List<StockBalance> GetDailyBalances(DateTime date)
        {
            var cages = _context.Cages.ToList();

            return cages.Select(cage => new StockBalance
            {
                Cage = cage,
                Balance = CalculateBalance(cage.CageId, date)
            }).ToList();
        }

           public List<MortalityPivot> GetMortalityPivot(List<MortalityDimension> dimensions)
        {
            var mortalityData = _context.Mortalities
                .Include(m => m.Cage)  
                .ToList();             

            var grouped = mortalityData.GroupBy(m =>
            {
                var key = new MortalityPivotKey();
                if (dimensions.Contains(MortalityDimension.Cage))
                    key.CageId = m.CageId;
                if (dimensions.Contains(MortalityDimension.Year))
                    key.Year = m.MortalityDate.Year;
                if (dimensions.Contains(MortalityDimension.Month))
                    key.Month = m.MortalityDate.Month;
                return key;
            });

            return grouped.Select(g => new MortalityPivot
            {
                CageId = g.Key.CageId,
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalMortalities = g.Sum(m => m.Quantity)
            }).ToList();
        }

    }

}
