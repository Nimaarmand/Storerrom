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

namespace Application.Features.Implementation.GoodsIssue_Service
{
    public class GoodsIssueService : GenericRepository<GoodsIssue>
    {
        private readonly UsermanagementService _usermanagementService;
        private readonly UserManager<ApplicationUser> _userManager;

        // ========== سازنده‌ها ==========

        public GoodsIssueService(IApplicationDbContext context) : base(context)
        {
        }

        public GoodsIssueService(IApplicationDbContext context, UsermanagementService usermanagementService)
            : this(context)
        {
            _usermanagementService = usermanagementService;
        }

        public GoodsIssueService(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
            : this(context)
        {
            _userManager = userManager;
        }

        public GoodsIssueService(IApplicationDbContext context, UsermanagementService usermanagementService, UserManager<ApplicationUser> userManager)
            : this(context)
        {
            _usermanagementService = usermanagementService;
            _userManager = userManager;
        }

        // ========== متدهای جستجو ==========

        /// <summary>
        /// دریافت حواله بر اساس شماره فاکتور
        /// </summary>
        public async Task<GoodsIssue> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber)) return null;
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// دریافت حواله‌های یک محصول خاص
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByProductIdAsync(Guid productId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(i => i.ProductId == productId)
                    .Include(i => i.Product)
                    .Include(i => i.Customer)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// دریافت حواله‌های یک مشتری خاص
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByCustomerIdAsync(int customerId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(i => i.CustomerId == customerId)
                    .Include(i => i.Customer)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// دریافت حواله‌های یک انبار خاص
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByWarehouseIdAsync(int warehouseId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(i => i.WarehouseId == warehouseId)
                    .Include(i => i.Warehouse)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// دریافت حواله‌های با نوع خاص
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByTypeAsync(IssueType type)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(i => i.Type == type)
                    .Include(i => i.Product)
                    .Include(i => i.Customer)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// دریافت حواله‌های بین دو تاریخ
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(i => i.IssueDate >= startDate && i.IssueDate <= endDate)
                    .Include(i => i.Product)
                    .Include(i => i.Customer)
                    .OrderBy(i => i.IssueDate)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// دریافت حواله به همراه جزئیات کامل
        /// </summary>
        public async Task<GoodsIssue> GetIssueWithDetailsAsync(int issueId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(i => i.Product)
                    .Include(i => i.Customer)
                    .Include(i => i.Warehouse)
                    .FirstOrDefaultAsync(i => i.IssueId == issueId);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// محاسبه مجموع تعداد خروجی برای یک محصول (تأیید شده)
        /// </summary>
        public async Task<decimal> GetTotalQuantityByProductAsync(Guid productId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var issues = await _dbSet
                    .Where(i => i.ProductId == productId && i.Status == 1)
                    .ToListAsync();
                return issues.Sum(i => i.Quantity);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // ========== عملیات اصلی ==========

        /// <summary>
        /// تأیید حواله خروج (کاهش موجودی و ثبت تاریخ خروج و کاربر تأییدکننده)
        /// </summary>
        public async Task<(bool Success, string Message)> ApproveIssueAsync(int issueId, string currentUserId)
        {
            if (string.IsNullOrEmpty(currentUserId))
                return (false, "کاربر جاری شناسایی نشد. لطفاً وارد شوید.");

            var issue = await _dbSet
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(i => i.IssueId == issueId);

            if (issue == null)
                return (false, "حواله یافت نشد.");

            if (issue.Status == 1)
                return (false, "این حواله قبلاً تأیید شده است.");

            if (issue.Product.Number < issue.Quantity)
                return (false, $"موجودی محصول '{issue.Product.Name}' کافی نیست.");

            // دریافت کاربر تأییدکننده
            ApplicationUser user = null;
            if (_userManager != null)
                user = await _userManager.FindByIdAsync(currentUserId);

            if (user == null && _usermanagementService != null)
            {
                var userEntity = await _usermanagementService.FindUserByIdAsync(currentUserId);
                if (userEntity != null)
                {
                    // اگر FullName در دسترس است
                    var fullName = userEntity.GetType().GetProperty("FullName")?.GetValue(userEntity)?.ToString();
                    user = new ApplicationUser { Id = currentUserId, FullName = fullName ?? "نامشخص" };
                }
            }

            if (user == null)
                return (false, "کاربر تأییدکننده در دیتابیس یافت نشد.");

            var dbContext = (DbContext)_context;
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                issue.Product.Number -= (int)issue.Quantity;
                issue.Warehouse.Number -= (int)issue.Quantity;
                issue.Status = 1;
                issue.IssueDate = DateTime.Today;
                issue.ApprovedByUserId = currentUserId;
                issue.ApprovedByFullName = user.FullName ?? user.UserName ?? "نامشخص";

                dbContext.Entry(issue).State = EntityState.Modified;
                dbContext.Entry(issue.Product).State = EntityState.Modified;
                dbContext.Entry(issue.Warehouse).State = EntityState.Modified;

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "حواله با موفقیت تأیید شد و موجودی‌ها به‌روزرسانی شدند.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"خطا در تأیید حواله: {ex.Message}");
            }
        }

        /// <summary>
        /// لغو حواله (تغییر وضعیت به 2)
        /// </summary>
        public async Task<bool> CancelIssueAsync(int issueId)
        {
            var issue = await GetByIdAsync(issueId);
            if (issue == null) return false;
            issue.Status = 2;
            await UpdateAsync(issue);
            return true;
        }

        /// <summary>
        /// به‌روزرسانی حواله (با قفل)
        /// </summary>
        public override async Task<GoodsIssue> UpdateAsync(GoodsIssue entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
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

        // ========== متدهای DTO ==========

        /// <summary>
        /// دریافت حواله‌های تأیید شده به صورت DTO
        /// </summary>
        public async Task<List<GoodsIssueDto>> GetApprovedIssuesDtoAsync(int topCount = 50)
        {
            if (topCount <= 0) topCount = 20;
            var issues = await _dbSet
                .Where(i => i.Status == 1)
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Include(i => i.Customer)
              
                .OrderByDescending(i => i.IssueDate)
                .Take(topCount)
                .ToListAsync();
            var result = issues.Select(i => new GoodsIssueDto
            {
                IssueId = i.IssueId,
                ProductName = i.Product?.Name ?? "نامشخص",
                WarehouseName = i.Warehouse?.Name ?? "نامشخص",
                CustomerName = i.Customer?.Name ?? null,
                UserName = i.UserFullName ?? "نامشخص",
                ApprovedByUserName = i.ApprovedByFullName ?? "نامشخص",
                Quantity = i.Quantity,
                Unit = i.Unit,
                UnitSellingPrice = i.UnitSellingPrice,
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate ?? DateTime.MinValue,
                CreatedAt = i.CreatedAt,
                IssueDate = i.IssueDate ?? DateTime.MinValue,
                StatusText = "تأیید شده",
                BatchNumber = i.BatchNumber,
                Description = i.Description,
                TypeText = i.Type.ToString()
            }).ToList();

            return result;
        }

        /// <summary>
        /// دریافت حواله‌های در انتظار تأیید (Status = 0) به صورت DTO
        /// </summary>
        public async Task<List<GoodsIssueDto>> GetTopPendingIssuesAsync(int topCount)
        {
            if (topCount <= 0) topCount = 20;

            var issues = await _dbSet
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.Warehouse)
                .Where(i => i.Status == 0)
                .OrderByDescending(i => i.IssueDate)
                .Take(topCount)
                .AsNoTracking()
                .Select(i => new GoodsIssueDto
                {
                    IssueId = i.IssueId,
                    ProductName = i.Product != null ? i.Product.Name : "نامشخص",
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    UnitSellingPrice = i.UnitSellingPrice,
                    TypeText = i.Type.ToString(),
                    CustomerName = i.Customer != null ? i.Customer.Name : null,
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "نامشخص",
                    CreatedAt = i.CreatedAt,
                    InvoiceNumber = i.InvoiceNumber,
                    StatusText = "در انتظار تأیید",
                    UserName = i.UserFullName ?? "نامشخص",
                    BatchNumber = i.BatchNumber,
                    Description = i.Description
                })
                .ToListAsync();

            return issues;
        }

        /// <summary>
        /// دریافت حواله‌های خروج (با فیلتر وضعیت) به صورت DTO
        /// </summary>
        public async Task<List<GoodsIssueDto>> GetTopIssuesWithDetailsAsync(int topCount)
        {
            if (topCount <= 0) topCount = 20;

            try
            {
                var issues = await _dbSet
                    .Include(i => i.Product)
                    .Include(i => i.Warehouse)
                    .Include(i => i.Customer)
                    .OrderByDescending(i => i.IssueDate)
                    .Take(topCount)
                    .AsNoTracking()
                    .ToListAsync();

                var result = new List<GoodsIssueDto>();
                foreach (var issue in issues)
                {
                    string userName = "نامشخص";
                    if (_usermanagementService != null && !string.IsNullOrEmpty(issue.UserId))
                        userName = await _usermanagementService.GetUserNameByIdAsync(issue.UserId);
                    else if (!string.IsNullOrEmpty(issue.UserFullName))
                        userName = issue.UserFullName;

                    result.Add(new GoodsIssueDto
                    {
                        IssueId = issue.IssueId,
                        ProductName = issue.Product?.Name ?? "نامشخص",
                        WarehouseName = issue.Warehouse?.Name ?? "نامشخص",
                        CustomerName = issue.Customer?.Name ?? (issue.Type == IssueType.Sale ? "مشتری حذف شده" : "-"),
                        UserName = userName,
                        Quantity = issue.Quantity,
                        Unit = issue.Unit,
                        UnitSellingPrice = issue.UnitSellingPrice,
                        InvoiceNumber = issue.InvoiceNumber,
                        InvoiceDate = issue.InvoiceDate ?? DateTime.MinValue,
                        CreatedAt = issue.CreatedAt,
                        IssueDate = issue.IssueDate ?? DateTime.MinValue,
                        StatusText = issue.Status == 0 ? "در انتظار تأیید" : (issue.Status == 1 ? "تأیید شده" : "لغو شده"),
                        BatchNumber = issue.BatchNumber,
                        Description = issue.Description,
                        TypeText = issue.Type.ToString(),
                        ApprovedByUserName = issue.ApprovedByFullName ?? "نامشخص"
                    });
                }
                return result;
            }
            finally { }
        }

        /// <summary>
        /// جستجوی حواله‌ها بر اساس کلمه کلیدی
        /// </summary>
        public async Task<List<GoodsIssueDto>> SearchIssuesAsync(string keyword, int topCount = 50)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetTopIssuesWithDetailsAsync(topCount);

            keyword = keyword.Trim().ToLower();
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var query = _dbSet
                    .Include(i => i.Product)
                    .Include(i => i.Warehouse)
                    .Include(i => i.Customer)
                    .Where(i => i.InvoiceNumber.ToLower().Contains(keyword) ||
                                (i.Product != null && i.Product.Name.ToLower().Contains(keyword)) ||
                                (i.Customer != null && i.Customer.Name.ToLower().Contains(keyword)))
                    .OrderByDescending(i => i.IssueDate)
                    .Take(topCount)
                    .AsNoTracking();

                var issues = await query.ToListAsync();
                var result = new List<GoodsIssueDto>();
                foreach (var issue in issues)
                {
                    string userName = "نامشخص";
                    if (_usermanagementService != null && !string.IsNullOrEmpty(issue.UserId))
                        userName = await _usermanagementService.GetUserNameByIdAsync(issue.UserId);
                    else if (!string.IsNullOrEmpty(issue.UserFullName))
                        userName = issue.UserFullName;

                    result.Add(new GoodsIssueDto
                    {
                        IssueId = issue.IssueId,
                        ProductName = issue.Product?.Name ?? "نامشخص",
                        WarehouseName = issue.Warehouse?.Name ?? "نامشخص",
                        CustomerName = issue.Customer?.Name ?? (issue.Type == IssueType.Sale ? "مشتری حذف شده" : "-"),
                        UserName = userName,
                        Quantity = issue.Quantity,
                        Unit = issue.Unit,
                        UnitSellingPrice = issue.UnitSellingPrice,
                        InvoiceNumber = issue.InvoiceNumber,
                        InvoiceDate = issue.InvoiceDate ?? DateTime.MinValue,
                        CreatedAt = issue.CreatedAt,
                        IssueDate = issue.IssueDate ?? DateTime.MinValue,
                        StatusText = issue.Status == 0 ? "در انتظار تأیید" : (issue.Status == 1 ? "تأیید شده" : "لغو شده"),
                        BatchNumber = issue.BatchNumber,
                        Description = issue.Description,
                        TypeText = issue.Type.ToString(),
                        ApprovedByUserName = issue.ApprovedByFullName ?? "نامشخص"
                    });
                }
                return result;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        /// <summary>
        /// جستجوی حواله‌های در انتظار تأیید بر اساس کلمه کلیدی
        /// </summary>
        public async Task<List<GoodsIssueDto>> SearchPendingIssuesAsync(string keyword, int topCount = 50)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetTopPendingIssuesAsync(topCount);

            keyword = keyword.Trim().ToLower();

            await DbLock.Semaphore.WaitAsync();
            try
            {
                var issues = await _dbSet
                    .Include(i => i.Product)
                    .Include(i => i.Customer)
                    .Include(i => i.Warehouse)
                    .Where(i => i.Status == 0 && (
                        i.InvoiceNumber.ToLower().Contains(keyword) ||
                        (i.Product != null && i.Product.Name.ToLower().Contains(keyword)) ||
                        (i.Customer != null && i.Customer.Name.ToLower().Contains(keyword))))
                    .OrderByDescending(i => i.IssueDate)
                    .Take(topCount)
                    .AsNoTracking()
                    .Select(i => new GoodsIssueDto
                    {
                        IssueId = i.IssueId,
                        ProductName = i.Product != null ? i.Product.Name : "نامشخص",
                        Quantity = i.Quantity,
                        Unit = i.Unit,
                        UnitSellingPrice = i.UnitSellingPrice,
                        TypeText = i.Type.ToString(),
                        CustomerName = i.Customer != null ? i.Customer.Name : null,
                        WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "نامشخص",
                        CreatedAt = i.CreatedAt,
                        InvoiceNumber = i.InvoiceNumber,
                        StatusText = "در انتظار تأیید",
                        UserName = i.UserFullName ?? "نامشخص",
                        BatchNumber = i.BatchNumber,
                        Description = i.Description
                    })
                    .ToListAsync();

                return issues;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }
    }
}