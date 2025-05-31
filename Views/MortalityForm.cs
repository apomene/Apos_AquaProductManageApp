using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Services;
using Apos_AquaProductManageApp.Views;
using System.Configuration;
using System.Data;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp
{
    public partial class MortalityForm : Form, IMortalityView
    {
        private MortalityPresenter _presenter = null!;
        private DateTimePicker dtPicker = null!;
        private DataGridView gridCages = null!;
        private DataGridView gridMortalities = null!;
        private NumericUpDown numQuantity = null!;
        private Button btnAdd = null!;
        private Button btnDeleteMortality = null!;
        private readonly TransferService _transferService;


        public MortalityForm( TransferService transferService) 
        { 
            InitializeComponent();
            _transferService = transferService;
            Utilities.InitializeFormSizeFromConfig(this, "MortalityForm");
            Initialize();
            SetUpGrid();
            SetUpDeleteButton();
        }

        private void SetUpGrid()
        {

            gridMortalities.ScrollBars = ScrollBars.Vertical;

            gridMortalities.ReadOnly = false;
            gridMortalities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridMortalities.AllowUserToAddRows = false;
            gridMortalities.AllowUserToDeleteRows = true;
            gridMortalities.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            gridMortalities.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gridMortalities.CellValidating += grid_CellValidating;
        }

        private void Initialize()
        {
            this.Text = "Mortality Registration";

            dtPicker = new DateTimePicker { Top = 10, Left = 10, Width = 200 };
            dtPicker.ValueChanged += (s, e) => _presenter?.LoadData(dtPicker.Value.Date);

            gridCages = new DataGridView
            {
                Top = 40,
                Left = 10,
                Width = 340,
                Height = 220,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            gridMortalities = new DataGridView
            {
                Top = 40,
                Left = 360,
                Width = 440,
                Height = 220,
                AutoGenerateColumns = true,
                ReadOnly = true
            };

            numQuantity = new NumericUpDown
            {
                Top = 280,
                Left = 10,
                Width = 100,
                Minimum = 1,
                Maximum = 100000
            };

            btnAdd = new Button
            {
                Text = "Add / Update",
                Top = 280,
                Left = 120,
                Width = 100
            };
            btnAdd.Click += (s, e) =>
            {
                try
                {
                    if (gridCages.SelectedRows.Count > 0)
                    {
                        var selectedItem = gridCages.SelectedRows[0].DataBoundItem;
                        if (selectedItem is Cage cage)
                        {
                            int quantity = (int)numQuantity.Value;
                            _presenter.AddOrUpdateMortality(cage.CageId, dtPicker.Value.Date, quantity);
                        }
                        else
                        {
                            MessageBox.Show("Invalid selection. Please select a valid cage.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select a cage first.");
                    }            
            }
                catch (Exception ex)
                {
                    MessageBox.Show($"failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            };
            this.Controls.Add(dtPicker);
            this.Controls.Add(gridCages);
            this.Controls.Add(gridMortalities);
            this.Controls.Add(numQuantity);
            this.Controls.Add(btnAdd);
        }

        private void SetUpDeleteButton()
        {
           
           
            btnDeleteMortality = new Button
            {
                Text = "Delete Mortality",
                Top = 280,
                Left = 360,
                Width = 120
            };
            btnDeleteMortality.Click += BtnDeleteMortality_Click;
            this.Controls.Add(btnDeleteMortality);
        }

        private void grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var grid = (DataGridView)sender;

            if (grid.Columns[e.ColumnIndex].Name == "Quantity")
            {
                if (int.TryParse(e.FormattedValue?.ToString(), out int newQuantity))
                {
                    var selection = (Mortality)grid.Rows[e.RowIndex].DataBoundItem;

                    int balance = _transferService.CalculateBalance(selection.CageId, selection.MortalityDate);

                    int maxAllowed = balance + selection.Quantity;

                    if (newQuantity > maxAllowed)
                    {
                        MessageBox.Show(
                            $"This update would exceed the available fish stock. Max allowed: {maxAllowed}",
                            "Invalid Operation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }


        private void BtnDeleteMortality_Click(object sender, EventArgs e)
        {
            if (gridMortalities.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }
            var selectedMortality = gridMortalities.SelectedRows[0].DataBoundItem as Mortality;

            if (selectedMortality != null)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this entry?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        _presenter.DeleteMortality(selectedMortality);
                        _presenter?.LoadData(dtPicker.Value.Date);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Deletion failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        public void SetPresenter(MortalityPresenter presenter)
        {
            _presenter = presenter;
            _presenter.LoadData(DateTime.Today);
        }

        public DateTime GetSelectedDate() => dtPicker.Value.Date;

        public void DisplayEligibleCages(List<Cage> cages)
        {
            gridCages.DataSource = null;
            gridCages.DataSource = cages;
        }

        public void DisplayMortalities(List<Mortality> mortalities)
        {
            gridMortalities.DataSource = null;
            gridMortalities.DataSource = mortalities;
            gridMortalities.Columns["Cage"].Visible = false;
            // or show only Cage.Name:
            gridMortalities.Columns["Cage"].DisplayIndex = 0;

        }
    }

}
