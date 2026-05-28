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

        public WarehouseService(IGenericRepository<Warehouse> warehouseRepo)
        {
            _warehouseRepo = warehouseRepo ?? throw new ArgumentNullException(nameof(warehouseRepo));
        }

        // ========== CRUD ==========
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


        public Task<IEnumerable<Warehouse>> GetTopWarehousesAsync(int count)
        {
            return _warehouseRepo.TakeAsync(count);
        }

        // ========== متدهای اختصاصی ==========
        public async Task<Warehouse> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var items = await _warehouseRepo.FindAsync(w => w.Name == name);
            return items.FirstOrDefault();
        }

        public async Task<IEnumerable<Warehouse>> GetLowCapacityWarehousesAsync(int threshold = 10)
        {
            var all = await _warehouseRepo.GetAllAsync();
            return all.Where(w => (w.Max - w.Min) < threshold);
        }

        public async Task<IEnumerable<Warehouse>> GetByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return new List<Warehouse>();
            return await _warehouseRepo.FindAsync(w => w.Status == status);
        }

        public async Task<IEnumerable<Warehouse>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllAsync();
            keyword = keyword.Trim();
            return await _warehouseRepo.FindAsync(w =>
                w.Name.Contains(keyword) || w.Location.Contains(keyword));
        }

        public async Task<IEnumerable<Warehouse>> GetTopAsync(int count)
        {
            var all = await _warehouseRepo.GetAllAsync();
            return all.Take(count);
        }

        public async Task<bool> IsWarehouseNameUniqueAsync(string name, int? excludeId = null)
        {
            var warehouses = await _warehouseRepo.FindAsync(w => w.Name == name);
            if (excludeId.HasValue)
                warehouses = warehouses.Where(w => w.WarehouseId != excludeId.Value);
            return !warehouses.Any();
        }
    }
}


