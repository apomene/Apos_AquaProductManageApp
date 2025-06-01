using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Services;
using Apos_AquaProductManageApp.Views;
using System.ComponentModel;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Apos_AquaProductManageApp
{
    public partial class StockingForm : Form, IStockingView
    {
        private StockingPresenter _presenter = null!;
        private DateTimePicker _dtPicker = null!;
        private DataGridView _grid = null!;
        private readonly TransferService _transferService;

        private List<Cage> _allCages = new();
        private List<FishStocking> _stockings = new();

        public StockingForm(TransferService transferService)
        {
            InitializeComponent();
            _transferService = transferService;
            Utilities.InitializeFormSizeFromConfig(this, "StockingForm");
            Initialize();
        }

        private void Initialize()
        {
            _dtPicker = new DateTimePicker { Top = 10, Left = 10, Width = 200 };
            _dtPicker.ValueChanged += (s, e) =>
            {
                var data = _presenter.GetMergedCageStockings(_dtPicker.Value.Date);
                DisplayStockings(data);
            };

            Controls.Add(_dtPicker);

            _grid = new DataGridView
            {
                Top = 50,
                Left = 10,
                Width = 850,
                Height = 550,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                EditMode = DataGridViewEditMode.EditOnEnter
            };
            _grid.CellValidating += Grid_CellValidating;
            _grid.CellEndEdit += gridStocked_CellEndEdit;

            Controls.Add(_grid);

            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            _grid.Columns.Clear();
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cage ID", DataPropertyName = "CageId", ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cage Name", DataPropertyName = "CageName", ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quantity", DataPropertyName = "Quantity" });
        }

   
        public void SetPresenter(StockingPresenter presenter)
        {
            _presenter = presenter;
            var mergedData = _presenter.GetMergedCageStockings(DateTime.Today);
            DisplayStockings(mergedData);
        }

        public void DisplayAvailableCages(List<Cage> cages)
        {
            _allCages = cages;
            MergeCagesWithStocking();
        }

        public void DisplayStockings(List<CageStockingView> data)
        {
            _grid.Columns.Clear();
            _grid.AutoGenerateColumns = false;
            _grid.ReadOnly = false;
            _grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;

            
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CageName",
                HeaderText = "Cage",
                ReadOnly = true,
                Width = 150
            });

            
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Quantity",
                ReadOnly = false,
                Width = 100
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CageId",
                Name = "CageId",
                Visible = false 
            });


            _grid.DataSource = new BindingList<CageStockingView>(data);
        }


        private void MergeCagesWithStocking()
        {
            if (_allCages == null || _stockings == null) return;

            var combined = _allCages
                .Select(cage =>
                {
                    var stocking = _stockings.FirstOrDefault(s => s.CageId == cage.CageId);
                    return new
                    {
                        CageId = cage.CageId,
                        CageName = cage.Name,
                        Quantity = stocking?.Quantity ?? 0
                    };
                })
                .ToList();

            _grid.DataSource = combined;
        }

        private void Grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (_grid.Columns[e.ColumnIndex].Name == "Quantity")
            {
                if (int.TryParse(e.FormattedValue.ToString(), out int newQuantity))
                {
                    var row = _grid.Rows[e.RowIndex];
                    var data = (CageStockingView)row.DataBoundItem;

                    int currentQty = data.Quantity;
                    int simulatedBalance = _transferService.CalculateBalance(data.CageId, _dtPicker.Value.Date)
                                              - currentQty + newQuantity;

                    if (simulatedBalance < 0)
                    {
                        MessageBox.Show("This update would result in a negative stock balance.", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void gridStocked_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var updated = _grid.Rows[e.RowIndex].DataBoundItem as CageStockingView;

                if (updated != null)
                {
                    try
                    {
                        _presenter.AddOrUpdateStocking(updated.CageId, _dtPicker.Value.Date, updated.Quantity);
                        _presenter.LoadStockingData(_dtPicker.Value.Date); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    
        public void RefreshCageGrid()
        {
            _presenter.LoadStockingData(_dtPicker.Value.Date);
        }


    }

}
