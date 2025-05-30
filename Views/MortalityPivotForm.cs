using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Views
{
    public partial class MortalityPivotForm : Form, IMortalityPivotView
    {
        private MortalityPivotPresenter _presenter = null!;
        private DataGridView gridPivot = null!;

        public MortalityPivotForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            this.Text = "Mortality Pivot Analysis";
            this.Dock = DockStyle.Fill;

            gridPivot = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
            Controls.Add(gridPivot);
        }

        public void SetPresenter(MortalityPivotPresenter presenter)
        {
            _presenter = presenter;
            _presenter.LoadPivot();
        }

        public void DisplayPivot(List<MortalityPivot> pivot)
        {
            gridPivot.DataSource = pivot;
        }
    }

}
