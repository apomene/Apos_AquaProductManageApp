using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Services;
using Apos_AquaProductManageApp.Views;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp
{
    public partial class MortalityForm : Form, IMortalityView
    {
        private MortalityPresenter _presenter = null!;
        private DataGridView gridMortality = null!;
        private DateTimePicker dtPicker = null!;
        private readonly TransferService _transferService;

        public MortalityForm(TransferService transferService)
        {
            InitializeComponent();
            _transferService = transferService;
            Utilities.InitializeFormSizeFromConfig(this, "MortalityForm");
            Initialize();
        }

        private void Initialize()
        {
            this.Text = "Fish Mortality";

            dtPicker = new DateTimePicker { Top = 10, Left = 10, Width = 200 };
            dtPicker.ValueChanged += (s, e) => _presenter.LoadMortalityData(dtPicker.Value.Date);
            this.Controls.Add(dtPicker);

            gridMortality = new DataGridView
            {
                Top = 40,
                Left = 10,
                Width = 800,
                Height = 460,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            gridMortality.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CageName",
                HeaderText = "Cage",
                ReadOnly = true,
                Width = 200
            });

            gridMortality.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Mortality Quantity",
                Width = 150
            });

            gridMortality.CellEndEdit += GridMortality_CellEndEdit;
            gridMortality.CellValidating += GridMortality_CellValidating;

            this.Controls.Add(gridMortality);
        }

        private void GridMortality_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (gridMortality.Columns[e.ColumnIndex].DataPropertyName == "Quantity")
            //{
            //    if (!int.TryParse(e.FormattedValue?.ToString(), out int newQuantity) || newQuantity < 0)
            //    {
            //        MessageBox.Show("Please enter a valid non-negative integer.");
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        var row = gridMortality.Rows[e.RowIndex].DataBoundItem as SetQuantityView;
            //        if (row != null)
            //        {
            //            int balance = _transferService.CalculateBalance(row.CageId, dtPicker.Value.Date);
            //            bool simulated = (row.Quantity-newQuantity) < balance ;

            //            if (simulated)
            //            {
            //                MessageBox.Show("This change would exceed the cage balance.");
            //                e.Cancel = true;
            //            }
            //        }
            //    }
            //}
        }

        private void GridMortality_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = gridMortality.Rows[e.RowIndex].DataBoundItem as SetQuantityView;
                if (row != null)
                {
                    try
                    {
                        _presenter.AddOrUpdateMortality(row.CageId, dtPicker.Value.Date, row.Quantity);
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        _presenter.LoadMortalityData(dtPicker.Value.Date); // Refresh
                    }
                }
            }
        }

        public void SetPresenter(MortalityPresenter presenter)
        {
            _presenter = presenter;
            _presenter.LoadMortalityData(DateTime.Today);
        }

        public void DisplayMortalityData(List<SetQuantityView> cageMortalityViews)
        {
            Utilities.BindDataSource(gridMortality, cageMortalityViews, "Cage");
        }
    }
}


