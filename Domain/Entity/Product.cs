using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entity
{
    // ============================== Product ==============================
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; } = Guid.NewGuid();    
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(20)]
        public string BaseUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinStockLevel { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxStockLevel { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Weight { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(50)]
        public string Barcode { get; set; }

        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public int? Number { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

