using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using Apos_AquaProductManageApp.Views;
using System.ComponentModel;
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
            Utilities.InitializeFormSizeFromConfig(this, "TransferForm");
            Initialize();
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
            transfersGrid.Top = 10;
            transfersGrid.Height = 380;
            datePicker.Top = 400;
            datePicker.Left = 10;
            datePicker.Width = 230;
            Label lblFrom = new Label { Text = "From Cage", Top = 430, Left = 10 };
            fromCageComboBox.Top = 430;
            fromCageComboBox.Left = 120;
            Label lblTo = new Label { Text = "To Cage", Top = 460, Left = 10 };
            toCageComboBox.Top = 460;
            toCageComboBox.Left = 120;
            Label lblQuantity = new Label { Text = "Set Quantity", Top = 490, Left = 10 };
            quantityNumeric.Top = 490;
            quantityNumeric.Left = 120;
            addButton.Top = 400;
            addButton.Left = 300;

            datePicker.ValueChanged += (s, e) => _presenter.LoadData(SelectedDate);
            addButton.Click += addButton_Click;

            // Add controls to the form
            Controls.Add(transfersGrid);
            Controls.Add(datePicker);
            Controls.Add(fromCageComboBox);
            Controls.Add(toCageComboBox);
            Controls.Add(lblFrom);
            Controls.Add(lblTo);
            Controls.Add(quantityNumeric);
            Controls.Add(lblQuantity);
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
            Utilities.BindDataSource(transfersGrid, transfers, "ToCage", "FromCage");             
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
