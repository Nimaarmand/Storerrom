using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    // ============================== GoodsIssue ==============================
    /// <summary>
    /// کلاس خروج کالا
    /// </summary>
    /// <summary>
    /// موجودیت حواله انبار (خروج کالا از انبار)
    /// </summary>
    public class GoodsIssue
    {
        /// <summary>
        /// شناسه یکتای حواله
        /// </summary>
        [Key]
        public int IssueId { get; set; }

        /// <summary>
        /// شناسه محصول خروجی
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// رابطه ارجاع به محصول
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        /// <summary>
        /// مقدار خروجی (تعداد یا وزن)
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// واحد اندازه‌گیری (مثل عدد، کیلوگرم، متر)
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Unit { get; set; }

        /// <summary>
        /// قیمت فروش واحد در زمان خروج (در صورت نیاز)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitSellingPrice { get; set; }

        /// <summary>
        /// نوع حواله (فروش، مصرف داخلی، برگشت از مشتری و...)
        /// </summary>
        [Required]
        public IssueType Type { get; set; }

        /// <summary>
        /// شناسه مشتری (در صورت فروش کالا به مشتری)
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// رابطه ارجاع به مشتری
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// شماره فاکتور مرتبط (در صورت وجود)
        /// </summary>
        [MaxLength(50)]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// تاریخ فاکتور
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        /// <summary>
        /// شناسه انباری که کالا از آن خارج می‌شود
        /// </summary>
        public int? WarehouseId { get; set; }

        /// <summary>
        /// رابطه ارجاع به انبار
        /// </summary>
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        /// <summary>
        /// موقعیت قفسه در انبار (در صورت نیاز)
        /// </summary>
        [MaxLength(100)]
        public string? ShelfLocation { get; set; }

        /// <summary>
        /// تاریخ صدور حواله (مقدار پیش‌فرض: امروز)
        /// </summary>
        public DateTime? IssueDate { get; set; } 

        /// <summary>
        /// تاریخ ایجاد رکورد در سیستم (مقدار پیش‌فرض: زمان حال)
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// شناسه کاربری که حواله را ثبت کرده است
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// کاربر تایید کننده حواله خروج
        /// </summary>
        public string? ApprovedByUserId { get; set; }
        /// <summary>
        /// کاربر ثبت کننده حواله خروج
        /// </summary>

         [MaxLength(100)]
        public string? UserFullName { get; set; }
        /// <summary>
        /// کاربر تایید کننده حواله 
        /// </summary>
        [MaxLength(100)]
        public string? ApprovedByFullName { get; set; }

        /// <summary>
        /// وضعیت حواله (پیش‌نویس، تایید شده، لغو شده و...) - مقدار پیش‌فرض 0
        /// </summary>
        [Required]
        public byte Status { get; set; } 

        /// <summary>
        /// شماره سری یا شماره دسته کالا
        /// </summary>
        [MaxLength(50)]
        public string? BatchNumber { get; set; }

        /// <summary>
        /// توضیحات اضافی درباره حواله
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }
    }
}

