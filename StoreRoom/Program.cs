using Application.Features.Implementation.Customer_Service;
using Application.Features.Implementation.GenericRepository_Service; 
using Application.Features.Implementation.GoodsIssue_Service;
using Application.Features.Implementation.GoodsReceipt_Service;
using Application.Features.Implementation.Supplier_Service;
using Application.Features.Implementation.Warehouse_Service;
using Domain.Entity;
using Persistans.Context;
using System.Windows.Forms;

namespace StoreRoom
{
    internal static class Program
    {
        public static WarehouseService WarehouseService { get; private set; }
        public static SupplierService SupplierService { get; private set; }
        public static GoodsReceiptService GoodsReceiptService { get; private set; }
        public static GoodsIssueService GoodsIssueService { get; private set; }
        public static CustomerService CustomerService { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var dbContext = new ApplicationDbContext();

            // WarehouseService (نیاز به ریپازیتوری دارد)
            var warehouseRepo = new GenericRepository<Warehouse>(dbContext);
            WarehouseService = new WarehouseService(warehouseRepo);

            // سایر سرویس‌ها (از GenericRepository<T> ارث‌بری کرده‌اند)
            SupplierService = new SupplierService(dbContext);
            GoodsReceiptService = new GoodsReceiptService(dbContext);
            GoodsIssueService = new GoodsIssueService(dbContext);
            CustomerService = new CustomerService(dbContext);

            System.Windows.Forms.Application.Run(new Form1());
        }
    }
}