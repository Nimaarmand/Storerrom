using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    // ============================== Supplier ==============================
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string EconomicCode { get; set; }
    }
}
