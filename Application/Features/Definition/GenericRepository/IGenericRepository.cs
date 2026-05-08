using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.Features.Definition.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        // دریافت همه رکوردها
        Task<IEnumerable<T>> GetAllAsync();

        // دریافت رکوردها با شرط (فیلتر)
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // دریافت یک رکورد بر اساس کلید اصلی
        Task<T> GetByIdAsync(params object[] keyValues);

        // دریافت تعداد مشخصی رکورد (از ابتدا)
        Task<IEnumerable<T>> TakeAsync(int count);

        // دریافت تعداد مشخصی رکورد با جا به جایی (برای صفحه‌بندی)
        Task<IEnumerable<T>> TakeAsync(int skip, int take);

        // اضافه کردن رکورد جدید
        Task<T> AddAsync(T entity);

        // اضافه کردن چند رکورد
        Task AddRangeAsync(IEnumerable<T> entities);

        // بروزرسانی رکورد
        Task<T> UpdateAsync(T entity);

        // حذف رکورد
        Task<T> RemoveAsync(T entity);

        // حذف بر اساس کلید اصلی
        Task RemoveByIdAsync(params object[] keyValues);

        // ذخیره تغییرات
        Task<int> SaveChangesAsync();
    }
}
