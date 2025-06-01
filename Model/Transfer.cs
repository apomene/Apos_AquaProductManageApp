
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Apos_AquaProductManageApp.Model
{
    public class FishTransfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransferId { get; set; }
        public int FromCageId { get; set; }
        public int ToCageId { get; set; }
        public DateTime TransferDate { get; set; }
        public int Quantity { get; set; }

        public  Cage? FromCage { get; set; }
        public  Cage? ToCage { get; set; }

        // Not mapped to DB, just for display
        [NotMapped]
        public string FromCageName => FromCage?.Name ?? "Unknown";
        // Not mapped to DB, just for display
        [NotMapped]
        public string ToCageName => ToCage?.Name ?? "Unknown";
    }

}
