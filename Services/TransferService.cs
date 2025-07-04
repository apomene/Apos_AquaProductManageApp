﻿using Apos_AquaProductManageApp.DBContext;
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
                      .Include(t => t.FromCage)
                      .Include(t => t.ToCage)
                      .Where(t => t.TransferDate.Date == date.Date)
                      .ToList();
        }

        public List<Cage> GetAllCages()
        {
            return _context.Cages .Where(c => c.IsActive).OrderBy(c => c.Name).ToList();
        }

        public void TransferFish(int fromCageId, int toCageId, DateTime date, int quantity)
        {
            ValidateTransfer(fromCageId, toCageId, date, quantity);

            var fromCage = _context.Cages.First(c => c.CageId == fromCageId);
            var toCage = _context.Cages.First(c => c.CageId == toCageId);

            var fromStocking = _context.FishStockings
                .FirstOrDefault(fs => fs.CageId == fromCageId && fs.StockingDate.Date == date.Date);

            var toStocking = _context.FishStockings
                .FirstOrDefault(fs => fs.CageId == toCageId && fs.StockingDate.Date == date.Date);
        
           
            if (toStocking != null)
            {
                toStocking.Quantity += quantity;
                _context.FishStockings.Update(toStocking);
            }
            if (fromStocking != null)
            {
                fromStocking!.Quantity -= quantity;
                _context.FishStockings.Update(fromStocking);
            }


            var transfer = new FishTransfer
            {
                FromCageId = fromCageId,
                ToCageId = toCageId,
                TransferDate = date.Date,
                Quantity = quantity,
                FromCage = fromCage,
                ToCage = toCage
            };

            _context.FishTransfers.Add(transfer);
            _context.SaveChanges();
        }

        private void ValidateTransfer(int fromCageId, int toCageId, DateTime date, int quantity)
        {
            if (fromCageId == toCageId)
                throw new InvalidOperationException("Cannot transfer fish to the same cage.");

            if (quantity <= 0)
                throw new InvalidOperationException("Transfer quantity must be greater than zero.");

            var fromCage = _context.Cages.FirstOrDefault(c => c.CageId == fromCageId)
                ?? throw new InvalidOperationException($"From cage with ID {fromCageId} not found.");

            var toCage = _context.Cages.FirstOrDefault(c => c.CageId == toCageId)
                ?? throw new InvalidOperationException($"To cage with ID {toCageId} not found.");

            int fromStock = _balanceService.GetStockBalance(fromCageId, date);
            if (quantity > fromStock)
                throw new InvalidOperationException("Transfer quantity exceeds available stock.");

            int balance = CalculateBalance(fromCageId, date);
            if (balance < quantity)
                throw new InvalidOperationException("Transfer would result in negative stock balance.");

            var fromStocking = _context.FishStockings
                .FirstOrDefault(fs => fs.CageId == fromCageId && fs.StockingDate.Date == date.Date)
                ?? throw new InvalidOperationException($"Stocking record for From cage ID {fromCageId} on {date:d} not found.");

            if (fromStocking.Quantity < quantity)
                throw new InvalidOperationException("Not enough fish in source cage stocking to transfer.");
        }

        public int CalculateBalance(int cageId, DateTime date)
        {
            var stocked = _context.FishStockings
                .Where(s => s.CageId == cageId && s.StockingDate == date)
                .Sum(s => (int?)s.Quantity) ?? 0;

            var dead = _context.Mortalities
                .Where(m => m.CageId == cageId && m.MortalityDate == date)
                .Sum(m => (int?)m.Quantity) ?? 0;

            return stocked - dead;
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
