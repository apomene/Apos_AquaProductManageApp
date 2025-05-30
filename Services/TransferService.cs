using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

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

        public void TransferFish(int fromCageId, int toCageId, DateTime date, int quantity)
        {
            int fromStock = _balanceService.GetStockBalance(fromCageId, date);
            if (quantity > fromStock)
                throw new InvalidOperationException("Transfer quantity exceeds available stock.");

            _context.FishTransfers.Add(new FishTransfer
            {
                FromCageId = fromCageId,
                FromCage = _context.Cages.Where(c => c.CageId == fromCageId).FirstOrDefault(),
                ToCageId = toCageId,
                ToCage = _context.Cages.Where(c => c.CageId == toCageId).FirstOrDefault(),
                TransferDate = date,
                Quantity = quantity
            });

            _context.SaveChanges();
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
    }

}
