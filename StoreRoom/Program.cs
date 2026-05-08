namespace StoreRoom
{
    internal static class Program
    {
        //public static WarehouseService WarehouseService { get; private set; }
        //public static ProductService ProductService { get; private set; }
        //public static SupplierService SupplierService { get; private set; }
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}