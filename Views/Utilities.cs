
using System.Configuration;

namespace Apos_AquaProductManageApp.Views
{
    /// <summary>
    /// Utility class for common functions used across the application.
    /// </summary>
    public static class Utilities
    {
        public static void BindDataSource<T>(DataGridView grid, List<T> data, params string[] columnsToHide)
        {
            void ApplyBinding()
            {
                grid.DataSource = null;
                grid.DataSource = data;

                if (grid.Columns != null && columnsToHide != null)
                {
                    foreach (var columnName in columnsToHide)
                    {
                        if (!string.IsNullOrEmpty(columnName) && grid.Columns.Contains(columnName))
                        {
                            var col = grid.Columns[columnName];
                            if (col != null)
                            {
                                col.Visible = false;
                            }
                        }
                    }
                }
            }

            // Delay if the grid is currently in edit mode to avoid re-entrant call
            if (grid.IsCurrentCellInEditMode)
            {
                // Wait until the control is ready
                grid.BeginInvoke((MethodInvoker)(() => ApplyBinding()));
            }
            else
            {
                ApplyBinding();
            }
        }

        public static void InitializeFormSizeFromConfig(Form form, string configPrefix)
        {
            if (form == null || string.IsNullOrWhiteSpace(configPrefix))
                return;

            if (int.TryParse(ConfigurationManager.AppSettings[$"{configPrefix}.Width"], out int width))
                form.Width = width;

            if (int.TryParse(ConfigurationManager.AppSettings[$"{configPrefix}.Height"], out int height))
                form.Height = height;
        }
    }
}

