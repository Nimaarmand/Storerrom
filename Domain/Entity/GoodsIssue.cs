using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    // ============================== GoodsIssue ==============================
    /// <summary>
    /// کلاس خروج کالا
    /// </summary>
    public class GoodsIssue
    {
        [Key]
        public int IssueId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitSellingPrice { get; set; }

        [Required]
        public IssueType Type { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [MaxLength(50)]
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        public int? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        [MaxLength(100)]
        public string ShelfLocation { get; set; }

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.Today;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public byte Status { get; set; } = 0;

        [MaxLength(50)]
        public string BatchNumber { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}

