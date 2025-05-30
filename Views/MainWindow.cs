using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Presenters;
using Microsoft.Extensions.DependencyInjection;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using Apos_AquaProductManageApp.Services;
using System.Configuration;
using Apos_AquaProductManageApp.Views;


namespace Apos_AquaProductManageApp
{
    public partial class MainWindow : Form
    {
        private TabControl _tabControl = null!;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            InitializeSizeFromConfig();
            Initialize(serviceProvider);
        }
        private void InitializeSizeFromConfig()
        {
            if (int.TryParse(ConfigurationManager.AppSettings["MainWindow.Width"], out int width))
                this.Width = width;
            if (int.TryParse(ConfigurationManager.AppSettings["MainWindow.Height"], out int height))
                this.Height = height;
        }

        private void Initialize(IServiceProvider serviceProvider)
        {
            this.Text = "Fish Farm Management";
            _tabControl = new TabControl { Dock = DockStyle.Fill };

            try
            {
                var dbContext = serviceProvider.GetRequiredService<FishFarmDbContext>();
                dbContext.Database.EnsureCreated();

                // Register tab definitions
                var tabs = new List<Action>
        {
            () => AddTab<CageForm, ICageView, CagePresenter, CageService>("Cages", serviceProvider),
            () => AddTab<StockingForm, IStockingView, StockingPresenter, StockingService>("Fish Stocking", serviceProvider),
            () => AddCustomTab(() => new MortalityForm(), "Fish Mortalities", view =>
            {
                var mortalityService = serviceProvider.GetRequiredService<MortalityService>();
                var balanceService = serviceProvider.GetRequiredService<StockBalanceService>();
                return new MortalityPresenter((IMortalityView)view, mortalityService, balanceService);
            }),
            () => AddCustomTab(() => new TransferForm(), "Fish Transfers", view =>
            {
                var transferService = serviceProvider.GetRequiredService<TransferService>();
                var cageService = serviceProvider.GetRequiredService<CageService>();
                return new TransferPresenter((ITransferView)view, transferService, cageService);
            }),
            () => AddTab<BalanceForm, IBalanceView, BalancePresenter>("Stock Balance", serviceProvider),
            () => AddTab<MortalityPivotForm, IMortalityPivotView, MortalityPivotPresenter>("Mortality Pivot", serviceProvider)
        };

                foreach (var tab in tabs)
                    tab();

                this.Controls.Add(_tabControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTab<TForm, TView, TPresenter, TService>(string title, IServiceProvider services)
    where TForm : Form, TView, new()
    where TPresenter : class
        {
            var form = CreateForm<TForm>();
            var view = (TView)form;
            var service = services.GetRequiredService<TService>();

            var presenter = Activator.CreateInstance(typeof(TPresenter), view, service);
            if (presenter is not TPresenter)
                throw new InvalidOperationException($"Failed to create presenter for {title}");

            ShowFormInTab(form, title);
        }

        private static TForm CreateForm<TForm>() where TForm : Form, new()
        {
            return new TForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
        }

        private void ShowFormInTab(Form form, string title)
        {
            form.Show();
            var tabPage = new TabPage(title);
            tabPage.Controls.Add(form);
            _tabControl.TabPages.Add(tabPage);
        }


        private void AddCustomTab<TForm>(Func<TForm> formFactory, string title, Func<TForm, object> presenterFactory)
    where TForm : Form
        {
            var form = formFactory();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            var presenter = presenterFactory(form);

            ShowFormInTab(form, title);
        }


        private void AddTab<TForm, TView, TPresenter>(string title, IServiceProvider services)
    where TForm : Form, TView, new()
    where TPresenter : class
        {
            var form = CreateForm<TForm>();
            var view = (TView)form;

            var presenter = (TPresenter)ActivatorUtilities.CreateInstance(services, typeof(TPresenter), view);
            ShowFormInTab(form, title);
        }
    }
}
