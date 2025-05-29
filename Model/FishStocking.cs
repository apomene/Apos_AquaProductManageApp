

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apos_AquaProductManageApp.Model
{
  
    public class FishStocking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockingId { get; set; }
        public int CageId { get; set; }
        public DateTime StockingDate  { get; set; } 
        public int Quantity { get; set; }
        // Navigation property
        public virtual Cage Cage { get; set; }

        // Not mapped to DB, just for display
        [NotMapped]
        public string CageName => Cage?.Name;
    }
}
