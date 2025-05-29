
using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Model;
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
            view.SetPresenter(this);
        }

        public void LoadStockingData(DateTime date)
        {
            _view.DisplayAvailableCages(_service.GetAvailableCagesForStocking(date));
            _view.DisplayStockings(_service.GetStockingsByDate(date));
        }

        public void AddStocking(int cageId, DateTime date, int quantity)
        {
            _service.AddStocking(cageId, date, quantity);
            LoadStockingData(date);
        }
    }
}
