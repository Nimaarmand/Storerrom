using Application.Features.Definition.Context;
using Application.Features.Implementation.GenericRepository_Service;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Features.Implementation.Product_Service
{
    public class ProductService : GenericRepository<Product>
    {
        public ProductService(IApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// دریافت تعداد مشخصی محصول (جدیدترین‌ها بر اساس ProductId)
        /// </summary>
        public async Task<IEnumerable<Product>> GetTopProductsAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(p => p.ProductId) // جدیدترین‌ها اول
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت اطلاعات به تعداد مشخص با قابلیت مرتب‌سازی دلخواه
        /// </summary>
        public async Task<IEnumerable<Product>> GetTopProductsOrderedAsync<TKey>(
            int count,
            Expression<Func<Product, TKey>> orderBy,
            bool ascending = true)
        {
            IQueryable<Product> query = _dbSet;
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            return await query.Take(count).ToListAsync();
        }

        // 1. دریافت محصول با شناسه Guid
        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        // 2. دریافت همه محصولات به همراه دسته‌بندی
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        // 3. جستجوی محصولات بر اساس نام (تطبیق جزئی)
        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllProductsAsync();

            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(name))
                .ToListAsync();
        }

        // 4. جستجو بر اساس بارکد (یکتا)
        public async Task<Product> GetByBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return null;

            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Barcode == barcode);
        }

        // 5. بررسی تکراری نبودن بارکد
        public async Task<bool> IsBarcodeExistsAsync(string barcode, Guid? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return false;

            var query = _dbSet.Where(p => p.Barcode == barcode);
            if (excludeId.HasValue)
                query = query.Where(p => p.ProductId != excludeId.Value);
            return await query.AnyAsync();
        }

        // 6. بررسی تکراری نبودن نام
        public async Task<bool> IsProductNameExistsAsync(string name, Guid? excludeId = null)
        {
            var query = _dbSet.Where(p => p.Name == name);
            if (excludeId.HasValue)
                query = query.Where(p => p.ProductId != excludeId.Value);
            return await query.AnyAsync();
        }

        // 7. دریافت محصولات یک دسته خاص
        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        // 8. دریافت محصولات با موجودی کمتر از حداقل مجاز
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.MinStockLevel.HasValue && p.Number < p.MinStockLevel)
                .ToListAsync();
        }

        // 9. دریافت محصولات منقضی شده
        public async Task<IEnumerable<Product>> GetExpiredProductsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.ExpirationDate < DateTime.Now)
                .ToListAsync();
        }

        // 10. ایجاد محصول جدید با اعتبارسنجی کامل
        public async Task<(bool Success, string Message, Product Product)> CreateProductAsync(Product product)
        {
            if (product == null)
                return (false, "محصول نامعتبر است", null);

            if (string.IsNullOrWhiteSpace(product.Name))
                return (false, "نام محصول نمی‌تواند خالی باشد", null);

            if (string.IsNullOrWhiteSpace(product.BaseUnit))
                return (false, "واحد پایه نمی‌تواند خالی باشد", null);

            if (await IsProductNameExistsAsync(product.Name))
                return (false, "محصولی با این نام قبلاً ثبت شده است", null);

            if (!string.IsNullOrWhiteSpace(product.Barcode))
            {
                if (await IsBarcodeExistsAsync(product.Barcode))
                    return (false, "بارکد تکراری است", null);
            }

            if (product.Number < 0)
                return (false, "تعداد موجودی نمی‌تواند منفی باشد", null);

            if (product.MinStockLevel.HasValue && product.MaxStockLevel.HasValue &&
                product.MinStockLevel > product.MaxStockLevel)
            {
                return (false, "حداقل موجودی نمی‌تواند از حداکثر بیشتر باشد", null);
            }

            if (product.ProductionDate.HasValue && product.ExpirationDate.HasValue &&
                product.ProductionDate > product.ExpirationDate)
            {
                return (false, "تاریخ تولید نمی‌تواند بعد از تاریخ انقضا باشد", null);
            }

            if (product.Weight < 0)
                return (false, "وزن نمی‌تواند منفی باشد", null);

            if (product.CategoryId.HasValue)
            {
                var categoryExists = await _context.Set<Category>().AnyAsync(c => c.CategoryId == product.CategoryId);
                if (!categoryExists)
                    return (false, "دسته‌بندی انتخابی وجود ندارد", null);
            }

            product.ProductId = Guid.NewGuid();
            product.CreatedAt = DateTime.Now;
            product.IsActive = true;

            var created = await AddAsync(product);
            return (true, "محصول با موفقیت اضافه شد", created);
        }

        // 11. ویرایش محصول
        public async Task<(bool Success, string Message, Product Product)> UpdateProductAsync(Product product)
        {
            if (product == null)
                return (false, "محصول نامعتبر است", null);

            if (string.IsNullOrWhiteSpace(product.Name))
                return (false, "نام محصول نمی‌تواند خالی باشد", null);

            if (string.IsNullOrWhiteSpace(product.BaseUnit))
                return (false, "واحد پایه نمی‌تواند خالی باشد", null);

            if (await IsProductNameExistsAsync(product.Name, product.ProductId))
                return (false, "محصولی با این نام قبلاً ثبت شده است", null);

            if (!string.IsNullOrWhiteSpace(product.Barcode))
            {
                if (await IsBarcodeExistsAsync(product.Barcode, product.ProductId))
                    return (false, "بارکد تکراری است", null);
            }

            if (product.Number < 0)
                return (false, "تعداد موجودی نمی‌تواند منفی باشد", null);

            if (product.MinStockLevel.HasValue && product.MaxStockLevel.HasValue &&
                product.MinStockLevel > product.MaxStockLevel)
            {
                return (false, "حداقل موجودی نمی‌تواند از حداکثر بیشتر باشد", null);
            }

            if (product.ProductionDate.HasValue && product.ExpirationDate.HasValue &&
                product.ProductionDate > product.ExpirationDate)
            {
                return (false, "تاریخ تولید نمی‌تواند بعد از تاریخ انقضا باشد", null);
            }

            if (product.Weight < 0)
                return (false, "وزن نمی‌تواند منفی باشد", null);

            if (product.CategoryId.HasValue)
            {
                var categoryExists = await _context.Set<Category>().AnyAsync(c => c.CategoryId == product.CategoryId);
                if (!categoryExists)
                    return (false, "دسته‌بندی انتخابی وجود ندارد", null);
            }

            var existing = await GetByIdAsync(product.ProductId);
            if (existing == null)
                return (false, "محصول یافت نشد", null);

            product.CreatedAt = existing.CreatedAt;
            product.IsActive = existing.IsActive;

            var updated = await UpdateAsync(product);
            return (true, "محصول با موفقیت بروزرسانی شد", updated);
        }

        // 12. حذف فیزیکی محصول
        public async Task<(bool Success, string Message)> DeleteProductAsync(Guid productId)
        {
            var product = await GetByIdAsync(productId);
            if (product == null)
                return (false, "محصول یافت نشد");

            await RemoveAsync(product);
            return (true, "محصول با موفقیت حذف شد");
        }

        // 13. غیرفعال کردن محصول
        public async Task<(bool Success, string Message)> DeactivateProductAsync(Guid productId)
        {
            var product = await GetByIdAsync(productId);
            if (product == null)
                return (false, "محصول یافت نشد");

            product.IsActive = false;
            await UpdateAsync(product);
            return (true, "محصول غیرفعال شد");
        }

        // 14. فعال کردن محصول
        public async Task<(bool Success, string Message)> ActivateProductAsync(Guid productId)
        {
            var product = await GetByIdAsync(productId);
            if (product == null)
                return (false, "محصول یافت نشد");

            product.IsActive = true;
            await UpdateAsync(product);
            return (true, "محصول فعال شد");
        }

        // 15. صفحه‌بندی محصولات
        public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}