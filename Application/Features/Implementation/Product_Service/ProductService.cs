using Application.Features.Definition.Context;
using Application.Features.Implementation.Common;
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

        public async Task<IEnumerable<Product>> GetTopProductsAsync(int count)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .OrderByDescending(p => p.ProductId)
                    .Take(count)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Product>> GetTopProductsOrderedAsync<TKey>(
            int count,
            Expression<Func<Product, TKey>> orderBy,
            bool ascending = true)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                IQueryable<Product> query = _dbSet;
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
                return await query.Take(count).ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllProductsAsync();

            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .Where(p => p.Name.Contains(name))
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<Product> GetByBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return null;
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Barcode == barcode);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> IsBarcodeExistsAsync(string barcode, Guid? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return false;
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var query = _dbSet.Where(p => p.Barcode == barcode);
                if (excludeId.HasValue)
                    query = query.Where(p => p.ProductId != excludeId.Value);
                return await query.AnyAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> IsProductNameExistsAsync(string name, Guid? excludeId = null)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var query = _dbSet.Where(p => p.Name == name);
                if (excludeId.HasValue)
                    query = query.Where(p => p.ProductId != excludeId.Value);
                return await query.AnyAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .Where(p => p.MinStockLevel.HasValue && p.Number < p.MinStockLevel)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Product>> GetExpiredProductsAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .Where(p => p.ExpirationDate < DateTime.Now)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<(bool Success, string Message, Product Product)> CreateProductAsync(Product product)
        {
            if (product == null) return (false, "محصول نامعتبر است", null);
            if (string.IsNullOrWhiteSpace(product.Name)) return (false, "نام محصول نمی‌تواند خالی باشد", null);
            if (string.IsNullOrWhiteSpace(product.BaseUnit)) return (false, "واحد پایه نمی‌تواند خالی باشد", null);
            if (await IsProductNameExistsAsync(product.Name)) return (false, "محصولی با این نام قبلاً ثبت شده است", null);
            if (!string.IsNullOrWhiteSpace(product.Barcode) && await IsBarcodeExistsAsync(product.Barcode))
                return (false, "بارکد تکراری است", null);
            if (product.Number < 0) return (false, "تعداد موجودی نمی‌تواند منفی باشد", null);
            if (product.MinStockLevel.HasValue && product.MaxStockLevel.HasValue && product.MinStockLevel > product.MaxStockLevel)
                return (false, "حداقل موجودی نمی‌تواند از حداکثر بیشتر باشد", null);
            if (product.ProductionDate.HasValue && product.ExpirationDate.HasValue && product.ProductionDate > product.ExpirationDate)
                return (false, "تاریخ تولید نمی‌تواند بعد از تاریخ انقضا باشد", null);
            if (product.Weight < 0) return (false, "وزن نمی‌تواند منفی باشد", null);
            if (product.CategoryId.HasValue)
            {
                var categoryExists = await _context.Set<Category>().AnyAsync(c => c.CategoryId == product.CategoryId);
                if (!categoryExists) return (false, "دسته‌بندی انتخابی وجود ندارد", null);
            }

            product.ProductId = Guid.NewGuid();
            product.CreatedAt = DateTime.Now;
            product.IsActive = true;
            var created = await AddAsync(product);
            return (true, "محصول با موفقیت اضافه شد", created);
        }

        public async Task<(bool Success, string Message, Product Product)> UpdateProductAsync(Product product)
        {
            if (product == null) return (false, "محصول نامعتبر است", null);
            if (string.IsNullOrWhiteSpace(product.Name)) return (false, "نام محصول نمی‌تواند خالی باشد", null);
            if (string.IsNullOrWhiteSpace(product.BaseUnit)) return (false, "واحد پایه نمی‌تواند خالی باشد", null);
            if (await IsProductNameExistsAsync(product.Name, product.ProductId))
                return (false, "محصولی با این نام قبلاً ثبت شده است", null);
            if (!string.IsNullOrWhiteSpace(product.Barcode) && await IsBarcodeExistsAsync(product.Barcode, product.ProductId))
                return (false, "بارکد تکراری است", null);
            if (product.Number < 0) return (false, "تعداد موجودی نمی‌تواند منفی باشد", null);
            if (product.MinStockLevel.HasValue && product.MaxStockLevel.HasValue && product.MinStockLevel > product.MaxStockLevel)
                return (false, "حداقل موجودی نمی‌تواند از حداکثر بیشتر باشد", null);
            if (product.ProductionDate.HasValue && product.ExpirationDate.HasValue && product.ProductionDate > product.ExpirationDate)
                return (false, "تاریخ تولید نمی‌تواند بعد از تاریخ انقضا باشد", null);
            if (product.Weight < 0) return (false, "وزن نمی‌تواند منفی باشد", null);
            if (product.CategoryId.HasValue)
            {
                var categoryExists = await _context.Set<Category>().AnyAsync(c => c.CategoryId == product.CategoryId);
                if (!categoryExists) return (false, "دسته‌بندی انتخابی وجود ندارد", null);
            }

            var existing = await GetByIdAsync(product.ProductId);
            if (existing == null) return (false, "محصول یافت نشد", null);
            product.CreatedAt = existing.CreatedAt;
            product.IsActive = existing.IsActive;
            var updated = await UpdateAsync(product);
            return (true, "محصول با موفقیت بروزرسانی شد", updated);
        }

        public async Task<(bool Success, string Message)> DeleteProductAsync(Guid productId)
        {
            var product = await GetByIdAsync(productId);
            if (product == null) return (false, "محصول یافت نشد");
            await RemoveAsync(product);
            return (true, "محصول با موفقیت حذف شد");
        }

        public async Task<(bool Success, string Message)> DeactivateProductAsync(Guid productId)
        {
            var product = await GetByIdAsync(productId);
            if (product == null) return (false, "محصول یافت نشد");
            product.IsActive = false;
            await UpdateAsync(product);
            return (true, "محصول غیرفعال شد");
        }

        public async Task<(bool Success, string Message)> ActivateProductAsync(Guid productId)
        {
            var product = await GetByIdAsync(productId);
            if (product == null) return (false, "محصول یافت نشد");
            product.IsActive = true;
            await UpdateAsync(product);
            return (true, "محصول فعال شد");
        }

        public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(p => p.Category)
                    .OrderBy(p => p.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }
    }
}