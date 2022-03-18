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
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Terror_Injector
{
    /// <summary>
    ///
    /// </summary>
    public static class InjectorHelper
    {

        private readonly static string Documents = Environment.GetEnvironmentVariable("USERPROFILE");
        private readonly static string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string TerrorDocuments { get; } = @$"{Documents}\Documents\Terror";
        public static string TerrorAppdata { get; } = @$"{Appdata}\Terror";
        public static string TerrorMenuName { get; } = @$"{TerrorAppdata}\MenuName.txt";

        public static string MisterModzZDocuments { get; } = @$"{Documents}\Documents\MisterModzZ";
        public static string MisterModzZAppdata { get; } = @$"{Appdata}\MisterModzZ";
        public static string MisterModzZMenuName { get; } = @$"{MisterModzZAppdata}\MenuName.txt";

        public static string InjectionKey { get; } = @$"{TerrorAppdata}\Injection_Key.txt";
        public static string InstallerLogin { get; } = @$"{TerrorAppdata}\InstallerLogin.txt";

        public static string Username { get; } = "EJSsRSSeCSQ4YatS";
        public static string Password { get; } = "aTtsnUAxaKvJT7YZ";
        public static string IP { get; } = "230.191.165.66";
        public static string Question_Ans { get; } = "9eT68JfWWJte99M2";

        //username = "EJSsRSSeCSQ4YatS"
        //password = "aTtsnUAxaKvJT7YZ"
        //IP_Address = "230.191.165.66"
        //VERIFICATION_QUESTION = "m2cMzr95BXvTBSfc"
        //VERIFICATION_QUESTION_ANSWER = "9eT68JfWWJte99M2"

        private static readonly HttpClient WebClient = new();

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
        /// Determines if a menu is installed.
        /// </summary>
        /// <returns>True if a menu is installed otherwise false.</returns>
        private static bool IsInstalled(string documents, string menuFile)
        {
            bool result;

            if (result = Directory.Exists(documents))
            {
                var dlls = new DirectoryInfo(documents).GetFiles().Where(f => IsDll(f));

                result = dlls.Count() >= 1 && File.Exists(menuFile);
            }

            return result;
        }


        //https://stackoverflow.com/a/6309893
        /// <summary>
        /// Determines if a file is a DLL.
        /// </summary>
        /// <param name="file">The file to be checked.</param>
        /// <returns>True if file is a DLL otherwise false.</returns>
        public static bool IsDll(FileInfo file)
        {
            FileStream stream = file.OpenRead();

            using (BinaryReader reader = new BinaryReader(stream))
            {
                byte[] header = reader.ReadBytes(2); //Read MZ
                if (header[0] != (byte)'M' && header[1] != (byte)'Z')
                    return false;

                stream.Seek(64 - 4, SeekOrigin.Begin);//read elf_new this is the offset where the IMAGE_NT_HEADER begins
                int offset = reader.ReadInt32();

                stream.Seek(offset, SeekOrigin.Begin);
                header = reader.ReadBytes(2);

                if (header[0] != (byte)'P' && header[1] != (byte)'E')
                    return false;

                stream.Seek(20, SeekOrigin.Current); //point to last word of IMAGE_FILE_HEADER
                short readInt16 = reader.ReadInt16();

                return (readInt16 & 0x2000) == 0x2000;
            }
        }

        /// <summary>
        /// Determines if Terror is installed.
        /// </summary>
        /// <returns>True if Terror is installed otherwise false.</returns>
        public static bool IsTerrorInstalled()
        {
            return IsInstalled(TerrorDocuments, TerrorMenuName);
        }

        /// <summary>
        /// Determines if MisterModzZ is installed.
        /// </summary>
        /// <returns>True if MisterModzZ is installed otherwise false.</returns>
        public static bool IsMisterModzZInstalled()
        {
            return IsInstalled(MisterModzZDocuments, MisterModzZMenuName);
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
                    //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
                    ShowWindow(GTAV_Process.MainWindowHandle, 5);
                    SetForegroundWindow(GTAV_Process.MainWindowHandle);
                }
            }
        }

        /// <summary>
        /// Gets the name of the menu file.
        /// </summary>
        /// <returns>Returns the name of the menu file if it exists otherwise an empty string.</returns>
        private static string GetMenuName(String menuName)
        {
            string FileName = string.Empty;

            if (File.Exists(menuName))
            {
                try
                {
                    FileName = File.ReadAllText(menuName).Trim();
                }
                catch (Exception)
                {

                }
            }

            return FileName;
        }

        /// <summary>
        /// Gets the name of the menu file.
        /// </summary>
        /// <returns>Returns the name of the menu file if it exists otherwise an empty string.</returns>
        public static string GetTerrorMenuName()
        {
            return GetMenuName(TerrorMenuName);
        }

        /// <summary>
        /// Gets the name of the menu file.
        /// </summary>
        /// <returns>Returns the name of the menu file if it exists otherwise an empty string.</returns>
        public static string GetMisterModzZMenuName()
        {
            return GetMenuName(MisterModzZMenuName);
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
                if (WebClient.DefaultRequestHeaders.UserAgent.Count == 0) {
                    string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
                    WebClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                }

                result = (await WebClient.GetStringAsync(uri)).Trim();
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
                version = File.ReadAllText(versionFile);
            }
            else
            {
                if (Directory.Exists(TerrorDocuments))
                {
                    DirectoryInfo Dir = new(TerrorDocuments);
                    FileInfo File = Dir.GetFiles("Version ?.?.txt").FirstOrDefault();

                    if (File != null)
                    {
                        version = Path.GetFileNameWithoutExtension(File.Name);
                    }
                }
            }

            return version.Replace("Version ", string.Empty).Trim();
        }

        /// <summary>
        /// Determine if a file is locked due to being in use.
        /// </summary>
        /// <param name="file">The file to be checked.</param>
        /// <returns>True if the file is locked, otherwise false.</returns>
        private static bool IsFileLocked(FileInfo file)
        {
            if (file.Exists)
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
        /// Determine if MisterModzZ is currently injected.
        /// </summary>
        /// <returns>True if injected, otherwise false.</returns>
        public static bool IsMisterModzZInjected()
        {
            return IsMisterModzZInstalled() && IsFileLocked(new FileInfo($"{MisterModzZDocuments}\\{GetMisterModzZMenuName()}")) && IsGTA5Running();
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

                if (!string.IsNullOrEmpty(responseBody))
                {
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

        public static bool SetOpenKey(int keyCode)
        {
            DirectoryInfo ConfigDir = new(@$"{TerrorAppdata}\Configs");
            FileInfo Key = new(@$"{ConfigDir.FullName}\Keys.ini");

            try
            {
                if (!ConfigDir.Exists)
                    ConfigDir.Create();

                using (StreamWriter streamWriter = new(Key.Create()))
                {
                    streamWriter.Write("[Hotkeys]\r\n");
                    streamWriter.Write($"OpenKey={keyCode}");
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //public static void GetModuleVersionId()
        //{
        //    Assembly assembly = Assembly.GetEntryAssembly();
        //    Guid hashId = assembly.ManifestModule.ModuleVersionId;

        //    Debug.WriteLine(hashId);
        //}

        //public static string BinaryHash()
        //{
        //    using (var SHA = SHA256.Create())
        //    {
        //        string filename = Process.GetCurrentProcess().MainModule.FileName;

        //        using (var stream = File.OpenRead(filename))
        //        {
        //            var hash = SHA.ComputeHash(stream);
        //            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        //        }
        //    }
        //}
    }
}