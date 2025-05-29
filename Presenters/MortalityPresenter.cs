
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.Presenters
{
    public class MortalityPresenter
    {
        private readonly IMortalityView _view;
        public MortalityPresenter(IMortalityView view) { _view = view; view.SetPresenter(this); }
        public void LoadMortalities(DateTime date) { _view.DisplayMortalities(new List<Mortality>()); }
    }
}
