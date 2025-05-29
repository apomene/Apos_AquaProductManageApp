
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apos_AquaProductManageApp.Model
{
    public class FishFarmModel
    {
            
         public class TransferDetail { public int DestinationCageId; public int Quantity; }
        public class StockBalance { public int CageId; public DateTime Date; public int Quantity; }
    }
}
