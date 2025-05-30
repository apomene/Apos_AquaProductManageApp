

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Apos_AquaProductManageApp.Model
{
    public class Mortality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MortalityId { get; set; }
        public int CageId { get; set; }
        public DateTime MortalityDate { get; set; }
        public int Quantity { get; set; }

        public  Cage? Cage { get; set; }
    }
}
