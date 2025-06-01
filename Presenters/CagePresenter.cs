using Apos_AquaProductManageApp.Services;
using Microsoft.EntityFrameworkCore;
using static Apos_AquaProductManageApp.Interfaces.ViewInterfaces;

namespace Apos_AquaProductManageApp.Presenters
{
    public class CagePresenter
    {
        private readonly ICageView _view;
        private readonly CageService _service;

        public CagePresenter(ICageView view, CageService service)
        {
            _view = view;
            _service = service;
            view.SetPresenter(this);
            LoadCages();
        }

        public void LoadCages()
        {
            var cages = _service.GetAllCages();
            _view.DisplayCages(cages);
        }

        public void AddCage(string name, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Cage name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                _service.AddCage(name, isActive);
            }
            catch (DbUpdateException ex)
            {
                var sqlEx = ex.InnerException as Microsoft.Data.SqlClient.SqlException;
                if (sqlEx != null)
                {
                    // SQL Server error codes
                    if (sqlEx.Number == 2627) // Unique constraint violation (e.g., PK conflict)
                        MessageBox.Show("A cage with the same primary key already exists.");
                    else if (sqlEx.Number == 547) // Foreign key constraint violation
                        MessageBox.Show("This operation violates a foreign key constraint.");
                    else
                        MessageBox.Show($"Database error: {sqlEx.Message}");
                }
                else
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}");
                }
            }
            finally
            {
                LoadCages();
            }
        }

        public void DeleteCage(int id)
        {
            try
            {
                _service.DeleteCage(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred, on delete: {ex.Message}");
            }
            finally
            {
                LoadCages();
            }

        }

        public void UpdateCage(int id, string name, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Cage name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                _service.UpdateCage(id, name, isActive);
            }
            catch (DbUpdateException ex)
            {
                var sqlEx = ex.InnerException as Microsoft.Data.SqlClient.SqlException;
                if (sqlEx != null)
                {
                    if (sqlEx.Number == 2627)
                        MessageBox.Show("Update failed: Duplicate key.");
                    else if (sqlEx.Number == 547)
                        MessageBox.Show("Update failed: Foreign key constraint violation.");
                    else
                        MessageBox.Show($"Database error: {sqlEx.Message}");
                }
                else
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}");
                }
            }
            finally
            {
                LoadCages();
            }
        }
    }
}
