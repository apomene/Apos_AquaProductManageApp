

namespace Apos_AquaProductManageApp.Model
{
    public class FishFarmModel
    {
        public class Cage { public int Id; public string Name; public bool IsActive; }
        public class Stocking { public int Id; public int CageId; public DateTime Date; public int Quantity; }
        public class Mortality { public int Id; public int CageId; public DateTime Date; public int Quantity; }
        public class Transfer { public int Id; public int SourceCageId; public DateTime Date; public List<TransferDetail> Details; }
        public class TransferDetail { public int DestinationCageId; public int Quantity; }
        public class StockBalance { public int CageId; public DateTime Date; public int Quantity; }
    }
}
