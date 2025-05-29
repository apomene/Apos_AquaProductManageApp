using static Apos_AquaProductManageApp.Presenters.Presenter;

namespace Apos_AquaProductManageApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var cageForm = new CageForm();
            var presenter = new CagePresenter(cageForm);
            Application.Run(cageForm);
        }
    }
}