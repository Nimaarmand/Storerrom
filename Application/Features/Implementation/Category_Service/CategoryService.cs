using Application.Features.Definition.Context;
using Application.Features.Implementation.Common;
using Application.Features.Implementation.GenericRepository_Service;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Implementation.Category_Service
{
    public class CategoryService : GenericRepository<Category>
    {
        public CategoryService(IApplicationDbContext context) : base(context)
        {
        }

        // ========== متد سفارشی برای دریافت N رکورد اول ==========
        public async Task<IEnumerable<Category>> GetTopCategoryAsync(int count)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .OrderByDescending(c => c.CategoryId)
                    .Take(count)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // ========== سایر متدهای سرویس (تک‌نسخه) ==========
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(c => c.SubCategories)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(c => c.SubCategories)
                    .Include(c => c.Parent)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Include(c => c.SubCategories)
                    .Where(c => c.ParentId == null)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(c => c.ParentId == parentId)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Category>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllCategoriesAsync();

            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(c => c.Name.Contains(name))
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> IsCategoryNameExistsAsync(string name, int? excludeId = null)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var query = _dbSet.Where(c => c.Name == name);
                if (excludeId.HasValue)
                    query = query.Where(c => c.CategoryId != excludeId.Value);
                return await query.AnyAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> IsValidParentCategoryAsync(int? parentId, int? currentCategoryId = null)
        {
            if (!parentId.HasValue) return true;
            // برای دریافت والد از GetByIdAsync که خود قفل دارد استفاده می‌کنیم (نیازی به قفل اضافی نیست)
            var parent = await GetByIdAsync(parentId.Value);
            if (parent == null) return false;
            if (currentCategoryId.HasValue && parentId == currentCategoryId) return false;
            return true;
        }

        public async Task<(bool Success, string Message, Category Category)> CreateCategoryAsync(Category category)
        {
            if (category == null) return (false, "دسته‌بندی نامعتبر است", null);
            if (string.IsNullOrWhiteSpace(category.Name)) return (false, "نام دسته‌بندی نمی‌تواند خالی باشد", null);
            if (await IsCategoryNameExistsAsync(category.Name)) return (false, "دسته‌بندی با این نام قبلاً ثبت شده است", null);
            if (!await IsValidParentCategoryAsync(category.ParentId)) return (false, "والد انتخاب شده نامعتبر است", null);

            category.CreatedAt = DateTime.Now;
            category.IsActive = true;

            var created = await AddAsync(category); // AddAsync قفل دارد
            return (true, "دسته‌بندی با موفقیت اضافه شد", created);
        }

        public async Task<(bool Success, string Message, Category Category)> UpdateCategoryAsync(Category category)
        {
            if (category == null) return (false, "دسته‌بندی نامعتبر است", null);
            if (string.IsNullOrWhiteSpace(category.Name)) return (false, "نام دسته‌بندی نمی‌تواند خالی باشد", null);
            if (await IsCategoryNameExistsAsync(category.Name, category.CategoryId))
                return (false, "دسته‌بندی با این نام قبلاً ثبت شده است", null);
            if (!await IsValidParentCategoryAsync(category.ParentId, category.CategoryId))
                return (false, "والد انتخاب شده نامعتبر است (احتمالاً خود دسته یا نوادگان آن است)", null);

            var existing = await GetByIdAsync(category.CategoryId); // GetByIdAsync قفل دارد
            if (existing == null) return (false, "دسته‌بندی یافت نشد", null);

            category.CreatedAt = existing.CreatedAt;
            category.IsActive = existing.IsActive;

            var updated = await UpdateAsync(category); // UpdateAsync قفل دارد
            return (true, "دسته‌بندی با موفقیت بروزرسانی شد", updated);
        }

        public async Task<(bool Success, string Message)> DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id); // GetCategoryByIdAsync قفل دارد
            if (category == null) return (false, "دسته‌بندی یافت نشد");

            // بررسی زیردسته‌ها نیاز به دسترسی به دیتابیس دارد، بنابراین باید قفل بگیریم
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var hasSub = await _dbSet.AnyAsync(c => c.ParentId == id);
                if (hasSub)
                    return (false, "این دسته‌بندی دارای زیردسته است. ابتدا زیردسته‌ها را حذف یا جابه‌جا کنید.");
            }
            finally
            {
                DbLock.Semaphore.Release();
            }

            await RemoveAsync(category); // RemoveAsync قفل دارد
            return (true, "دسته‌بندی با موفقیت حذف شد");
        }

        public async Task<(bool Success, string Message)> DeactivateCategoryAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category == null) return (false, "دسته‌بندی یافت نشد");
            category.IsActive = false;
            await UpdateAsync(category);
            return (true, "دسته‌بندی غیرفعال شد");
        }

        public async Task<(bool Success, string Message)> ActivateCategoryAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category == null) return (false, "دسته‌بندی یافت نشد");
            category.IsActive = true;
            await UpdateAsync(category);
            return (true, "دسته‌بندی فعال شد");
        }

        public async Task<IEnumerable<Category>> GetPagedAsync(int pageNumber, int pageSize)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .OrderBy(c => c.Name)
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