using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    // ============================== GoodsReceipt ==============================
    public class GoodsReceipt
    {
        [Key]
        public int ReceiptId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [MaxLength(50)]
        public string ScannedBarcode { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxRate { get; set; } = 0;

        [NotMapped]
        public decimal NetPrice => TotalPrice * (1 + TaxRate / 100);

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [MaxLength(100)]
        public string ShelfLocation { get; set; }

        [Required]
        public DateTime ReceiptDate { get; set; } = DateTime.Today;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public byte Status { get; set; } = 0;

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(50)]
        public string BatchNumber { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }
    }
}

