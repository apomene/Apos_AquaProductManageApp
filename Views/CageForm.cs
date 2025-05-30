
using Apos_AquaProductManageApp.Presenters;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Views;



namespace Apos_AquaProductManageApp
{
    public partial class CageForm : Form, ICageView
    {
        private CagePresenter _presenter = null!;
        private DataGridView _cageGrid = null!;
        private TextBox txtName = null!;
        private CheckBox chkIsActive = null!;
        private Button btnAdd = null!, btnDelete = null!, btnUpdate = null!;
        public CageForm()
        {
            InitializeComponent();
            Utilities.InitializeFormSizeFromConfig(this, "CageForm");

            Initialize();
        }
      
        private void Initialize()
        {
            _cageGrid = new DataGridView { Dock = DockStyle.Top, Height = 200, AutoGenerateColumns = true };
            _cageGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _cageGrid.MultiSelect = false;

            txtName = new TextBox { PlaceholderText = "Name", Top = 210, Left = 10, Width = 200 };
            chkIsActive = new CheckBox { Text = "Is Active", Top = 210, Left = 220 };

            btnAdd = new Button { Text = "Add", Top = 240, Left = 10 };
            btnUpdate = new Button { Text = "Update", Top = 240, Left = 90 };
            btnDelete = new Button { Text = "Delete", Top = 240, Left = 170 };

            InitializeEventHandlers();

            this.Controls.Add(_cageGrid);
            this.Controls.Add(txtName);
            this.Controls.Add(chkIsActive);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);

            this.Text = "Cage Management";
        }

        public void SetPresenter(CagePresenter presenter) { _presenter = presenter; }
        public void DisplayCages(List<Cage> cages) { _cageGrid.DataSource = null; _cageGrid.DataSource = cages; }

        private Cage? GetSelectedCage()
        {
            if (_cageGrid.SelectedRows.Count > 0 &&
                _cageGrid.SelectedRows[0].DataBoundItem is Cage cage)
            {
                return cage;
            }

            return null;
        }

        private void InitializeEventHandlers()
        {
            btnAdd.Click += (s, e) => _presenter.AddCage(txtName.Text, chkIsActive.Checked);

            btnDelete.Click += (s, e) =>
            {
                var cage = GetSelectedCage();
                if (cage != null)
                {
                    _presenter.DeleteCage(cage.CageId);
                }
            };

            btnUpdate.Click += (s, e) =>
            {
                var cage = GetSelectedCage();
                if (cage != null)
                {
                    _presenter.UpdateCage(cage.CageId, txtName.Text, chkIsActive.Checked);
                }
            };

            _cageGrid.SelectionChanged += (s, e) =>
            {
                var cage = GetSelectedCage();
                if (cage != null)
                {
                    txtName.Text = cage.Name;
                    chkIsActive.Checked = cage.IsActive;
                }
            };
        }

    }



}

