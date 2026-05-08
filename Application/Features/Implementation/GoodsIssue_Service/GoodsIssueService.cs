using Application.Features.Definition.Context;
using Application.Features.Implementation.GenericRepository_Service;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Implementation.GoodsIssue_Service
{
    public class GoodsIssueService : GenericRepository<GoodsIssue>
    {
        public GoodsIssueService(IApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// دریافت حواله بر اساس شماره فاکتور
        /// </summary>
        public async Task<GoodsIssue> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber))
                return null;
            return await _dbSet.FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        /// <summary>
        /// دریافت همه حواله‌های یک محصول خاص
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByProductIdAsync(Guid productId)
        {
            return await _dbSet
                .Where(i => i.ProductId == productId)
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت حواله‌های یک مشتری
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(i => i.CustomerId == customerId)
                .Include(i => i.Customer)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت حواله‌های یک انبار خاص
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByWarehouseIdAsync(int warehouseId)
        {
            return await _dbSet
                .Where(i => i.WarehouseId == warehouseId)
                .Include(i => i.Warehouse)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت حواله‌های با نوع خاص (فروش، مصرف داخلی و...)
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByTypeAsync(IssueType type)
        {
            return await _dbSet
                .Where(i => i.Type == type)
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت حواله‌های بین دو تاریخ (میلادی)
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(i => i.IssueDate >= startDate && i.IssueDate <= endDate)
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .OrderBy(i => i.IssueDate)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت حواله‌های تأیید شده (Status == 1)
        /// </summary>
        public async Task<IEnumerable<GoodsIssue>> GetApprovedIssuesAsync()
        {
            return await _dbSet
                .Where(i => i.Status == 1)
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت حواله به همراه جزئیات کامل (Include‌های مرتبط)
        /// </summary>
        public async Task<GoodsIssue> GetIssueWithDetailsAsync(int issueId)
        {
            return await _dbSet
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.Warehouse)
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.IssueId == issueId);
        }

        /// <summary>
        /// محاسبه مجموع تعداد خروجی برای یک محصول خاص (تأیید شده)
        /// </summary>
        public async Task<decimal> GetTotalQuantityByProductAsync(Guid productId)
        {
            var issues = await _dbSet
                .Where(i => i.ProductId == productId && i.Status == 1)
                .ToListAsync();
            return issues.Sum(i => i.Quantity);
        }

        /// <summary>
        /// تأیید یک حواله (تغییر Status به 1)
        /// </summary>
        public async Task<bool> ApproveIssueAsync(int issueId)
        {
            var issue = await GetByIdAsync(issueId);
            if (issue == null) return false;
            issue.Status = 1;
            await UpdateAsync(issue);
            return true;
        }

        /// <summary>
        /// لغو یک حواله (تغییر Status به 2)
        /// </summary>
        public async Task<bool> CancelIssueAsync(int issueId)
        {
            var issue = await GetByIdAsync(issueId);
            if (issue == null) return false;
            issue.Status = 2; // فرض می‌کنیم 2 به معنی لغو شده است
            await UpdateAsync(issue);
            return true;
        }

        // override UpdateAsync برای اعتبارسنجی (اختیاری)
        public override async Task<GoodsIssue> UpdateAsync(GoodsIssue entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            // می‌توانید بررسی کنید که مقدار موجودی کافی است یا خیر
            return await base.UpdateAsync(entity);
        }
    }
}