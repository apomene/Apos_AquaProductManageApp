
using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Services;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Presenters
{
    public class BalancePresenter
    {
        private readonly IBalanceView _view;
        private readonly TransferService _service;

        public BalancePresenter(IBalanceView view, TransferService service)
        {
            _view = view;
            _service = service;
            _view.SetPresenter(this);
        }

        public void LoadBalances(DateTime date)
        {
            var balances = _service.GetDailyBalances(date);
            _view.DisplayBalances(balances);
        }
    }

}
