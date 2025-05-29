
using System;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;
using static Apos_AquaProductManageApp.Model.FishFarmModel;
using static Apos_AquaProductManageApp.Presenters.Presenter;

namespace Apos_AquaProductManageApp
{
    public partial class CageForm : Form, ICageView
    {
        private CagePresenter _presenter;
        private DataGridView _cageGrid;
        private TextBox txtName;
        private CheckBox chkIsActive;
        private Button btnAdd, btnDelete, btnUpdate;
        public CageForm()
        {
            InitializeComponent();
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

            btnAdd.Click += (s, e) => _presenter.AddCage(txtName.Text, chkIsActive.Checked);
            btnDelete.Click += (s, e) =>
            {
                if (_cageGrid.SelectedRows.Count > 0)
                    _presenter.DeleteCage(((Cage)_cageGrid.SelectedRows[0].DataBoundItem).CageId);
            };
            btnUpdate.Click += (s, e) =>
            {
                if (_cageGrid.SelectedRows.Count > 0)
                {
                    var selected = (Cage)_cageGrid.SelectedRows[0].DataBoundItem;
                    _presenter.UpdateCage(selected.CageId, txtName.Text, chkIsActive.Checked);
                }
            };

            _cageGrid.SelectionChanged += (s, e) =>
            {
                if (_cageGrid.SelectedRows.Count > 0)
                {
                    var cage = (Cage)_cageGrid.SelectedRows[0].DataBoundItem;
                    txtName.Text = cage.Name;
                    chkIsActive.Checked = cage.IsActive;
                }
            };


            this.Controls.Add(_cageGrid);
            this.Controls.Add(txtName);
            this.Controls.Add(chkIsActive);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);

            this.Text = "Cage Management";
            this.Width = 500;
            this.Height = 350;
        }

        public void SetPresenter(CagePresenter presenter) { _presenter = presenter; }
        public void DisplayCages(List<Cage> cages) { _cageGrid.DataSource = null; _cageGrid.DataSource = cages; }
        }

        

}

