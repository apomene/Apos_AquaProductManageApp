

using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.Presenters
{
    public class BalancePresenter
    {
        private readonly IBalanceView _view;
        public BalancePresenter(IBalanceView view) { _view = view; view.SetPresenter(this); }
        public void LoadBalances(DateTime date) { _view.DisplayBalances(new List<StockBalance>()); }
    }
}
