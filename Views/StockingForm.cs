using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Views;
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
        private Button btnUPdate = null!;
        private Button btnDelete = null!;

        public StockingForm()
        {
            InitializeComponent();
            Utilities.InitializeFormSizeFromConfig(this, "StockingForm");
            Initialize();
        }
     
        private void Initialize()
        {

            dtPicker = new DateTimePicker { Top = 10, Left = 10, Width = 200 };
            dtPicker.ValueChanged += (s, e) => _presenter.LoadStockingData(dtPicker.Value.Date);

            gridAvailable = new DataGridView { Top = 40, Left = 10, Width = 800, Height = 210, AutoGenerateColumns = true };
            gridAvailable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridAvailable.ScrollBars = ScrollBars.Vertical;

             Label lblQuantity = new Label { Text = "Set Quantity", Top = 480, Left = 10 };
            numQuantity = new NumericUpDown { Top = 480, Left = 130, Width = 100, Minimum = 1, Maximum = 100000 };
          
            gridAvailable.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
           
           
            numQuantity.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            SetUpStockGrid();
            SetUpButtons();          

            this.Controls.Add(dtPicker);
            this.Controls.Add(gridAvailable);
            this.Controls.Add(gridStocked);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(numQuantity);          
            this.Text = "Fish Stocking";

        }

        private void SetUpStockGrid()
        {
            gridStocked = new DataGridView { Top = 260, Left = 10, Width = 800, Height = 210, AutoGenerateColumns = true };
            gridStocked.ScrollBars = ScrollBars.Vertical;

            gridStocked.ReadOnly = false;
            gridStocked.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridStocked.AllowUserToAddRows = false;
            gridStocked.AllowUserToDeleteRows = true;
            gridStocked.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            gridStocked.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        }

        private void SetUpButtons()
        {
            btnAdd = new Button { Text = "Add Stocking", Top = 480, Left = 240 };
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
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.Controls.Add(btnAdd);
            btnUPdate = new Button
            {
                Text = "Update",
                Top = 480,
                Left = 340,
                Width = 100
            };
            btnDelete = new Button
            {
                Text = "Delete",
                Top = 480,
                Left = 460,
                Width = 100
            };
            btnUPdate.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnUPdate.Click += BtnUpdateStocking_Click;
            btnDelete.Click += BtnDeleteStocking_Click;
            this.Controls.Add(btnUPdate);
            this.Controls.Add(btnDelete);
        }

        private void BtnUpdateStocking_Click(object sender, EventArgs e)
        {
            if (gridStocked.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to update.");
                return;
            }
            var selected = gridStocked.SelectedRows[0].DataBoundItem as FishStocking;

            if (selected != null)
            {
                var confirm = MessageBox.Show("Are you sure you want to update this entry?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        _presenter.UpdateStocking(selected);
                        _presenter.LoadStockingData(dtPicker.Value.Date);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnDeleteStocking_Click(object sender, EventArgs e)
        {
            if (gridStocked.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }
            var selected= gridStocked.SelectedRows[0].DataBoundItem as FishStocking;

            if (selected != null)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this entry?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        _presenter.DeleteStocking(selected);
                        _presenter.LoadStockingData(dtPicker.Value.Date);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Deletion failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.DataBoundItem is FishStocking stockingToDelete)
            {
                try
                {
                    _presenter.DeleteStocking(stockingToDelete);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Deletion failed: {ex.Message}");
                    e.Cancel = true;
                }
            }
        }



        public void SetPresenter(StockingPresenter presenter) { _presenter = presenter; _presenter.LoadStockingData(DateTime.Today); }
        public void DisplayAvailableCages(List<Cage> cages) { gridAvailable.DataSource = null; gridAvailable.DataSource = cages; }
        public void DisplayStockings(List<FishStocking> stockings)
        {
            Utilities.BindDataSource(gridStocked, stockings, "Cage");
        }

        public void RefreshCageGrid()
        {
            _presenter.LoadStockingData(dtPicker.Value.Date);           
        }

    }
}
