
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.Presenters
{
    public class StockingPresenter
    {
        private readonly IStockingView _view;
        public StockingPresenter(IStockingView view) { _view = view; view.SetPresenter(this); }
        public void LoadStockings(DateTime date) { _view.DisplayStockings(new List<Stocking>()); }
    }
}
