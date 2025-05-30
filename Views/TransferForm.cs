using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using System.ComponentModel;
using System.Configuration;
using System.Security.Cryptography.Xml;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp
{
    public partial class TransferForm : Form, ITransferView
    {
        private TransferPresenter _presenter = null!;

        private DataGridView transfersGrid = null!;
        private DateTimePicker datePicker = null!;
        private ComboBox fromCageComboBox = null!;
        private ComboBox toCageComboBox = null!;
        private NumericUpDown quantityNumeric = null!;
        private Button addButton = null!;


        public TransferForm()
        {
            InitializeComponent();
            InitializeSizeFromConfig();
            Initialize();
            
        }

        private void InitializeSizeFromConfig()
        {
            if (int.TryParse(ConfigurationManager.AppSettings["TransferForm.Width"], out int width))
                this.Width = width;
            if (int.TryParse(ConfigurationManager.AppSettings["TransferForm.Height"], out int height))
                this.Height = height;
        }

        private void Initialize()
        {
            
            transfersGrid = new DataGridView();
            datePicker = new DateTimePicker();
            fromCageComboBox = new ComboBox();
            toCageComboBox = new ComboBox();
            quantityNumeric = new NumericUpDown();
            addButton = new Button { Text = "Add Transfer" };

            transfersGrid.Dock = DockStyle.Top;
            transfersGrid.Height = 200;
            datePicker.Top = 210;
            fromCageComboBox.Top = 240;
            toCageComboBox.Top = 270;
            quantityNumeric.Top = 300;
            addButton.Top = 330;

            datePicker.ValueChanged += (s, e) => _presenter.LoadData(SelectedDate);
            addButton.Click += addButton_Click;

            // Add controls to the form
            Controls.Add(transfersGrid);
            Controls.Add(datePicker);
            Controls.Add(fromCageComboBox);
            Controls.Add(toCageComboBox);
            Controls.Add(quantityNumeric);
            Controls.Add(addButton);
        }


        public DateTime SelectedDate => datePicker.Value.Date;

        public void SetPresenter(TransferPresenter presenter)
        {
            _presenter = presenter;
            _presenter.LoadData(SelectedDate);
        }

        public void DisplayTransfers(List<FishTransfer> transfers)
        {
            transfersGrid.DataSource = null;
            transfersGrid.DataSource = transfers;

            if (transfersGrid.Columns != null)
            {
                if (transfersGrid.Columns["FromCage"] != null && transfersGrid.Columns["ToCage"] != null)
                {
                    var fromCageColumn = transfersGrid.Columns?["FromCage"];
                    var toCageColumn = transfersGrid.Columns?["ToCage"];
                    fromCageColumn!.Visible = false;
                    toCageColumn!.Visible = false;
                }
            }
               
        }

        public void DisplayCages(List<Cage> cages)
        {
            fromCageComboBox.DataSource = new BindingList<Cage>(cages);
            fromCageComboBox.DisplayMember = "Name";
            fromCageComboBox.ValueMember = "CageId";

            toCageComboBox.DataSource = new BindingList<Cage>(cages);
            toCageComboBox.DisplayMember = "Name";
            toCageComboBox.ValueMember = "CageId";
        }


        private void addButton_Click(object? sender, EventArgs e)
        {
            if (fromCageComboBox.SelectedItem is not Cage fromCage ||
                toCageComboBox.SelectedItem is not Cage toCage)
            {
                MessageBox.Show("Please select both From and To cages.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var transfer = new FishTransfer
            {
                FromCageId = fromCage.CageId,
                FromCage = (Cage)fromCageComboBox.SelectedItem,
                ToCageId = toCage.CageId,
                ToCage = (Cage)toCageComboBox.SelectedItem,
                Quantity = (int)quantityNumeric.Value,
                TransferDate = SelectedDate
            };

            try
            {
                _presenter.AddTransfer(transfer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}
