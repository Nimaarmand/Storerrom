using Application.Features.Definition.Context;
using Application.Features.Implementation.GenericRepository_Service;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Implementation.GoodsReceipt_Service
{
    public class GoodsReceiptService : GenericRepository<GoodsReceipt>
    {
        public GoodsReceiptService(IApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// دریافت رسید بر اساس شماره فاکتور
        /// </summary>
        public async Task<GoodsReceipt> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber))
                return null;
            return await _dbSet.FirstOrDefaultAsync(r => r.InvoiceNumber == invoiceNumber);
        }

        /// <summary>
        /// دریافت همه رسیدهای یک محصول خاص
        /// </summary>
        public async Task<IEnumerable<GoodsReceipt>> GetByProductIdAsync(Guid productId)
        {
            return await _dbSet
                .Where(r => r.ProductId == productId)
                .Include(r => r.Product)
                .Include(r => r.Supplier)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت رسیدهای یک تأمین‌کننده
        /// </summary>
        public async Task<IEnumerable<GoodsReceipt>> GetBySupplierIdAsync(int supplierId)
        {
            return await _dbSet
                .Where(r => r.SupplierId == supplierId)
                .Include(r => r.Supplier)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت رسیدهای یک انبار خاص
        /// </summary>
        public async Task<IEnumerable<GoodsReceipt>> GetByWarehouseIdAsync(int warehouseId)
        {
            return await _dbSet
                .Where(r => r.WarehouseId == warehouseId)
                .Include(r => r.Warehouse)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت رسیدهای بین دو تاریخ (میلادی)
        /// </summary>
        public async Task<IEnumerable<GoodsReceipt>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(r => r.ReceiptDate >= startDate && r.ReceiptDate <= endDate)
                .Include(r => r.Product)
                .Include(r => r.Supplier)
                .OrderBy(r => r.ReceiptDate)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت رسیدهای تأیید شده (Status == 1)
        /// </summary>
        public async Task<IEnumerable<GoodsReceipt>> GetApprovedReceiptsAsync()
        {
            return await _dbSet
                .Where(r => r.Status == 1)
                .Include(r => r.Product)
                .Include(r => r.Supplier)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت رسیدهای همراه با جزئیات کامل (Include‌های مربوطه)
        /// </summary>
        public async Task<GoodsReceipt> GetReceiptWithDetailsAsync(int receiptId)
        {
            return await _dbSet
                .Include(r => r.Product)
                .Include(r => r.Supplier)
                .Include(r => r.Warehouse)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);
        }

        /// <summary>
        /// محاسبه مجموع مبلغ خالص (NetPrice) رسیدهای یک محصول
        /// </summary>
        public async Task<decimal> GetTotalNetPriceByProductAsync(Guid productId)
        {
            var receipts = await _dbSet
                .Where(r => r.ProductId == productId && r.Status == 1)
                .ToListAsync();
            return receipts.Sum(r => r.NetPrice);
        }

        /// <summary>
        /// تأیید یک رسید (تغییر Status به 1)
        /// </summary>
        public async Task<bool> ApproveReceiptAsync(int receiptId)
        {
            var receipt = await GetByIdAsync(receiptId);
            if (receipt == null) return false;
            receipt.Status = 1;
            await UpdateAsync(receipt);
            return true;
        }

        /// <summary>
        /// لغو یک رسید (تغییر Status به 2 یا حذف منطقی)
        /// </summary>
        public async Task<bool> CancelReceiptAsync(int receiptId)
        {
            var receipt = await GetByIdAsync(receiptId);
            if (receipt == null) return false;
            receipt.Status = 2; // فرض می‌کنیم 2 به معنی لغو شده است
            await UpdateAsync(receipt);
            return true;
        }

        // در صورت نیاز override UpdateAsync برای اعتبارسنجی
        public override async Task<GoodsReceipt> UpdateAsync(GoodsReceipt entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            // اعتبارسنجی‌های اضافی (مثلاً بررسی موجود بودن ProductId و ...)
            return await base.UpdateAsync(entity);
        }
    }
}