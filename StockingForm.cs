using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp
{
    public partial class StockingForm : Form, IStockingView
    {
        private StockingPresenter _presenter =null!;
        private DataGridView gridAvailable = null!, gridStocked = null!;
        private DateTimePicker dtPicker =null!;
        private NumericUpDown numQuantity = null!;
        private Button btnAdd = null!;

        public StockingForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {

            dtPicker = new DateTimePicker { Top = 10, Left = 10, Width = 200 };
            dtPicker.ValueChanged += (s, e) => _presenter.LoadStockingData(dtPicker.Value.Date);

            gridAvailable = new DataGridView { Top = 40, Left = 10, Width = 800, Height = 180, AutoGenerateColumns = true };
            gridAvailable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridAvailable.ScrollBars = ScrollBars.Vertical;

            gridStocked = new DataGridView { Top = 280, Left = 10, Width = 800, Height = 180, AutoGenerateColumns = true };
            gridStocked.ScrollBars = ScrollBars.Vertical;
            Label lblQuantity = new Label { Text = "Set Quantity", Top = 480, Left = 10 };
            numQuantity = new NumericUpDown { Top = 480, Left = 130, Width = 100, Minimum = 1, Maximum = 100000 };
            btnAdd = new Button { Text = "Add Stocking", Top = 480, Left = 280 };
            gridAvailable.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gridStocked.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            numQuantity.Anchor = AnchorStyles.Top | AnchorStyles.Left;


            btnAdd.Click += (s, e) =>
            {

                if (gridAvailable.SelectedRows.Count > 0 && gridAvailable.SelectedRows[0].DataBoundItem is Cage cage)
                {
                    _presenter.AddStocking(cage.CageId, dtPicker.Value.Date, (int)numQuantity.Value);
                }
                else
                {                   
                    MessageBox.Show("Please select a valid cage.");
                }
            };

            this.Controls.Add(dtPicker);
            this.Controls.Add(gridAvailable);
            this.Controls.Add(gridStocked);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(numQuantity);
            this.Controls.Add(btnAdd);
            this.Text = "Fish Stocking";

        }

        public void SetPresenter(StockingPresenter presenter) { _presenter = presenter; _presenter.LoadStockingData(DateTime.Today); }
        public void DisplayAvailableCages(List<Cage> cages) { gridAvailable.DataSource = null; gridAvailable.DataSource = cages; }
        public void DisplayStockings(List<FishStocking> stockings)
        {
            gridStocked.DataSource = null;
            gridStocked.DataSource = stockings;
            // Ensure columns are generated before trying to access them
            if (gridStocked.Columns["Cage"] != null)
            {
                gridStocked.Columns["Cage"].Visible = false;
            }
        }
    }
}
