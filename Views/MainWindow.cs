using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Presenters;
using Microsoft.Extensions.DependencyInjection;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using Apos_AquaProductManageApp.Services;
using Apos_AquaProductManageApp.Views;

namespace Apos_AquaProductManageApp
{
    public partial class MainWindow : Form
    {
        private TabControl _tabControl = null!;
        private TabPage _fishStockingTab = null!;
        private TabPage _mortalityTab = null!;
        private TabPage _stockBalanceTab = null!;
        private TransferPresenter? _transferPresenter;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Utilities.InitializeFormSizeFromConfig(this, "MainWindow");
            Initialize(serviceProvider);
        }

        private void Initialize(IServiceProvider serviceProvider)
        {
            this.Text = "Fish Farm Management";
            _tabControl = new TabControl { Dock = DockStyle.Fill };
            _tabControl.SelectedIndexChanged += (s, e) =>
            {
                var form = _tabControl.SelectedTab?.Controls.OfType<Form>().FirstOrDefault();
                if (form != null && !form.Visible)
                {
                    form.Show();
                }

               

                if (_tabControl.SelectedTab == _fishStockingTab)
                {
                    var stockingForm = _fishStockingTab.Controls.OfType<StockingForm>().FirstOrDefault();
                    stockingForm?.RefreshCageGrid();
                }

                if (_tabControl.SelectedTab == _stockBalanceTab)
                {
                    var balanceForm = _stockBalanceTab.Controls.OfType<BalanceForm>().FirstOrDefault();
                    balanceForm?.RefreshBalance();
                }

                if (_tabControl.SelectedTab?.Text == "Fish Mortalities")
                {
                    var mortalityForm = _mortalityTab.Controls.OfType<MortalityForm>().FirstOrDefault();
                    mortalityForm?.RefreshCageGrid();
                }

                if (_tabControl.SelectedTab?.Text == "Fish Transfers")
                {
                    _transferPresenter?.LoadCages();
                }
            };

            try
            {
                var dbContext = serviceProvider.GetRequiredService<FishFarmDbContext>();
                dbContext.Database.EnsureCreated();

                var tabs = new List<Action>
                {
                    () => AddTab<CageForm, ICageView, CagePresenter, CageService>("Cages", serviceProvider),
                    () => _fishStockingTab = AddTab<StockingForm, IStockingView, StockingPresenter, StockingService>(
                        "Fish Stocking",
                        serviceProvider,
                        () => new StockingForm(serviceProvider.GetRequiredService<TransferService>()),
                        form =>   new StockingPresenter(
                            (IStockingView)form,
                            serviceProvider.GetRequiredService<StockingService>())),
                    () => AddCustomTab(() => new MortalityForm(), "Fish Mortalities", view =>
                    {
                        var mortalityService = serviceProvider.GetRequiredService<MortalityService>();
                        var presenter = new MortalityPresenter((IMortalityView)view, mortalityService);
                        return presenter;
                    }),
                    () => _mortalityTab = AddCustomTab(() => new TransferForm(serviceProvider.GetRequiredService<TransferService>()), "Fish Transfers", view =>
                    {
                        var transferService = serviceProvider.GetRequiredService<TransferService>();
                        var presenter = new TransferPresenter((ITransferView)view, transferService);
                        _transferPresenter = presenter;
                        return presenter;
                    }),
                    () => _stockBalanceTab = AddTab<BalanceForm, IBalanceView, BalancePresenter>("Stock Balance", serviceProvider),
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

        private TabPage AddTab<TForm, TView, TPresenter, TService>(
            string title,
            IServiceProvider serviceProvider,
            Func<TForm> formFactory,
            Func<TForm, TPresenter> presenterFactory)
            where TForm : Form
        {
            TForm form = formFactory();
            TPresenter presenter = presenterFactory(form);

            TabPage tabPage = new TabPage(title);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tabPage.Controls.Add(form);
            form.Show();

            _tabControl.TabPages.Add(tabPage);

            return tabPage;
        }

        private TabPage AddTab<TForm, TView, TPresenter, TService>(string title, IServiceProvider services)
            where TForm : Form, TView, new()
            where TPresenter : class
        {
            var form = CreateForm<TForm>();
            var view = (TView)form;
            var service = services.GetRequiredService<TService>();

            var presenter = Activator.CreateInstance(typeof(TPresenter), view, service);
            if (presenter is not TPresenter)
                throw new InvalidOperationException($"Failed to create presenter for {title}");

            return ShowFormInTab(form, title);
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

        private TabPage ShowFormInTab(Form form, string title)
        {
            form.Show();
            var tabPage = new TabPage(title);
            tabPage.Controls.Add(form);
            _tabControl.TabPages.Add(tabPage);
            return tabPage;
        }

        private TabPage AddCustomTab<TForm>(Func<TForm> formFactory, string title, Func<TForm, object> presenterFactory)
      where TForm : Form
        {
            var form = formFactory();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            var presenter = presenterFactory(form);

            return ShowFormInTab(form, title);
        }


        private TabPage AddTab<TForm, TView, TPresenter>(string title, IServiceProvider services)
            where TForm : Form, TView, new()
            where TPresenter : class
        {
            var form = CreateForm<TForm>();
            var view = (TView)form;

            var presenter = (TPresenter)ActivatorUtilities.CreateInstance(services, typeof(TPresenter), view);
            return ShowFormInTab(form, title);
        }
    }
}
