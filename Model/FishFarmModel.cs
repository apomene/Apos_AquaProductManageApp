
using System.ComponentModel.DataAnnotations;

namespace Apos_AquaProductManageApp.Model
{
    public class FishFarmModel
    {
        public class Cage
        {
            [Key]
            public int CageId { get; set; }

            public string Name { get; set; }

            public bool IsActive { get; set; }
        }

        public class Stocking { public int Id; public int CageId; public DateTime Date; public int Quantity; }
        public class Mortality { public int Id; public int CageId; public DateTime Date; public int Quantity; }
        public class Transfer { public int Id; public int SourceCageId; public DateTime Date; public List<TransferDetail> Details; }
        public class TransferDetail { public int DestinationCageId; public int Quantity; }
        public class StockBalance { public int CageId; public DateTime Date; public int Quantity; }
    }
}
