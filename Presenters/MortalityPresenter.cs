using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Services;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Presenters
{
    public class MortalityPresenter
    {
        private readonly IMortalityView _view;
        private readonly MortalityService _service;

        public MortalityPresenter(IMortalityView view, MortalityService service)
        {
            _view = view;
            _service = service;
            _view.SetPresenter(this);
        }

        public void LoadMortalityData(DateTime date)
        {
            var merged = _service.GetMergedCageMortalities(date);
            _view.DisplayMortalityData(merged);
        }

        public void AddOrUpdateMortality(int cageId, DateTime date, int quantity)
        {
            _service.AddOrUpdateMortality(cageId, date, quantity);
        }

        public void DeleteMortality(Mortality mortality)
        {
            _service.DeleteMortality(mortality);
        }     
    }
}
