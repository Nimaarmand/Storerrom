using Application.Features.Definition.GenericRepository;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Implementation.Warehouse_Service
{
    public class WarehouseService
    {
        private readonly IGenericRepository<Warehouse> _warehouseRepo;

        // تزریق سرویس جنریک از طریق سازنده
        public WarehouseService(IGenericRepository<Warehouse> warehouseRepo)
        {
            _warehouseRepo = warehouseRepo ?? throw new ArgumentNullException(nameof(warehouseRepo));
        }

        // ========== متدهای CRUD (همانند سرویس جنریک) ==========
        public async Task<IEnumerable<Warehouse>> GetAllAsync()
            => await _warehouseRepo.GetAllAsync();

        public async Task<Warehouse> GetByIdAsync(int id)
            => await _warehouseRepo.GetByIdAsync(id);

        public async Task<Warehouse> AddAsync(Warehouse warehouse)
            => await _warehouseRepo.AddAsync(warehouse);

        public async Task<Warehouse> UpdateAsync(Warehouse warehouse)
            => await _warehouseRepo.UpdateAsync(warehouse);

        public async Task DeleteAsync(int id)
            => await _warehouseRepo.RemoveByIdAsync(id);

        // ========== متدهای اختصاصی انبار ==========

        /// <summary>
        /// دریافت انبار بر اساس نام (جستجوی دقیق)
        /// </summary>
        public async Task<Warehouse> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var items = await _warehouseRepo.FindAsync(w => w.Name == name);
            return items.FirstOrDefault();
        }

        /// <summary>
        /// دریافت انبارهایی که ظرفیت فعلی (تفاضل Max و Min) کمتر از حد مشخص است
        /// </summary>
        public async Task<IEnumerable<Warehouse>> GetLowCapacityWarehousesAsync(int threshold = 10)
        {
            // فرض می‌کنیم ظرفیت آزاد = Max - Min
            var all = await _warehouseRepo.GetAllAsync();
            return all.Where(w => (w.Max - w.Min) < threshold);
        }

        /// <summary>
        /// دریافت انبارهای با وضعیت مشخص (مقدار Status از نوع string)
        /// </summary>
        public async Task<IEnumerable<Warehouse>> GetByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return new List<Warehouse>();

            return await _warehouseRepo.FindAsync(w => w.Status == status);
        }

        /// <summary>
        /// جستجوی انبارها بر اساس کلمه کلیدی (در نام، موقعیت)
        /// </summary>
        public async Task<IEnumerable<Warehouse>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllAsync();

            keyword = keyword.Trim();
            return await _warehouseRepo.FindAsync(w =>
                w.Name.Contains(keyword) ||
                w.Location.Contains(keyword));
        }

        /// <summary>
        /// دریافت تعداد مشخصی انبار (مثلاً ۱۰ انبار اول)
        /// </summary>
        public async Task<IEnumerable<Warehouse>> GetTopAsync(int count)
        {
            var all = await _warehouseRepo.GetAllAsync();
            return all.Take(count);
        }

        /// <summary>
        /// بررسی یکتا بودن نام انبار (برای جلوگیری از ثبت تکراری)
        /// </summary>
        public async Task<bool> IsWarehouseNameUniqueAsync(string name, int? excludeId = null)
        {
            var warehouses = await _warehouseRepo.FindAsync(w => w.Name == name);
            if (excludeId.HasValue)
                warehouses = warehouses.Where(w => w.WarehouseId != excludeId.Value);
            return !warehouses.Any();
        }

        // در صورت نیاز به متدهای Async بیشتر می‌توانید اضافه کنید
    }
}


