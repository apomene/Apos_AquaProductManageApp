
using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;


namespace Apos_AquaProductManageApp.Interfaces
{
    public class ViewInterfaces
    {
        public interface ICageView
        {
            void DisplayCages(List<FishFarmModel.Cage> cages);
            void SetPresenter(CagePresenter cagePresenter);
        }
        public interface IStockingView
        {
            void DisplayStockings(List<FishFarmModel.Stocking> stockings);
            void SetPresenter(StockingPresenter stockingPresenter);
        }
        public interface IMortalityView
        {
            void SetPresenter(MortalityPresenter presenter);
            void DisplayMortalities(List<FishFarmModel.Mortality> mortalities);
        }
        public interface ITransferView
        {
            void SetPresenter(TransferPresenter presenter);
            void DisplayTransfers(List<FishFarmModel.Transfer> transfers);
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
