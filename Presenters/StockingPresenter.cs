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
            var merged = _service.GetMergedCageStockings(selectedDate);
            _view.DisplayStockings(merged);
        }


        public void AddOrUpdateStocking(int cageId, DateTime date, int quantity)
        {
            _service.AddOrUpdateStocking(cageId, date, quantity);
        }

        public void DeleteStocking(int cageId, DateTime date)
        {
            _service.DeleteStocking(cageId, date);
        }

        public List<SetQuantityView> GetMergedCageStockings(DateTime date)
        {
            return _service.GetMergedCageStockings(date);
        }




    }

}
