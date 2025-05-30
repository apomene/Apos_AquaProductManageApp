using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.Interfaces
{
    public class ViewInterfaces
    {
        public interface ICageView
        {
            void DisplayCages(List<Cage> cages);
            void SetPresenter(CagePresenter cagePresenter);
        }
        public interface IStockingView { void SetPresenter(StockingPresenter presenter); void DisplayAvailableCages(List<Cage> cages); void DisplayStockings(List<FishStocking> stockings); }

        public interface IMortalityView
        {
            void SetPresenter(MortalityPresenter presenter);
            void DisplayEligibleCages(List<Cage> cages);
            void DisplayMortalities(List<Mortality> mortalities);
            DateTime GetSelectedDate();
        }

        public interface ITransferView
        {
            void SetPresenter(TransferPresenter presenter);
            void DisplayTransfers(List<FishTransfer> transfers);
        }
        public interface IBalanceView
        {
            void SetPresenter(BalancePresenter presenter);
            void DisplayBalances(List<FishFarmModel.StockBalance> balances);
        }
        public interface IMortalityPivotView
        {
            void SetPresenter(MortalityPivotPresenter presenter);
            void DisplayPivot(Dictionary<string, int> pivotData);
        }
    }
}

