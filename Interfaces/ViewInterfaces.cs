
using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;


namespace Apos_AquaProductManageApp.Interfaces
{
    public class ViewInterfaces
    {
        public interface ICageView
        {
            void SetPresenter(Presenter.CagePresenter presenter);
            void DisplayCages(List<FishFarmModel.Cage> cages);
        }
        public interface IStockingView
        {
            void SetPresenter(Presenter.StockingPresenter presenter);
            void DisplayStockings(List<FishFarmModel.Stocking> stockings);
        }
        public interface IMortalityView
        {
            void SetPresenter(Presenter.MortalityPresenter presenter);
            void DisplayMortalities(List<FishFarmModel.Mortality> mortalities);
        }
        public interface ITransferView
        {
            void SetPresenter(Presenter.TransferPresenter presenter);
            void DisplayTransfers(List<FishFarmModel.Transfer> transfers);
        }
        public interface IBalanceView
        {
            void SetPresenter(Presenter.BalancePresenter presenter);
            void DisplayBalances(List<FishFarmModel.StockBalance> balances);
        }
        public interface IMortalityPivotView
        {
            void SetPresenter(Presenter.MortalityPivotPresenter presenter);
            void DisplayPivot(Dictionary<string, int> pivotData);
        }
    }
}
