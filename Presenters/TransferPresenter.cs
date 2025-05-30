
using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Services;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Presenters
{
        public class TransferPresenter
        {
            private readonly ITransferView _view;
            private readonly TransferService _service;
            private readonly CageService _cageService;

            public TransferPresenter(ITransferView view, TransferService service, CageService cageService)
            {
                _view = view;
                _service = service;
                _cageService = cageService;
                _view.SetPresenter(this);
            }

            public void LoadData(DateTime date)
            {
                _view.DisplayTransfers(_service.GetTransfersByDate(date));
                _view.DisplayCages(_cageService.GetAllCages());
            }

            public void AddTransfer(FishTransfer transfer)
            {
                _service.AddTransfer(transfer);
                LoadData(transfer.TransferDate);
            }
        }
    
}
