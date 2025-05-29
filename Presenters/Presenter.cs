
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.Presenters
{
    public class Presenter
    {
        public class CagePresenter
        {
            private readonly ICageView _view;
            private List<Cage> _cages = new();
            private int _nextId = 3;

            public CagePresenter(ICageView view) { _view = view; view.SetPresenter(this); LoadCages(); }

            public void LoadCages()
            {
                _cages = new List<Cage> {
                new Cage { Id = 1, Name = "Cage A", IsActive = true },
                new Cage { Id = 2, Name = "Cage B", IsActive = false }
            };
                _view.DisplayCages(_cages);
            }

            public void AddCage(string name, bool isActive)
            {
                _cages.Add(new Cage { Id = _nextId++, Name = name, IsActive = isActive });
                _view.DisplayCages(_cages);
            }

            public void DeleteCage(int id)
            {
                _cages.RemoveAll(c => c.Id == id);
                _view.DisplayCages(_cages);
            }

            public void UpdateCage(int id, string name, bool isActive)
            {
                var cage = _cages.FirstOrDefault(c => c.Id == id);
                if (cage != null)
                {
                    cage.Name = name;
                    cage.IsActive = isActive;
                    _view.DisplayCages(_cages);
                }
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
