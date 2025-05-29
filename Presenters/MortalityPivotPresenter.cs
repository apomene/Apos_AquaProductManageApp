
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Presenters
{
    public class MortalityPivotPresenter
    {
        private readonly IMortalityPivotView _view;
        public MortalityPivotPresenter(IMortalityPivotView view) { _view = view; view.SetPresenter(this); }
        public void LoadPivot() { _view.DisplayPivot(new Dictionary<string, int>()); }
    }
}
