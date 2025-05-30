using Apos_AquaProductManageApp.Model;
using Apos_AquaProductManageApp.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public MortalityForm() 
        { 
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            this.Text = "Mortality Registration";
            this.Width = 700;
            this.Height = 500;

            dtPicker = new DateTimePicker { Top = 10, Left = 10, Width = 200 };
            dtPicker.ValueChanged += (s, e) => _presenter?.LoadData(dtPicker.Value.Date);

            gridCages = new DataGridView
            {
                Top = 40,
                Left = 10,
                Width = 300,
                Height = 150,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            gridMortalities = new DataGridView
            {
                Top = 40,
                Left = 330,
                Width = 340,
                Height = 150,
                AutoGenerateColumns = true,
                ReadOnly = true
            };

            numQuantity = new NumericUpDown
            {
                Top = 210,
                Left = 10,
                Width = 100,
                Minimum = 1,
                Maximum = 100000
            };

            btnAdd = new Button
            {
                Text = "Add / Update",
                Top = 210,
                Left = 120,
                Width = 100
            };
            btnAdd.Click += (s, e) =>
            {
                if (gridCages.SelectedRows.Count > 0)
                {
                    var cage = (Cage)gridCages.SelectedRows[0].DataBoundItem;
                    int quantity = (int)numQuantity.Value;
                    _presenter.AddOrUpdateMortality(cage.CageId, dtPicker.Value.Date, quantity);
                }
                else
                {
                    MessageBox.Show("Select a cage first.");
                }
            };

            this.Controls.Add(dtPicker);
            this.Controls.Add(gridCages);
            this.Controls.Add(gridMortalities);
            this.Controls.Add(numQuantity);
            this.Controls.Add(btnAdd);
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
            gridMortalities.DataSource = mortalities.Select(m => new
            {
                Cage = m.Cage?.Name ?? $"Cage {m.CageId}",
                m.MortalityDate,
                m.Quantity
            }).ToList();
        }
    }

}
