using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using System.Configuration;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Views
{
    public partial class BalanceForm : Form, IBalanceView
    {
        private BalancePresenter _presenter = null!;
        private DataGridView gridBalances = null!;
        private DateTimePicker datePicker = null!;

        public BalanceForm()
        {
            InitializeComponent();
            InitializeSizeFromConfig();
            Initialize();
        }

        private void InitializeSizeFromConfig()
        {
            if (int.TryParse(ConfigurationManager.AppSettings["BalanceForm.Width"], out int width))
                this.Width = width;
            if (int.TryParse(ConfigurationManager.AppSettings["BalanceForm.Height"], out int height))
                this.Height = height;
        }

        private void Initialize()
        {
            this.Text = "Stock Balance";
            this.Dock = DockStyle.Fill;

            datePicker = new DateTimePicker { Dock = DockStyle.Top };
            gridBalances = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };

            datePicker.ValueChanged += (s, e) => _presenter.LoadBalances(datePicker.Value.Date);

            Controls.Add(gridBalances);
            Controls.Add(datePicker);
        }

        public void SetPresenter(BalancePresenter presenter)
        {
            _presenter = presenter;
            _presenter.LoadBalances(datePicker.Value.Date);
        }

        public void DisplayBalances(List<StockBalance> balances)
        {
            gridBalances.DataSource = balances;
        }
    }

}
