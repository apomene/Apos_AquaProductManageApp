using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using System.Security.Cryptography.Xml;
using static Apos_AquaProductManageApp.Services.TransferService;


namespace Apos_AquaProductManageApp.Interfaces
{
    public class ViewInterfaces
    {
        public interface ICageView
        {
            void DisplayCages(List<Cage> cages);
            void SetPresenter(CagePresenter cagePresenter);
        }
        public interface IStockingView { void SetPresenter(StockingPresenter presenter); void DisplayAvailableCages(List<Cage> cages); void DisplayStockings(List<SetQuantityView> stockings); }


        public interface IMortalityView
        {
            void SetPresenter(MortalityPresenter presenter);
            void DisplayMortalityData(List<SetQuantityView> cageMortalityViews);
        }



        public interface ITransferView
        {
            void SetPresenter(TransferPresenter presenter);
            void DisplayTransfers(List<FishTransfer> transfers);
            void DisplayCages(List<Cage> cages);
            DateTime SelectedDate { get; }
        }
        public interface IBalanceView
        {
            void SetPresenter(BalancePresenter presenter);
            void DisplayBalances(List<StockBalance> balances);
        }
        public interface IMortalityPivotView
        {
            void SetPresenter(MortalityPivotPresenter presenter);
            List<MortalityDimension> GetSelectedDimensions();
            void DisplayMortalityPivot(List<MortalityPivot> pivot);

        }
    }
}

