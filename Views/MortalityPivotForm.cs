using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Services.TransferService;

namespace Apos_AquaProductManageApp.Views
{
    public partial class MortalityPivotForm : Form, IMortalityPivotView
    {
        private MortalityPivotPresenter _presenter = null!;
        private DataGridView gridPivot = null!;
        private CheckBox chkCage = null!;
        private CheckBox chkYear = null!;
        private CheckBox chkMonth = null!;

        public MortalityPivotForm()
        {
            InitializeComponent();
            Utilities.InitializeFormSizeFromConfig(this, "MortalityPivotForm");
            InitializePivotCheckboxes();
            Initialize();
        }

        private void InitializePivotCheckboxes()
        {
            chkCage = new CheckBox
            {
                Name = "chkCage",
                Text = "Cage",
                Location = new Point(20, 380),
                AutoSize = true,
                Checked = true
            };

            chkYear = new CheckBox
            {
                Name = "chkYear",
                Text = "Year",
                Location = new Point(20, 410),
                AutoSize = true,
                Checked = true
            };

            chkMonth = new CheckBox
            {
                Name = "chkMonth",
                Text = "Month",
                Location = new Point(20, 440),
                AutoSize = true,
                Checked = true
            };

            this.Controls.Add(chkCage);
            this.Controls.Add(chkYear);
            this.Controls.Add(chkMonth);
        }


        private void Initialize()
        {
            this.Text = "Mortality Pivot Analysis";
           

            gridPivot = new DataGridView {  ReadOnly = true, AutoGenerateColumns = true };
            gridPivot.Width = 550;
            gridPivot.Height = 300;
            Controls.Add(gridPivot);
        }

        public void SetPresenter(MortalityPivotPresenter presenter)
        {
            _presenter = presenter;
        }

        public void DisplayPivot(List<MortalityPivot> pivot)
        {
            gridPivot.DataSource = pivot;
        }

        public List<MortalityDimension> GetSelectedDimensions()
        {
            var dimensions = new List<MortalityDimension>();

            if (chkCage.Checked) dimensions.Add(MortalityDimension.Cage);
            if (chkYear.Checked) dimensions.Add(MortalityDimension.Year);
            if (chkMonth.Checked) dimensions.Add(MortalityDimension.Month);

            return dimensions;
        }

        private void LoadPivot_Click(object sender, EventArgs e)
        {
            var selectedDimensions = GetSelectedDimensions();
            _presenter.LoadPivot(selectedDimensions);
        }
    }

}
