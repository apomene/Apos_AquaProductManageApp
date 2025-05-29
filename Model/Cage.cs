
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Apos_AquaProductManageApp.Model
{
    public class Cage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CageId { get; set; }

        public required string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
