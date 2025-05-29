using Apos_AquaProductManageApp.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Apos_AquaProductManageApp.Presenters;

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

            var provider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var cageForm = new CageForm();
            var presenter = new CagePresenter(cageForm, provider.GetRequiredService<CageService>());
            Application.Run(cageForm);
        }
    }
}