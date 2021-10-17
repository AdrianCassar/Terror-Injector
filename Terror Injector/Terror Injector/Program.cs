/*
    Terror Injector is an easy-to-use tool for seamlessly injecting the free GTA V Terror mod menu.
    Copyright (C) 2021 MoistyMarley <https://github.com/MoistyMarley/Terror-Injector/>.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            SaveLicense();

            if (IsElevated())
                Application.Run(new frmTerrorInjector());
            else
                MessageBox.Show("Please Run as Administrator.", "Run As Administrator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Determine if the application is running with evaluated privileges.
        /// </summary>
        /// <returns>True if elevated, otherwise false.</returns>
        private static bool IsElevated()
        {
            bool IsAdmin;

            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new(identity);

                IsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return IsAdmin;
        }

        /// <summary>
        /// Save License if it cannot be found.
        /// </summary>
        /// <returns>True if license exists in the same directory as the binary, otherwise false.</returns>
        private static bool SaveLicense() {
            string LicensePath = $"{Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}\\LICENSE.txt";

            if (!File.Exists(LicensePath)) {
                try
                {
                    File.WriteAllText(LicensePath, Properties.Resources.LICENSE);
                }
                catch (Exception)
                {
                }
            }

            return File.Exists(LicensePath);
        }
    }
}
