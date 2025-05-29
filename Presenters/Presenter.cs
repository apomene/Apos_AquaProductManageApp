
using Apos_AquaProductManageApp.DBContext;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.Presenters
{
    public class Presenter
    {
        public class CagePresenter
        {
            private readonly ICageView _view;
            private readonly CageService _service;

            public CagePresenter(ICageView view, CageService service)
            {
                _view = view;
                _service = service;
                view.SetPresenter(this);
                LoadCages();
            }

            public void LoadCages()
            {
                var cages = _service.GetAllCages();
                _view.DisplayCages(cages);
            }

            public void AddCage(string name, bool isActive)
            {
                _service.AddCage(name, isActive);
                LoadCages();
            }

            public void DeleteCage(int id)
            {
                _service.DeleteCage(id);
                LoadCages();
            }

            public void UpdateCage(int id, string name, bool isActive)
            {
                _service.UpdateCage(id, name, isActive);
                LoadCages();
            }
        }


        public class StockingPresenter
        {
            private readonly IStockingView _view;
            public StockingPresenter(IStockingView view) { _view = view; view.SetPresenter(this); }
            public void LoadStockings(DateTime date) { _view.DisplayStockings(new List<Stocking>()); }
        }

        public class MortalityPresenter
        {
            private readonly IMortalityView _view;
            public MortalityPresenter(IMortalityView view) { _view = view; view.SetPresenter(this); }
            public void LoadMortalities(DateTime date) { _view.DisplayMortalities(new List<Mortality>()); }
        }

        public class TransferPresenter
        {
            private readonly ITransferView _view;
            public TransferPresenter(ITransferView view) { _view = view; view.SetPresenter(this); }
            public void LoadTransfers(DateTime date) { _view.DisplayTransfers(new List<Transfer>()); }
        }

        public class BalancePresenter
        {
            private readonly IBalanceView _view;
            public BalancePresenter(IBalanceView view) { _view = view; view.SetPresenter(this); }
            public void LoadBalances(DateTime date) { _view.DisplayBalances(new List<StockBalance>()); }
        }

        public class MortalityPivotPresenter
        {
            private readonly IMortalityPivotView _view;
            public MortalityPivotPresenter(IMortalityPivotView view) { _view = view; view.SetPresenter(this); }
            public void LoadPivot() { _view.DisplayPivot(new Dictionary<string, int>()); }
        }
    }
}
