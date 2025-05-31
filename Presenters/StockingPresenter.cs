using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Services;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;


namespace Apos_AquaProductManageApp.Presenters
{
    public class StockingPresenter
    {
        private readonly IStockingView _view;
        private readonly StockingService _service;

        public StockingPresenter(IStockingView view, StockingService service)
        {
            _view = view;
            _service = service;
            _view.SetPresenter(this);

            LoadStockingData(DateTime.Today); // Initialize with today's data
        }

        public void LoadStockingData(DateTime selectedDate)
        {
            var availableCages = _service.GetAvailableCagesForStocking(selectedDate);
            var existingStockings = _service.GetStockingsByDate(selectedDate);

            _view.DisplayAvailableCages(availableCages);
            _view.DisplayStockings(existingStockings);
        }

        public void AddStocking(int cageId, DateTime date, int quantity)
        {
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _service.AddStocking(cageId, date, quantity);
                LoadStockingData(date);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add stocking: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateStocking(FishStocking stocking)
        {
            _service.UpdateStocking(stocking);
        }

        public void DeleteStocking(FishStocking stocking)
        {
            _service.DeleteStocking(stocking);
        }

    }

}
