
using Apos_AquaProductManageApp.Model;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.Presenters
{
    public class TransferPresenter
    {
        private readonly ITransferView _view;
        public TransferPresenter(ITransferView view) { _view = view; view.SetPresenter(this); }
        public void LoadTransfers(DateTime date) { _view.DisplayTransfers(new List<FishTransfer>()); }
    }
}
