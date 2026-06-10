//using Application.Features.Definition.Context;
//using Application.Features.Implementation.GenericRepository_Service;
//using Domain.Entity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Application.Features.Implementation.GoodsReceipt_Service
//{
//    public class GoodsReceiptService : GenericRepository<GoodsReceipt>
//    {
//        public GoodsReceiptService(IApplicationDbContext context) : base(context)
//        {
//        }

//        /// <summary>
//        /// دریافت رسید بر اساس شماره فاکتور
//        /// </summary>
//        public async Task<GoodsReceipt> GetByInvoiceNumberAsync(string invoiceNumber)
//        {
//            if (string.IsNullOrWhiteSpace(invoiceNumber))
//                return null;
//            return await _dbSet.FirstOrDefaultAsync(r => r.InvoiceNumber == invoiceNumber);
//        }

//        /// <summary>
//        /// دریافت همه رسیدهای یک محصول خاص
//        /// </summary>
//        public async Task<IEnumerable<GoodsReceipt>> GetByProductIdAsync(Guid productId)
//        {
//            return await _dbSet
//                .Where(r => r.ProductId == productId)
//                .Include(r => r.Product)
//                .Include(r => r.Supplier)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// دریافت رسیدهای یک تأمین‌کننده
//        /// </summary>
//        public async Task<IEnumerable<GoodsReceipt>> GetBySupplierIdAsync(int supplierId)
//        {
//            return await _dbSet
//                .Where(r => r.SupplierId == supplierId)
//                .Include(r => r.Supplier)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// دریافت رسیدهای یک انبار خاص
//        /// </summary>
//        public async Task<IEnumerable<GoodsReceipt>> GetByWarehouseIdAsync(int warehouseId)
//        {
//            return await _dbSet
//                .Where(r => r.WarehouseId == warehouseId)
//                .Include(r => r.Warehouse)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// دریافت رسیدهای بین دو تاریخ (میلادی)
//        /// </summary>
//        public async Task<IEnumerable<GoodsReceipt>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
//        {
//            return await _dbSet
//                .Where(r => r.ReceiptDate >= startDate && r.ReceiptDate <= endDate)
//                .Include(r => r.Product)
//                .Include(r => r.Supplier)
//                .OrderBy(r => r.ReceiptDate)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// دریافت رسیدهای تأیید شده (Status == 1)
//        /// </summary>
//        public async Task<IEnumerable<GoodsReceipt>> GetApprovedReceiptsAsync()
//        {
//            return await _dbSet
//                .Where(r => r.Status == 1)
//                .Include(r => r.Product)
//                .Include(r => r.Supplier)
//                .ToListAsync();
//        }

//        /// <summary>
//        /// دریافت رسیدهای همراه با جزئیات کامل (Include‌های مربوطه)
//        /// </summary>
//        public async Task<GoodsReceipt> GetReceiptWithDetailsAsync(int receiptId)
//        {
//            return await _dbSet
//                .Include(r => r.Product)
//                .Include(r => r.Supplier)
//                .Include(r => r.Warehouse)

//                .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);
//        }

//        /// <summary>
//        /// محاسبه مجموع مبلغ خالص (NetPrice) رسیدهای یک محصول
//        /// </summary>
//        public async Task<decimal> GetTotalNetPriceByProductAsync(Guid productId)
//        {
//            var receipts = await _dbSet
//                .Where(r => r.ProductId == productId && r.Status == 1)
//                .ToListAsync();
//            return receipts.Sum(r => r.NetPrice);
//        }

//        /// <summary>
//        /// تأیید یک رسید (تغییر Status به 1)
//        /// </summary>
//        public async Task<bool> ApproveReceiptAsync(int receiptId)
//        {
//            var receipt = await GetByIdAsync(receiptId);
//            if (receipt == null) return false;
//            receipt.Status = 1;
//            await UpdateAsync(receipt);
//            return true;
//        }

//        /// <summary>
//        /// لغو یک رسید (تغییر Status به 2 یا حذف منطقی)
//        /// </summary>
//        public async Task<bool> CancelReceiptAsync(int receiptId)
//        {
//            var receipt = await GetByIdAsync(receiptId);
//            if (receipt == null) return false;
//            receipt.Status = 2; // فرض می‌کنیم 2 به معنی لغو شده است
//            await UpdateAsync(receipt);
//            return true;
//        }

//        // در صورت نیاز override UpdateAsync برای اعتبارسنجی
//        public override async Task<GoodsReceipt> UpdateAsync(GoodsReceipt entity)
//        {
//            if (entity == null)
//                throw new ArgumentNullException(nameof(entity));
//            // اعتبارسنجی‌های اضافی (مثلاً بررسی موجود بودن ProductId و ...)
//            return await base.UpdateAsync(entity);
//        }
//    }
//}
using Application.Dto;
using Application.Features.Definition.Context;
using Application.Features.Implementation.Common;
using Application.Features.Implementation.GenericRepository_Service;
using Application.Features.Implementation.Usermanagement_Service;
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
        private readonly UsermanagementService _usermanagementService;

        public GoodsReceiptService(IApplicationDbContext context, UsermanagementService usermanagementService)
            : base(context)
        {
            _usermanagementService = usermanagementService;
        }

        // ========== متدهای اختصاصی (همگی با قفل) ==========

        public async Task<GoodsReceipt> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber)) return null;
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.FirstOrDefaultAsync(r => r.InvoiceNumber == invoiceNumber);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<GoodsReceipt>> GetByProductIdAsync(Guid productId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(r => r.ProductId == productId)
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<GoodsReceipt>> GetBySupplierIdAsync(int supplierId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(r => r.SupplierId == supplierId)
                    .Include(r => r.Supplier)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<GoodsReceipt>> GetByWarehouseIdAsync(int warehouseId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(r => r.WarehouseId == warehouseId)
                    .Include(r => r.Warehouse)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<GoodsReceipt>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(r => r.ReceiptDate >= startDate && r.ReceiptDate <= endDate)
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .OrderBy(r => r.ReceiptDate)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<GoodsReceipt>> GetApprovedReceiptsAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(r => r.Status == 1)
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<GoodsReceipt> GetReceiptWithDetailsAsync(int receiptId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .Include(r => r.Warehouse)
                    .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<decimal> GetTotalNetPriceByProductAsync(Guid productId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var receipts = await _dbSet
                    .Where(r => r.ProductId == productId && r.Status == 1)
                    .ToListAsync();
                return receipts.Sum(r => r.NetPrice);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> ApproveReceiptAsync(int receiptId)
        {
            var receipt = await GetByIdAsync(receiptId);
            if (receipt == null) return false;
            receipt.Status = 1;
            await UpdateAsync(receipt);
            return true;
        }

        public async Task<bool> CancelReceiptAsync(int receiptId)
        {
            var receipt = await GetByIdAsync(receiptId);
            if (receipt == null) return false;
            receipt.Status = 2;
            await UpdateAsync(receipt);
            return true;
        }

        public override async Task<GoodsReceipt> UpdateAsync(GoodsReceipt entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await base.UpdateAsync(entity);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // ========== متدهای DTO (نیاز به قفل دارند چون به دیتابیس دسترسی دارند) ==========

        public async Task<List<GoodsReceiptDto>> GetTopReceiptsWithDetailsAsync(int topCount)
        {
            if (topCount <= 0) topCount = 20;

            await DbLock.Semaphore.WaitAsync();
            try
            {
                var receipts = await _dbSet
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .Include(r => r.Warehouse)
                    .OrderByDescending(r => r.ReceiptDate)
                    .Take(topCount)
                    .AsNoTracking()
                    .ToListAsync();

                var result = new List<GoodsReceiptDto>();
                foreach (var receipt in receipts)
                {
                    var userName = await _usermanagementService.GetUserNameByIdAsync(receipt.UserId);
                    result.Add(new GoodsReceiptDto
                    {
                        ReceiptId = receipt.ReceiptId,
                        ProductName = receipt.Product?.Name ?? "نامشخص",
                        SupplierName = receipt.Supplier?.Name ?? "نامشخص",
                        WarehouseName = receipt.Warehouse?.Name ?? "نامشخص",
                        UserName = userName,
                        Quantity = receipt.Quantity,
                        Unit = receipt.Unit,
                        UnitPrice = receipt.UnitPrice,
                        TotalPrice = receipt.Quantity * receipt.UnitPrice,
                        InvoiceNumber = receipt.InvoiceNumber,
                        InvoiceDate = receipt.InvoiceDate,
                        ReceiptDate = receipt.ReceiptDate,
                        StatusText = receipt.Status == 0 ? "در انتظار تأیید" : (receipt.Status == 1 ? "تأیید شده" : "لغو شده"),
                        BatchNumber = receipt.BatchNumber,
                        Description = receipt.Description
                    });
                }
                return result;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<List<GoodsReceiptDto>> SearchReceiptsAsync(string keyword, int topCount = 50)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetTopReceiptsWithDetailsAsync(topCount);

            keyword = keyword.Trim().ToLower();

            await DbLock.Semaphore.WaitAsync();
            try
            {
                var query = _dbSet
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .Include(r => r.Warehouse)
                    .Where(r => r.InvoiceNumber.ToLower().Contains(keyword) ||
                                (r.Product != null && r.Product.Name.ToLower().Contains(keyword)) ||
                                (r.Supplier != null && r.Supplier.Name.ToLower().Contains(keyword)))
                    .OrderByDescending(r => r.ReceiptDate)
                    .Take(topCount)
                    .AsNoTracking();

                var receipts = await query.ToListAsync();
                var result = new List<GoodsReceiptDto>();
                foreach (var receipt in receipts)
                {
                    var userName = await _usermanagementService.GetUserNameByIdAsync(receipt.UserId);
                    result.Add(new GoodsReceiptDto
                    {
                        ReceiptId = receipt.ReceiptId,
                        ProductName = receipt.Product?.Name ?? "نامشخص",
                        SupplierName = receipt.Supplier?.Name ?? "نامشخص",
                        WarehouseName = receipt.Warehouse?.Name ?? "نامشخص",
                        UserName = userName,
                        Quantity = receipt.Quantity,
                        Unit = receipt.Unit,
                        UnitPrice = receipt.UnitPrice,
                        TotalPrice = receipt.Quantity * receipt.UnitPrice,
                        InvoiceNumber = receipt.InvoiceNumber,
                        InvoiceDate = receipt.InvoiceDate,
                        ReceiptDate = receipt.ReceiptDate,
                        StatusText = receipt.Status == 0 ? "در انتظار تأیید" : (receipt.Status == 1 ? "تأیید شده" : "لغو شده"),
                        BatchNumber = receipt.BatchNumber,
                        Description = receipt.Description
                    });
                }
                return result;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }
    }
}