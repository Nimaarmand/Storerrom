using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    // ============================== GoodsReceipt ==============================
    /// <summary>
    /// کلاس ورود کالا
    /// </summary>
    public class GoodsReceipt
    {
        [Key]
        public int ReceiptId { get; set; } // شناسه یکتای ورودی کالا

        [Required]
        public Guid ProductId { get; set; } // شناسه کالای ورودی
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } // شیء کالای مرتبط

        [MaxLength(50)]
        public string ScannedBarcode { get; set; } // بارکد اسکن شده (در صورت وجود)

        [Required]
        public decimal Quantity { get; set; } // تعداد یا مقدار ورودی

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; } // واحد اندازه‌گیری (عدد، کیلوگرم، جعبه و ...)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // قیمت واحد (بدون مالیات)

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice; // قیمت کل (محاسبه شده - در دیتابیس ذخیره نمی‌شود)

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxRate { get; set; } = 0; // نرخ مالیات (درصد)

        [NotMapped]
        public decimal NetPrice => TotalPrice * (1 + TaxRate / 100); // قیمت نهایی با احتساب مالیات (محاسبه شده)

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; } // شماره فاکتور خرید

        [Required]
        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; } // تاریخ فاکتور

        [MaxLength(100)]
        public string ShelfLocation { get; set; } // موقعیت قفسه در انبار

        [Required]
        public DateTime ReceiptDate { get; set; } = DateTime.Today; // تاریخ ورود کالا به انبار

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // تاریخ ثبت رکورد در سیستم

        public string? UserId { get; set; } // شناسه کاربر ثبت‌کننده
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } // شیء کاربر مرتبط

        [Required]
        public byte Status { get; set; } = 0; // وضعیت ورودی (مثلاً ۰ = در انتظار تأیید، ۱ = تأیید شده)

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; } // تاریخ انقضای کالا (در صورت وجود)

        [MaxLength(50)]
        public string BatchNumber { get; set; } // شماره دسته (Batch) یا لات

        [MaxLength(500)]
        public string Description { get; set; } // توضیحات اضافی

        [Required]
        public int SupplierId { get; set; } // شناسه تأمین‌کننده
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; } // شیء تأمین‌کننده مرتبط

        public int? WarehouseId { get; set; } // شناسه انبار مقصد
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; } // شیء انبار مرتبط
    }
}

