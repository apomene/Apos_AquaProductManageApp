
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
        public CageForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            this._cageGrid = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = true };
            var btnLoad = new Button { Text = "Load Cages", Dock = DockStyle.Top };
            btnLoad.Click += (s, e) => _presenter.LoadCages();

            this.Controls.Add(_cageGrid);
            this.Controls.Add(btnLoad);
            this.Text = "Cage Management";
            this.Width = 600;
            this.Height = 400;
        }

        public void SetPresenter(CagePresenter presenter)
        {
            _presenter = presenter;
        }

        public void DisplayCages(List<Cage> cages)
        {
            _cageGrid.DataSource = cages;
        }
    }

}

