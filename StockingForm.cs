using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp
{
    public partial class StockingForm : Form, IStockingView
    {
        private StockingPresenter _presenter;
        private DataGridView gridAvailable, gridStocked;
        private DateTimePicker dtPicker;
        private NumericUpDown numQuantity;
        private Button btnAdd;

        public StockingForm() 
        { 
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            dtPicker = new DateTimePicker { Top = 10, Left = 10, Width = 200 };
            dtPicker.ValueChanged += (s, e) => _presenter.LoadStockingData(dtPicker.Value.Date);

            gridAvailable = new DataGridView { Top = 40, Left = 10, Width = 300, Height = 150, AutoGenerateColumns = true };
            gridAvailable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            gridStocked = new DataGridView { Top = 200, Left = 10, Width = 300, Height = 150, AutoGenerateColumns = true };

            numQuantity = new NumericUpDown { Top = 360, Left = 10, Width = 100, Minimum = 1, Maximum = 100000 };
            btnAdd = new Button { Text = "Add Stocking", Top = 360, Left = 120 };
            btnAdd.Click += (s, e) =>
            {
                if (gridAvailable.SelectedRows.Count > 0)
                {
                    var cage = (Cage)gridAvailable.SelectedRows[0].DataBoundItem;
                    _presenter.AddStocking(cage.CageId, dtPicker.Value.Date, (int)numQuantity.Value);
                }
            };

            this.Controls.Add(dtPicker);
            this.Controls.Add(gridAvailable);
            this.Controls.Add(gridStocked);
            this.Controls.Add(numQuantity);
            this.Controls.Add(btnAdd);
            this.Text = "Fish Stocking";
            this.Width = 340;
            this.Height = 450;
        }

        public void SetPresenter(StockingPresenter presenter) { _presenter = presenter; _presenter.LoadStockingData(DateTime.Today); }
        public void DisplayAvailableCages(List<Cage> cages) { gridAvailable.DataSource = null; gridAvailable.DataSource = cages; }
        public void DisplayStockings(List<FishStocking> stockings) { gridStocked.DataSource = null; gridStocked.DataSource = stockings; }
    }
}
