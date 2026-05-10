using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entity
{
    // ============================== Customer ==============================
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }
        public string MarketName { get; set; }
        public DateTime Creationdate { get; set; } = DateTime.Now;
    }
}

