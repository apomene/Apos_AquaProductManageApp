using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Services;
using Apos_AquaProductManageApp.Views;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp
{
    public partial class TransferForm : Form, ITransferView
    {
        private TransferPresenter _presenter = null!;

        private readonly TransferService _transferService = null!;
        private DateTimePicker dtPicker = null!;
        private ComboBox cbSourceCage = null!;
        private DataGridView gridDestinations = null!;
        private Button btnSaveTransfer = null!;
        private DataGridView gridTransfers = null!;

        private List<Cage> _cages = new();

        public TransferForm(TransferService transferService)
        {
            InitializeComponent();
            Initialize();
            _transferService = transferService;
        }

        private void Initialize()
        {
            this.Text = "Fish Transfers";
            this.Width = 900;
            this.Height = 650;

            dtPicker = new DateTimePicker { Left = 10, Top = 10, Width = 200 };
            dtPicker.ValueChanged += (s, e) => _presenter.LoadTransfers(dtPicker.Value);

            cbSourceCage = new ComboBox { Left = 220, Top = 10, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cbSourceCage.SelectedIndexChanged += CmbSourceCage_SelectedIndexChanged;

            Label lblDest = new Label { Text = "Destination Cages:", Top = 50, Left = 10 };

            gridDestinations = new DataGridView
            {
                Top = 80,
                Left = 10,
                Width = 850,
                Height = 200,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false
            };

            gridDestinations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CageName",
                HeaderText = "Destination Cage",
                DataPropertyName = "CageName",
                ReadOnly = true
            });

            gridDestinations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });

            gridDestinations.ReadOnly = false;
            gridDestinations.AllowUserToAddRows = false;
            gridDestinations.AllowUserToDeleteRows = false;
            gridDestinations.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            gridDestinations.CellEndEdit += gridDestinations_CellEndEdit;



            btnSaveTransfer = new Button { Text = "Save Transfers", Top = 300, Left = 10, Width = 150 };
            btnSaveTransfer.Click += BtnSaveTransfer_Click;

            Label lblTransfers = new Label { Text = "Transfers Made:", Top = 340, Left = 10 };

            gridTransfers = new DataGridView
            {
                Top = 370,
                Left = 10,
                Width = 850,
                Height = 200,
                ReadOnly = true,
                AutoGenerateColumns = true
            };

            this.Controls.Add(dtPicker);
            this.Controls.Add(cbSourceCage);
            this.Controls.Add(lblDest);
            this.Controls.Add(gridDestinations);
            this.Controls.Add(btnSaveTransfer);
            this.Controls.Add(lblTransfers);
            this.Controls.Add(gridTransfers);
        }

        private void CmbSourceCage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadDestinationCages();
        }


        private void gridDestinations_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (gridDestinations.Columns[e.ColumnIndex].Name == "Quantity")
            {
                var row = gridDestinations.Rows[e.RowIndex];
                var entry = row.DataBoundItem as DestinationCageEntry;

                if (entry == null)
                {
                    return;
                }

                if (entry.Quantity < 0)
                {
                    MessageBox.Show("Quantity must be 0 or greater.", "Invalid Input");
                    entry.Quantity = 0;
                    gridDestinations.Refresh();
                }

            }
        }

        private void ExecuteTransfers()
        {
            var sourceCage = cbSourceCage.SelectedItem as Cage;
            if (sourceCage == null)
            {
                MessageBox.Show("Please select a source cage.");
                return;
            }

            int totalQuantity = 0;
            var transfersToMake = new List<(int toCageId, int quantity)>();

            foreach (DataGridViewRow row in gridDestinations.Rows)
            {
                if (row.Cells["Quantity"].Value is int quantity && quantity > 0)
                {
                    var destCage = _cages.FirstOrDefault(c => c.Name == row.Cells["CageName"].Value.ToString());
                    if (destCage != null)
                    {
                        totalQuantity += quantity;
                        transfersToMake.Add((destCage.CageId, quantity));
                    }
                }
            }

            int availableBalance = _transferService.CalculateBalance(sourceCage.CageId, dtPicker.Value.Date);

            if (totalQuantity > availableBalance)
            {
                MessageBox.Show($"Total transfer quantity ({totalQuantity}) exceeds available stock ({availableBalance}). No transfers were made.", "Invalid Transfer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proceed to do transfers
            try
            {
                foreach (var (toCageId, quantity) in transfersToMake)
                {
                    _presenter.TransferFish(sourceCage.CageId, toCageId, dtPicker.Value.Date, quantity);
                }
                MessageBox.Show("Transfers completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transfer failed: {ex.Message}");
            }
            finally
            {
                _presenter.LoadTransfers(dtPicker.Value.Date);               
            }
        }


        private void BtnSaveTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbSourceCage.SelectedItem is Cage sourceCage)
                {
                    ExecuteTransfers();
                }
                else
                {
                    MessageBox.Show("Please select a source cage.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving transfers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DisplayTransfers(List<FishTransfer> transfers)
        {
            gridTransfers.DataSource = null;
            gridTransfers.DataSource = transfers;

            if (gridTransfers.Columns.Contains("FromCage"))
            {
                var fromCageColumn = gridTransfers.Columns["FromCage"];
                if (fromCageColumn != null)
                    fromCageColumn.Visible = false;
            }

            if (gridTransfers.Columns.Contains("ToCage"))
            {
                var toCageColumn = gridTransfers.Columns["ToCage"];
                if (toCageColumn != null)
                    toCageColumn.Visible = false;
            }


        }

        public void DisplayCages(List<Cage> cages)
        {
            _cages = cages;
            cbSourceCage.DataSource = cages;
            cbSourceCage.DisplayMember = "Name";
            cbSourceCage.ValueMember = "CageId";

            // Populate destination cage grid
            LoadDestinationCages();
        }

        private void LoadDestinationCages()
        {
            var sourceCage = cbSourceCage.SelectedItem as Cage;
            var fromCageId = sourceCage!.CageId;
            var destinationOptions = _cages
                .Where(c => c.CageId != fromCageId)
                .Select(c => new DestinationCageEntry { CageName = c.Name, Quantity = 0 })
                .ToList();

            gridDestinations.DataSource = destinationOptions;
            gridDestinations.ReadOnly = false;

            var quantityColumn = gridDestinations.Columns["Quantity"];
            if (quantityColumn != null)
            {
                quantityColumn.ReadOnly = false;
            }

            var cageNameColumn = gridDestinations.Columns["CageName"];
            if (cageNameColumn != null)
            {
                cageNameColumn.ReadOnly = true;
            }

        }


        public class DestinationCageEntry
        {
            public string? CageName { get; set; }
            public int Quantity { get; set; }
        }



        public void SetPresenter(TransferPresenter presenter)
        {
            _presenter = presenter;
            _cages = _presenter.LoadCages();
            _presenter.LoadTransfers(DateTime.Today);
        }
    }
}
