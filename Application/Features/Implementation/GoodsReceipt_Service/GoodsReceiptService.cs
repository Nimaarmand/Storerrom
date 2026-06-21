using Application.Dto;
using Application.Features.Definition.Context;
using Application.Features.Implementation.Common;
using Application.Features.Implementation.GenericRepository_Service;
using Application.Features.Implementation.Usermanagement_Service;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public GoodsReceiptService(IApplicationDbContext context,
            UsermanagementService usermanagementService,
            UserManager<ApplicationUser> userManager
            )
            : base(context)
        {
            _usermanagementService = usermanagementService;
            _userManager = userManager;
        }

        // ========== متدهای اختصاصی (همگی با قفل) ==========
        
        
        /// <summary>
        /// دریافت یک رسید بر اساس شماره فاکتور (حساس به حروف بزرگ و کوچک نیست)
        /// </summary>
        /// <param name="invoiceNumber">شماره فاکتور</param>
        /// <returns>شیء GoodsReceipt در صورت وجود، در غیر این صورت null</returns>
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

        /// <summary>
        /// دریافت تمام رسیدهای مربوط به یک محصول مشخص
        /// </summary>
        /// <param name="productId">شناسه محصول (Guid)</param>
        /// <returns>لیستی از رسیدها به همراه اطلاعات محصول و تأمین‌کننده</returns>
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

        /// <summary>
        /// دریافت تمام رسیدهای مربوط به یک تأمین‌کننده خاص
        /// </summary>
        /// <param name="supplierId">شناسه تأمین‌کننده</param>
        /// <returns>لیست رسیدها به همراه اطلاعات تأمین‌کننده</returns>
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

        /// <summary>
        /// دریافت تمام رسیدهای مربوط به یک انبار خاص
        /// </summary>
        /// <param name="warehouseId">شناسه انبار</param>
        /// <returns>لیست رسیدها به همراه اطلاعات انبار</returns>
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

        /// <summary>
        /// دریافت رسیدهای ثبت‌شده در یک بازه زمانی مشخص
        /// </summary>
        /// <param name="startDate">تاریخ شروع (شمسی – میلادی در دیتابیس)</param>
        /// <param name="endDate">تاریخ پایان</param>
        /// <returns>لیست رسیدها مرتب شده بر اساس تاریخ دریافت (قدیم‌ترین اول)</returns>
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
        /// <summary>
        /// دریافت تعداد مشخصی از آخرین رسیدهای **تأیید شده** (وضعیت 1) به همراه جزئیات (DTO)
        /// </summary>
        /// <param name="topCount">تعداد رکورد مورد نظر (پیش‌فرض ۲۰)</param>
        /// <returns>لیستی از GoodsReceiptDto</returns>
        public async Task<List<GoodsReceiptDto>> GetTopApprovedReceiptsAsync(int topCount)
        {
            if (topCount <= 0) topCount = 20;

            try
            {
                var receipts = await _dbSet
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .Include(r => r.Warehouse)
                    .Where(r => r.Status == 1)   // فقط تأیید شده‌ها
                    .OrderByDescending(r => r.ReceiptDate)
                    .Take(topCount)
                    .AsNoTracking()
                    .ToListAsync();

                if (receipts == null || !receipts.Any())
                    return new List<GoodsReceiptDto>();

                // دریافت نام کاربران یکجا
                var userIds = receipts.Where(r => !string.IsNullOrEmpty(r.UserId)).Select(r => r.UserId).Distinct().ToList();
                Dictionary<string, string> userNames = new Dictionary<string, string>();

                if (userIds.Any())
                {
                    var users = await _userManager.Users
                        .Where(u => userIds.Contains(u.Id))
                        .Select(u => new { u.Id, u.UserName })
                        .ToListAsync();
                    userNames = users.ToDictionary(u => u.Id, u => u.UserName);
                }

                // ساخت DTO
                var result = new List<GoodsReceiptDto>();
                foreach (var receipt in receipts)
                {
                    string userName = (!string.IsNullOrEmpty(receipt.UserId) && userNames.ContainsKey(receipt.UserId))
                        ? userNames[receipt.UserId]
                        : "نامشخص";

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
                        StatusText = "تأیید شده", // چون وضعیت 1
                        BatchNumber = receipt.BatchNumber,
                        Description = receipt.Description
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// دریافت یک رسید به همراه تمام جزئیات مرتبط (محصول، تأمین‌کننده، انبار)
        /// </summary>
        /// <param name="receiptId">شناسه رسید</param>
        /// <returns>شیء GoodsReceipt با اطلاعات وابسته</returns>
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

        /// <summary>
        /// محاسبه قیمت خالص کل (با احتساب مالیات) برای یک محصول خاص بر اساس رسیدهای تأیید شده
        /// </summary>
        /// <param name="productId">شناسه محصول</param>
        /// <returns>جمع قیمت خالص (decimal)</returns>
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

        /// <summary>
        /// تأیید رسید ورودی، افزایش موجودی محصول و موجودی کل انبار، با اعتبارسنجی ظرفیت
        /// </summary>
        public async Task<(bool Success, string Message)> ApproveReceiptAsync(int receiptId)
        {
            var receipt = await _dbSet
                .Include(r => r.Product)
                .Include(r => r.Warehouse)
                .FirstOrDefaultAsync(r => r.ReceiptId == receiptId);

            if (receipt == null)
                return (false, "رسید یافت نشد.");

            if (receipt.Status == 1)
                return (false, "این رسید قبلاً تأیید شده است.");

            int currentWarehouseStock = receipt.Warehouse.Number;
            int newWarehouseStock = currentWarehouseStock + (int)receipt.Quantity;
            if (newWarehouseStock > receipt.Warehouse.Max)
            {
                return (false, $"ظرفیت انبار کافی نیست. حداکثر: {receipt.Warehouse.Max} - موجودی فعلی: {currentWarehouseStock}");
            }

            // کست کردن _context به DbContext برای دسترسی به Database
            var dbContext = (DbContext)_context;
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                receipt.Product.Number += (int)receipt.Quantity;
                receipt.Warehouse.Number = newWarehouseStock;
                receipt.Status = 1;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "رسید با موفقیت تأیید شد و موجودی‌ها به‌روزرسانی گردید.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"خطا: {ex.Message}");
            }
        }

        /// <summary>
        /// لغو یک رسید (تغییر وضعیت به 2)
        /// </summary>
        /// <param name="receiptId">شناسه رسید</param>
        /// <returns>true در صورت موفقیت، false اگر رسید یافت نشد</returns>
        public async Task<bool> CancelReceiptAsync(int receiptId)
        {
            var receipt = await GetByIdAsync(receiptId);
            if (receipt == null) return false;
            receipt.Status = 2;
            await UpdateAsync(receipt);
            return true;
        }

        /// <summary>
        /// به‌روزرسانی یک رسید (با قفل)
        /// </summary>
        /// <param name="entity">شیء GoodsReceipt با مقادیر جدید</param>
        /// <returns>شیء به‌روز شده</returns>
        public override async Task<GoodsReceipt> UpdateAsync(GoodsReceipt entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            //await DbLock.Semaphore.WaitAsync();
            try
            {
                return await base.UpdateAsync(entity);
            }
            finally
            {
                //DbLock.Semaphore.Release();
            }
        }

        // ========== متدهای DTO (نیاز به قفل دارند چون به دیتابیس دسترسی دارند) ==========

        /// <summary>
        /// دریافت تعداد مشخصی از آخرین رسیدها به همراه جزئیات (نام محصول، تأمین‌کننده، انبار، کاربر و ...)
        /// </summary>
        /// <param name="topCount">تعداد رکورد مورد نظر (پیش‌فرض ۲۰)</param>
        /// <returns>لیستی از GoodsReceiptDto</returns>
        public async Task<List<GoodsReceiptDto>> GetTopReceiptsWithDetailsAsync(int topCount)
        {
            if (topCount <= 0) topCount = 20;

            // (اختیاری) در صورت نیاز قفل را فعال کنید
            // await DbLock.Semaphore.WaitAsync();
            try
            {
                var receipts = await _dbSet
                    .Include(r => r.Product)
                    .Include(r => r.Supplier)
                    .Include(r => r.Warehouse)
                    .Where(r => r.Status == 0)
                    .OrderByDescending(r => r.ReceiptDate)
                    .Take(topCount)
                    .AsNoTracking()
                    .ToListAsync();

                if (receipts == null || !receipts.Any())
                    return new List<GoodsReceiptDto>();

                var userIds = receipts.Where(r => !string.IsNullOrEmpty(r.UserId)).Select(r => r.UserId).Distinct().ToList();
                Dictionary<string, string> userNames = new Dictionary<string, string>();

                if (userIds.Any())
                {
                    // فرض بر این است که _userManager در دسترس است (از سازنده تزریق شده)
                    var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).Select(u => new { u.Id, u.UserName }).ToListAsync();
                    userNames = users.ToDictionary(u => u.Id, u => u.UserName);
                }

                var result = new List<GoodsReceiptDto>();
                foreach (var receipt in receipts)
                {
                    string userName = (!string.IsNullOrEmpty(receipt.UserId) && userNames.ContainsKey(receipt.UserId))
                        ? userNames[receipt.UserId]
                        : "نامشخص";

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
            catch (Exception ex)
            {
                // می‌توانید خطا را لاگ کنید
                throw;
            }
            finally
            {
                // DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// جستجوی رسیدها بر اساس کلمه کلیدی (در شماره فاکتور، نام محصول، نام تأمین‌کننده)
        /// </summary>
        /// <param name="keyword">عبارت جستجو</param>
        /// <param name="topCount">حداکثر تعداد نتیجه (پیش‌فرض ۵۰)</param>
        /// <returns>لیستی از GoodsReceiptDto</returns>
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