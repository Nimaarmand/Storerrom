using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    // ============================== Warehouse ==============================
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Location { get; set; }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Number { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
    }
}
