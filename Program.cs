using Apos_AquaProductManageApp.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Services;

namespace Apos_AquaProductManageApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FishFarmDbContext>(options =>
                options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FishFarmDB;Trusted_Connection=True;TrustServerCertificate=True;"));
            services.AddTransient<CageService>();
            services.AddTransient<StockingService>();
            services.AddTransient<MortalityService>();
            services.AddTransient<StockBalanceService>();


            var provider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //var stockingForm = new StockingForm();
            //var presenter = new StockingPresenter(stockingForm, provider.GetRequiredService<StockingService>());
            var mainWindow = new MainWindow(provider);
            Application.Run(mainWindow);

        }
    }
}