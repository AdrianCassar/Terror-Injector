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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Terror_Injector.Injector;
using System.Json;
using System.Net.Http;

namespace Terror_Injector
{
    /// <summary>
    ///
    /// </summary>
    public static class InjectorHelper
    {
        public static string TerrorDocuments { get; } = @$"{Environment.GetEnvironmentVariable("USERPROFILE")}\Documents\Terror";
        public static string TerrorAppdata { get; } = @$"{ Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Terror";

        public static string InjectionKey { get; } = @$"{InjectorHelper.TerrorAppdata}\Injection_Key.txt";
        public static string InstallerLogin { get; } = @$"{InjectorHelper.TerrorAppdata}\InstallerLogin.txt";
        public static string MenuName { get; } = @$"{InjectorHelper.TerrorAppdata}\MenuName.txt";

        static readonly HttpClient client = new HttpClient();

        #region WinAPI DllImports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        public static DllInjectionResult StartInjection(string Path)
        {
            return Injection.GetInstance.Inject("GTA5", Path);
        }

        /// <summary>
        /// Determines if Terror is installed.
        /// </summary>
        /// <returns>True if Terror is installed otherwise false.</returns>
        public static bool IsTerrorInstalled()
        {
            bool result;

            if (result = Directory.Exists(TerrorDocuments))
            {
                int count = Directory.GetDirectories(TerrorDocuments).Length + Directory.GetFiles(TerrorDocuments).Length;

                result = count >= 6 && File.Exists($"{TerrorDocuments}\\{GetTerrorMenuName()}");
            }

            return result;
        }

        /// <summary>
        /// Determine if GTA V is running.
        /// </summary>
        /// <returns>True if GTA V is running, otherwise false.</returns>
        public static bool IsGTA5Running()
        {
            return Process.GetProcessesByName("GTA5").FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero) != null;
        }

        /// <summary>
        /// Switch focus to GTA V.
        /// </summary>
        public static void SwitchToGTA5()
        {
            if (IsGTA5Running())
            {
                Process GTAV_Process = Process.GetProcessesByName("GTA5").FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero);

                if (GTAV_Process != null)
                {
                    ShowWindow(GTAV_Process.MainWindowHandle, 3);
                    SetForegroundWindow(GTAV_Process.MainWindowHandle);
                }
            }
        }

        /// <summary>
        /// Gets the name of the menu file.
        /// </summary>
        /// <returns>Returns the name of the menu file if it exists otherwise an empty string.</returns>
        public static string GetTerrorMenuName()
        {
            string FileName = string.Empty;

            if (File.Exists(MenuName))
            {
                try
                {
                    FileName = File.ReadAllText(MenuName).Trim();
                }
                catch (Exception)
                {

                }
            }

            return FileName;
        }

        /// <summary>
        /// Determine the detected status of Terror.
        /// </summary>
        /// <returns>An DetectedStatus value which reflects the detected status.</returns>
        public static async Task<DetectedStatus> IsDetected()
        {
            string detectedStatus = await GetDetectedStatusAsync();

            if (string.IsNullOrEmpty(detectedStatus))
                detectedStatus = DetectedStatus.Unknown.ToString();

            return Enum.Parse<DetectedStatus>(detectedStatus.ToLower(), true);
        }

        /// <summary>
        /// Request the detected status of Terror.
        /// </summary>
        /// <returns>A string which represents the detected status.</returns>
        public static async Task<string> GetDetectedStatusAsync()
        {
            return await GetWebRequest(new Uri("https://mistermodzzforum.space/authserver/FreeMenuDetected.php"));
        }

        /// <summary>
        /// Request the latest version of Terror.
        /// </summary>
        /// <returns>A string which represents the latest version of Terror.</returns>
        public static async Task<string> GetLatestVersionAsync()
        {
            return await GetWebRequest(new Uri("https://mistermodzzforum.space/authserver/Terrormenu.php"));
        }

        /// <summary>
        /// Perform a web request.
        /// </summary>
        /// <param name="url">The URL to perform a request on.</param>
        /// <returns>The string which was read from the URI.</returns>
        public static async Task<string> GetWebRequest(Uri uri)
        {
            string result;

            try
            {
                string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                result = (await client.GetStringAsync(uri)).Trim();
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Determine which version of Terror is installed.
        /// </summary>
        /// <returns>Returns a string which represents the installed version of Terror. If Terror is not installed an empty string is returned.</returns>
        public static string GetInstalledVersion()
        {
            string version = string.Empty;
            string versionFile = $"{TerrorAppdata}\\Version.txt";

            if (File.Exists(versionFile))
            {
                version = File.ReadAllText(versionFile).Trim();
            }
            else
            {
                if (Directory.Exists(TerrorDocuments))
                {
                    string versionFilev2 = Directory.GetFiles(TerrorDocuments).FirstOrDefault((file) => file.StartsWith("Version") && file.EndsWith(".txt"));

                    DirectoryInfo Dir = new(TerrorDocuments);
                    FileInfo File = Dir.GetFiles("*.txt").FirstOrDefault((file) => file.Name.StartsWith("Version"));

                    if (File != null)
                    {
                        version = Path.GetFileNameWithoutExtension(File.Name);
                    }
                }
            }

            return version.Replace("Version ", string.Empty);
        }

        /// <summary>
        /// Determine if a file is locked due to being in use.
        /// </summary>
        /// <param name="file">The file to be checked.</param>
        /// <returns>True if the file is locked, otherwise false.</returns>
        private static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using FileStream fileStream = file.Open(FileMode.Open, FileAccess.Write, FileShare.None);
            }
            catch (IOException)
            {
                Debug.WriteLine("\nFileLocked");

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determine if Terror is currently injected.
        /// </summary>
        /// <returns>True if injected, otherwise false.</returns>
        public static bool IsTerrorInjected()
        {
            return IsTerrorInstalled() && IsFileLocked(new FileInfo($"{TerrorDocuments}\\{GetTerrorMenuName()}")) && IsGTA5Running();
        }

        /// <summary>
        /// Determine if server is alive.
        /// </summary>
        /// <param name="url">The URL to ping.</param>
        /// <returns>True if server could be reached, otherwise false.</returns>
        private static async Task<bool> PingWeb(string url)
        {
            IPStatus result;

            try
            {
                result = (await new Ping().SendPingAsync(url, 2000)).Status;
            }
            catch (Exception)
            {
                result = IPStatus.TimedOut;
            }

            return result == IPStatus.Success;
        }

        /// <summary>
        /// Ping download server.
        /// </summary>
        /// <returns>True if server online, otherwise false.</returns>
        public static async Task<bool> CheckDownloadServer()
        {
            return await PingWeb("mistermodzzforum.space");
        }

        /// <summary>
        /// Ping Google.
        /// </summary>
        /// <returns>True if server online, otherwise false.</returns>
        public static async Task<bool> IsOnline()
        {
            return await PingWeb("google.com");
        }

        /// <summary>
        /// Gets the latest version.
        /// - If connection fails default version is returned.
        /// </summary>
        /// <returns>Returns the latest version, </returns>
        public static async Task<Version> GetInjectorLatestVersionAsync()
        {
            JsonValue value;
            Version LatestVer = new();

            try
            {
                string responseBody = await GetWebRequest(new Uri("https://api.github.com/repos/MoistyMarley/Terror-Injector/releases/latest"));

                if (!string.IsNullOrEmpty(responseBody)) {
                    value = JsonValue.Parse(responseBody);
                    LatestVer = new(value["tag_name"]);
                }
            }
            catch (Exception)
            {

            }

            return LatestVer;
        }

        /// <summary>
        /// Gets the version of this assembly.
        /// - If file is not found default version is returned.
        /// </summary>
        /// <returns>Returns the version of this assembly.</returns>
        public static Version GetAssemblyVersion()
        {
            Version AssemblyVer = new();

            try
            {
                //Assembly assembly = Assembly.GetExecutingAssembly();
                //FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName);
                AssemblyVer = new(VersionInfo.FileVersion);
            }
            catch (Exception)
            {

            }

            return AssemblyVer;
        }

        /// <summary>
        /// Check if an update is available.
        /// </summary>
        /// <returns>Returns true if an update is available otherwise false.</returns>
        public static async Task<bool> UpdateAvailableAsync()
        {
            Version AssemblyVer = GetAssemblyVersion();
            Version LatestVer = await GetInjectorLatestVersionAsync();

            return AssemblyVer.CompareTo(LatestVer) < 0;
        }
    }
}