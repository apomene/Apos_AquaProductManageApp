using Apos_AquaProductManageApp.DBContext;
using Apos_AquaProductManageApp.Presenters;
using Microsoft.Extensions.DependencyInjection;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using System.Windows.Forms;



namespace Apos_AquaProductManageApp
{
    public partial class MainWindow : Form
    {
        private TabControl _tabControl=null!;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Initialize(serviceProvider);
        }

        private void Initialize(IServiceProvider serviceProvider)
        {
            this.Text = "Fish Farm Management";
            this.Width = 800;
            this.Height = 600;

            _tabControl = new TabControl { Dock = DockStyle.Fill };

            try
            {
                // Initialize the database context
                var dbContext = serviceProvider.GetRequiredService<FishFarmDbContext>();
                dbContext.Database.EnsureCreated();
                AddTabWithPresenter<CageForm, ICageView, CagePresenter, CageService>(
              "Cages", serviceProvider);

                AddTabWithPresenter<StockingForm, IStockingView, StockingPresenter, StockingService>(
                    "Fish Stocking", serviceProvider);

                this.Controls.Add(_tabControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTabWithPresenter<TForm, TView, TPresenter, TService>(
        string tabTitle, IServiceProvider serviceProvider)
        where TForm : Form, TView, new()
        where TPresenter : class
        {
            var form = new TForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            var view = (TView)form;
            TService? service = serviceProvider.GetRequiredService<TService>();

            var presenterObj = Activator.CreateInstance(typeof(TPresenter), view, service);

            if (presenterObj is not TPresenter presenter)
            {
                throw new InvalidOperationException($"Could not create instance of type {typeof(TPresenter).Name}.");
            }
      

            form.Show();

            var tabPage = new TabPage(tabTitle);
            tabPage.Controls.Add(form);
            _tabControl.TabPages.Add(tabPage);
        }


    }

}
