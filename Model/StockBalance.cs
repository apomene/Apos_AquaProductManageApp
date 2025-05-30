
using System.ComponentModel.DataAnnotations.Schema;

namespace Apos_AquaProductManageApp.Model
{
    public class StockBalance
    {
        public Cage Cage { get; set; } = null!;

        // Not mapped to DB, just for display
        [NotMapped]
        public string CageName => Cage?.Name ?? "Unknown";
        public int Balance { get; set; }
    }
}
