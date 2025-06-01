
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


        public void LoadTransfers(DateTime date)
        {
            var transfers = _service.GetTransfersByDate(date);
            _view.DisplayTransfers(transfers);
        }

        public List<Cage> LoadCages()
        {
            var cages = _service.GetAllCages();
            _view.DisplayCages(cages);
            return cages;
        }

        public void TransferFish(int fromCageId, int toCageId, DateTime date, int quantity)
        {
            _service.TransferFish(fromCageId, toCageId, date, quantity);
        }




    }


}
