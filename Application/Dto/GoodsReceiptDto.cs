using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    /// <summary>
    /// مدل نمایشی برای رسید انبار (GoodsReceipt) - جهت استفاده در DataGridView و گزارشات
    /// </summary>
    public class GoodsReceiptDto
    {
        /// <summary>
        /// شناسه یکتای رسید (برای استفاده در پشت صحنه، معمولاً مخفی می‌شود)
        /// </summary>
        public int ReceiptId { get; set; }

        /// <summary>
        /// نام محصول (جایگزین ProductId)
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// نام تأمین‌کننده (جایگزین SupplierId)
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// نام انبار مقصد (جایگزین WarehouseId)
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// نام کاربر ثبت‌کننده (جایگزین UserId)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// بارکد اسکن شده (در صورت وجود)
        /// </summary>
        public string ScannedBarcode { get; set; }

        /// <summary>
        /// تعداد ورودی
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// واحد اندازه‌گیری (عدد، کیلوگرم، متر و ...)
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// قیمت واحد (بدون مالیات)
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// قیمت کل (محاسبه شده = تعداد * قیمت واحد)
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// نرخ مالیات (درصد)
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// قیمت نهایی با احتساب مالیات
        /// </summary>
        public decimal NetPrice { get; set; }

        /// <summary>
        /// شماره فاکتور خرید
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// تاریخ فاکتور
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// موقعیت قفسه در انبار
        /// </summary>
        public string ShelfLocation { get; set; }

        /// <summary>
        /// تاریخ ورود کالا به انبار (ReceiptDate)
        /// </summary>
        public DateTime ReceiptDate { get; set; }

        /// <summary>
        /// تاریخ ثبت رکورد در سیستم (CreatedAt)
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// وضعیت رسید (متن فارسی: در انتظار تأیید، تأیید شده، لغو شده)
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// تاریخ انقضای کالا (در صورت وجود)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// شماره دسته (Batch) یا لات
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// توضیحات اضافی
        /// </summary>
        public string Description { get; set; }
    }
}
