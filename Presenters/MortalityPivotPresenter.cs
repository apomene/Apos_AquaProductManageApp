
using Apos_AquaProductManageApp.Services;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Presenters
{
    public class MortalityPivotPresenter
    {
        private readonly IMortalityPivotView _view;
        private readonly TransferService _service;

        public MortalityPivotPresenter(IMortalityPivotView view, TransferService service)
        {
            _view = view;
            _service = service;
            _view.SetPresenter(this);
        }

        public void LoadPivot()
        {
            var pivotData = _service.GetMortalityPivot();
            _view.DisplayPivot(pivotData);
        }
    }

}
