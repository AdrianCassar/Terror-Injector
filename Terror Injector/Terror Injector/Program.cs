using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Terror_Injector
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (IsElevated())
            {
                Application.Run(new Terror_Injector());
            }
            else
            {
                MessageBox.Show("Please Run As Administrator.", "Run As Administrator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static bool IsElevated()
        {
            bool IsAdmin;

            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new(identity);

                IsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return IsAdmin;
        }
    }
}
