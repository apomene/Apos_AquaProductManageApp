using Apos_AquaProductManageApp.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Services;
using System.Configuration;

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


            string connectionString = ConfigurationManager.ConnectionStrings["FishFarmDb"].ConnectionString;

            services.AddDbContext<FishFarmDbContext>(options =>
                options.UseSqlServer(connectionString)); services.AddTransient<CageService>();
            services.AddTransient<StockingService>();
            services.AddTransient<MortalityService>();
            services.AddTransient<StockBalanceService>();
            services.AddTransient<TransferService>();


            var provider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainWindow = new MainWindow(provider);
            Application.Run(mainWindow);

        }
    }
}