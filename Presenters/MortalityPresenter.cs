using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Services;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Presenters
{
    public class MortalityPresenter
    {
        private readonly IMortalityView _view;
        private readonly MortalityService _mortalityService;
        private readonly StockBalanceService _balanceService;

        public MortalityPresenter(IMortalityView view, MortalityService mortalityService, StockBalanceService balanceService)
        {
            _view = view;
            _mortalityService = mortalityService;
            _balanceService = balanceService;
            _view.SetPresenter(this);
        }

        public void LoadData(DateTime date)
        {
            var cages = _mortalityService.GetCagesEligibleForMortality(date);
            var mortalities = _mortalityService.GetMortalitiesByDate(date);

            _view.DisplayEligibleCages(cages);
            _view.DisplayMortalities(mortalities);
        }

        public void AddOrUpdateMortality(int cageId, DateTime date, int quantity)
        {
           
            _mortalityService.AddOrUpdateMortality(cageId, date, quantity);
            LoadData(date);
        }

        public void UpdateMortality(Mortality mortality)
        {
            _mortalityService.UpdateMortality(mortality);
        }

        public void DeleteMortality(Mortality mortality)
        {
            _mortalityService.DeleteMortality(mortality);
        }
      
    }

}
