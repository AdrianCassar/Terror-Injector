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
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Terror_Injector.Injector
{
    /// <summary>
    /// Event data
    /// </summary>
    public class TaskEventArgs : EventArgs
    {
        public bool Result { get; set; }
    }

    /// <summary>
    /// - Downloads Menu
    /// - Updates Menu
    /// - Installs Menu
    /// - Uninstalls Menu
    /// - Injector Updates
    /// </summary>
    static class InjectorTasks
    {
        public static event EventHandler<TaskEventArgs> DownloadCompleted = delegate { };
        public static event EventHandler<TaskEventArgs> InstallationCompleted = delegate { };
        public static event EventHandler<TaskEventArgs> UninstallCompleted = delegate { };
        public static event EventHandler<TaskEventArgs> UpdateCompleted = delegate { };

        public static event EventHandler BeginDownloading = delegate { };
        public static event EventHandler BeginInstalling = delegate { };
        public static event EventHandler BeginUninstalling = delegate { };
        public static event EventHandler BeginUpdating = delegate { };

        //private static Timer downloadTimer;
        //private static ElapsedEventHandler TimerTick;

        //Download Locations
        //https://mistermodzzforum.space/Downloads/KPmcvgQdA34aJ8BX.zip
        //https://mistermodzzforum.space/Downloads/Terror.zip

        //MisterModzZV2
        //https://mistermodzzforum.space/TerrorInstaller/MisterModzZV2/MenuFiles.zip

        private static readonly HttpClient WebClient = new();

        /// <summary>
        /// - Uninstalls Terror
        /// - Downloads Terror
        /// - Download cancelled after 1m 40s Source: http://slowsite.local/
        /// </summary>
        /// <returns>True if the download was successfully, otherwise false.</returns>
        public static async Task<bool> DownloadTerrorAsync()
        {
            BeginDownloading(null, EventArgs.Empty);

            if (await InjectorHelper.CheckDownloadServer())
                if (InjectorHelper.IsTerrorInstalled())
                    SilentUninstaller(InjectorHelper.TerrorDocuments, InjectorHelper.TerrorAppdata);

            bool taskCancelled = false;

            //Timeout = 100s/1m 40s
            //WebClient webClient = new();

            if (!InjectorHelper.IsTerrorInstalled())
            {
                Uri downloadAddress = new("https://mistermodzzforum.space/Downloads/KPmcvgQdA34aJ8BX.zip");

                try
                {
                    //webclient.Timeout = TimeSpan.FromSeconds(5);
                    Directory.CreateDirectory(InjectorHelper.TerrorDocuments);

                    //webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                    //await webClient.DownloadFileTaskAsync(downloadAddress, $"{InjectorHelper.TerrorDocuments}\\Terror.zip");

                    byte[] fileBytes = await WebClient.GetByteArrayAsync(downloadAddress);
                    File.WriteAllBytes(@$"{InjectorHelper.TerrorDocuments}\Terror.zip", fileBytes);
                }
                catch (Exception)
                {
                    taskCancelled = true;
                    //Debug.WriteLine(ex.ToString());

                    //Remove leftovers
                    if(!InjectorHelper.IsTerrorInstalled())
                        SilentUninstaller(InjectorHelper.TerrorDocuments, InjectorHelper.TerrorAppdata);
                }
            }

            DownloadCompleted(null, new TaskEventArgs() { Result = !taskCancelled });

            return File.Exists(@$"{InjectorHelper.TerrorDocuments}\Terror.zip") & !taskCancelled;
        }

        /// <summary>
        /// - Uninstalls Terror
        /// - Downloads Terror
        /// - Download cancelled after 1m 40s Source: http://slowsite.local/
        /// </summary>
        /// <returns>True if the download was successfully, otherwise false.</returns>
        public static async Task<bool> DownloadMisterModzZAsync()
        {
            BeginDownloading(null, EventArgs.Empty);

            //if (await InjectorHelper.CheckDownloadServer())
            //    if (InjectorHelper.IsMisterModzZInstalled())
            //        SilentUninstaller(InjectorHelper.MisterModzZDocuments, InjectorHelper.MisterModzZAppdata);

            bool taskCancelled = false;

            //Timeout = 100s/1m 40s
            //WebClient webClient = new();

            if (!InjectorHelper.IsMisterModzZInstalled())
            {
                Uri downloadAddress = new("https://mistermodzzforum.space/TerrorInstaller/MisterModzZV2/MenuFiles.zip");

                try
                {
                    //webclient.Timeout = TimeSpan.FromSeconds(5);
                    Directory.CreateDirectory(InjectorHelper.MisterModzZDocuments);

                    //webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                    //await webClient.DownloadFileTaskAsync(downloadAddress, $"{InjectorHelper.TerrorDocuments}\\Terror.zip");

                    byte[] fileBytes = await WebClient.GetByteArrayAsync(downloadAddress);
                    File.WriteAllBytes(@$"{InjectorHelper.MisterModzZDocuments}\MisterModzZ.zip", fileBytes);
                }
                catch (Exception)
                {
                    taskCancelled = true;
                    //Debug.WriteLine(ex.ToString());

                    //Remove leftovers
                    if (!InjectorHelper.IsMisterModzZInstalled())
                    {
                        //SilentUninstallTerror();
                    }
                }
            }

            DownloadCompleted(null, new TaskEventArgs() { Result = !taskCancelled });

            return File.Exists(@$"{InjectorHelper.MisterModzZDocuments}\MisterModzZ.zip") & !taskCancelled;
        }

        ///// <summary>
        ///// Occurs when an asynchronous file download operation completes.
        ///// </summary>
        ///// <param name="sender">Reference to the control/object that raised the event.</param>
        ///// <param name="e">Event Data.</param>
        //private static void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        //{
        //    bool Downloaded = !(e.Cancelled || e.Error != null);

        //    //If download failed remove leftovers
        //    if (!Downloaded)
        //    {
        //        SilentUninstallTerror();
        //    }

        //    DownloadCompleted(null, new TaskEventArgs() { Result = Downloaded });
        //}

        /// <summary>
        /// Install Terror
        /// </summary>
        /// <returns>True if installation was successful, otherwise false.</returns>
        public static async Task<bool> InstallTerrorAsync()
        {
            BeginInstalling(null, EventArgs.Empty);

            bool result = false;

            string TerrorZip = @$"{InjectorHelper.TerrorDocuments}\Terror.zip";

            if (File.Exists(TerrorZip))
            {
                try
                {
                    if (!InjectorHelper.IsTerrorInstalled())
                        Directory.CreateDirectory(InjectorHelper.TerrorAppdata);

                    ZipFile.ExtractToDirectory(TerrorZip, InjectorHelper.TerrorDocuments, true);
                    File.Delete(TerrorZip);

                    CreateMenuFiles();

                    result = true;

                    //result = await CreateMenuFilesAsync(InjectorHelper.TerrorDocuments) && LockFiles(true);
                }
                catch (Exception)
                {

                }
            }

            InstallationCompleted(null, new TaskEventArgs() { Result = result });

            return result;
        }

        public static async Task<bool> InstallMisterModzZAsync()
        {
            BeginInstalling(null, EventArgs.Empty);

            bool result = false;

            string TerrorZip = @$"{InjectorHelper.MisterModzZDocuments}\MisterModzZ.zip";

            if (File.Exists(TerrorZip))
            {
                try
                {
                    if (!InjectorHelper.IsMisterModzZInstalled())
                        Directory.CreateDirectory(InjectorHelper.MisterModzZAppdata);

                    ZipFile.ExtractToDirectory(TerrorZip, InjectorHelper.MisterModzZDocuments, true);
                    File.Delete(TerrorZip);

                    CreateMenuFiles();

                    result = true;

                    //result = await CreateMenuFilesAsync(InjectorHelper.MisterModzZMenuName) && LockFiles(true);
                }
                catch (Exception)
                {

                }
            }

            InstallationCompleted(null, new TaskEventArgs() { Result = result });

            return result;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <returns></returns>
        //public static async Task<bool> CreateMenuFilesAsync()
        //{
        //    bool result = true;

        //    try
        //    {
        //        if (!Directory.Exists(InjectorHelper.TerrorAppdata))
        //        {
        //            Directory.CreateDirectory(InjectorHelper.TerrorAppdata);
        //        }

        //        if (!File.Exists(InjectorHelper.TerrorMenuName))
        //        {
        //            File.WriteAllText(InjectorHelper.TerrorMenuName, "UpP1YrVpA74DW11Y.menu\r\n");
        //        }

        //        //string key = await InjectorHelper.GetWebRequest(new Uri("https://raw.githubusercontent.com/MoistyMarley/Terror-Injector/main/Terror%20Injector/InjectionKey"));
        //        //await Insert_Injection_KeyAsync(key);

        //        string key = await Get_Injection_KeyAsync();

        //        if (!key.Equals(string.Empty))
        //        {
        //            if (!File.Exists(InjectorHelper.InjectionKey))
        //            {
        //                File.WriteAllText(InjectorHelper.InjectionKey, $"{key}\r\n");
        //            }
        //            else
        //            {
        //                string savedKey = File.ReadAllText(InjectorHelper.InjectionKey);

        //                if (!savedKey.Equals($"{key}\r\n"))
        //                {
        //                    FileAccessControl Key = new(new FileInfo(InjectorHelper.InjectionKey));

        //                    bool IsLocked = Key.IsLocked();

        //                    if (IsLocked)
        //                        Key.ToggleLock();

        //                    File.WriteAllText(InjectorHelper.InjectionKey, $"{key}\r\n");

        //                    if (IsLocked)
        //                        Key.ToggleLock();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            result = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result = false;
        //    }

        //    return result;
        //}

        public static bool CreateMenuFiles() {
            try
            {
                if (!Directory.Exists(InjectorHelper.TerrorAppdata))
                    Directory.CreateDirectory(InjectorHelper.TerrorAppdata);

                if (!File.Exists(InjectorHelper.TerrorMenuName))
                    File.WriteAllText(InjectorHelper.TerrorMenuName, "UpP1YrVpA74DW11Y.menu\r\n");

                if (!Directory.Exists(InjectorHelper.MisterModzZAppdata))
                    Directory.CreateDirectory(InjectorHelper.MisterModzZAppdata);

                if (!File.Exists(InjectorHelper.MisterModzZMenuName))
                    File.WriteAllText(InjectorHelper.MisterModzZMenuName, "c3NSpqDvkTxYaysr.g6hmMn\r\n");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> CreateMenuFilesAsync()
        {
            bool result = true;

            try
            {
                CreateMenuFiles();

                //Key cannot be empty
                string key = "Xr0WKvmKH3D";

                bool updatedKey = await Insert_Injection_KeyAsync(key);
                string serverKey = await Get_Injection_KeyAsync();

                if (serverKey.Equals(key) && !key.Equals(string.Empty))
                {
                    if (!File.Exists(InjectorHelper.InjectionKey))
                    {
                        File.WriteAllText(InjectorHelper.InjectionKey, $"{key}\r\n");
                    }
                    else
                    {
                        string savedKey = File.ReadAllText(InjectorHelper.InjectionKey);

                        if (!savedKey.Equals($"{key}\r\n"))
                        {
                            FileAccessControl Key = new(new FileInfo(InjectorHelper.InjectionKey));

                            bool IsLocked = Key.IsLocked();

                            if (IsLocked)
                                Key.ToggleLock();

                            File.WriteAllText(InjectorHelper.InjectionKey, $"{key}\r\n");

                            if (IsLocked)
                                Key.ToggleLock();
                        }
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool CreateLogin()
        {
            try
            {
                if (!File.Exists(InjectorHelper.InstallerLogin))
                {
                    File.WriteAllText(InjectorHelper.InstallerLogin, $"Username: {InjectorHelper.Username}\r\nPassword: {InjectorHelper.Password}\r\n");
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool DeleteLogin()
        {
            try
            {
                if (File.Exists(InjectorHelper.InstallerLogin))
                {
                    FileAccessControl Login = new(new FileInfo(InjectorHelper.InstallerLogin));

                    if (Login.IsLocked())
                        Login.ToggleLock();

                    File.Delete(InjectorHelper.InstallerLogin);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// If the latest version of Terror is not installed then update/install Terror.
        /// </summary>
        /// <returns>True if update was successful, otherwise false.</returns>
        public static async Task<bool> UpdateTerror()
        {
            BeginUpdating(null, EventArgs.Empty);

            bool result;

            if (result = await InjectorHelper.CheckDownloadServer())
            {
                string latestVersion = await InjectorHelper.GetLatestVersionAsync();

                if (latestVersion == string.Empty)
                {
                    latestVersion = $"Version {InjectorHelper.GetInstalledVersion()}";
                }

                string versionFile = @$"{InjectorHelper.TerrorDocuments}\{latestVersion}.txt";

                if (!(result = File.Exists(versionFile)))
                {
                    if (await DownloadTerrorAsync())
                        result = await InstallTerrorAsync();
                }
            }

            //Latest version installed
            //If installed check temp files.
            //if (result)
            //{
            //    if (await CreateMenuFilesAsync(InjectorHelper.TerrorMenuName))
            //        LockFiles(true);
            //}

            UpdateCompleted(null, new TaskEventArgs() { Result = result });

            return result;
        }

        /// <summary>
        /// Uninstall Terror
        /// </summary>
        /// <returns>True if Terror was uninstalled otherwise false.</returns>
        public static bool UninstallTerror()
        {
            BeginUninstalling(null, EventArgs.Empty);

            bool result = false;

            //Prevent uninstalling while it's injected.
            if (!InjectorHelper.IsTerrorInjected())
            {
                result = SilentUninstaller(InjectorHelper.TerrorDocuments, InjectorHelper.TerrorAppdata);
            }

            UninstallCompleted(null, new TaskEventArgs() { Result = result });

            return result;
        }

        /// <summary>
        /// Uninstall MisterModzZ
        /// </summary>
        /// <returns>True if MisterModzZ was uninstalled otherwise false.</returns>
        public static bool UninstallMisterModzZ()
        {
            BeginUninstalling(null, EventArgs.Empty);

            bool result = false;

            //Prevent uninstalling while it's injected.
            if (!InjectorHelper.IsMisterModzZInjected())
            {
                result = SilentUninstaller(InjectorHelper.MisterModzZDocuments, InjectorHelper.MisterModzZAppdata);
            }

            UninstallCompleted(null, new TaskEventArgs() { Result = result });

            return result;
        }

        /// <summary>
        /// Silently uninstall without GUI changes.
        /// </summary>
        /// <returns>True if successfully uninstalled, otherwise false.</returns>
        private static bool SilentUninstaller(string documents, string appdata)
        {
            bool IsLocked = true;
            bool IsInjected;

            if (documents == InjectorHelper.TerrorDocuments)
                IsInjected = InjectorHelper.IsTerrorInjected();
            else
                IsInjected = InjectorHelper.IsMisterModzZInjected();

            //Prevent uninstalling while it's injected.
            if (!IsInjected)
            {
                if (IsLocked = LockFiles(false))
                {
                    try
                    {
                        if (Directory.Exists(documents))
                            Directory.Delete(documents, true);

                        if (Directory.Exists(appdata))
                            Directory.Delete(appdata, true);
                    }
                    catch (Exception) {}
                }
            }

            return IsLocked && !Directory.Exists(documents) && !Directory.Exists(appdata);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Lock"></param>
        /// <returns></returns>
        private static bool LockFiles(bool Lock)
        {
            int count = 0;

            List<FileAccessControl> LockedFiles = new();

            FileAccessControl Key = new(new FileInfo(InjectorHelper.InjectionKey));
            LockedFiles.Add(Key);

            foreach (FileAccessControl file in LockedFiles)
            {
                if (file.IsLocked() != Lock)
                {
                    if (file.ToggleLock() == Lock) //if(file.IsLocked() == Lock)
                        count++;
                }
                else
                {
                    count++;
                }
            }

            return count == LockedFiles.Count;
        }

        /// <summary>
        /// Deletes an account.
        /// </summary>
        /// <returns>True if the account was deleted, otherwise false.</returns>
        public static async Task<bool> DeleteAccountAsync()
        {
            return await DeleteAccountAsync(InjectorHelper.IP);
        }

        /// <summary>
        /// Deletes an account.
        /// </summary>
        /// <param name="IP">The IP address of the account to delete.</param>
        /// <returns>True if the account was deleted, otherwise false.</returns>
        private static async Task<bool> DeleteAccountAsync(string IP)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://mistermodzzforum.space/authserver/AccountDelete.php"),
                Method = HttpMethod.Get
            };

            string resultContent = string.Empty;

            using (request)
            {
                HttpResponseMessage response;

                try
                {
                    request.Headers.Add("Referer", "https://linkvertise.com/");
                    request.Headers.Add("Client-IP", IP);

                    using (response = await WebClient.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resultContent = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                catch (Exception) { }
            }

            return resultContent.Equals("[\"Account has Sucsessfully been deleted\"]");
        }

        /// <summary>
        /// Creates an account.
        /// </summary>
        /// <returns>True if the account was created, otherwise false.</returns>
        public static async Task<bool> CreateAccount()
        {
            return await CreateAccount(InjectorHelper.Username, InjectorHelper.Password, InjectorHelper.IP, InjectorHelper.Question_Ans);
        }

        /// <summary>
        /// Creates an account.
        /// </summary>
        /// <param name="username">Account Username</param>
        /// <param name="password">Account Username</param>
        /// <param name="IP">Account IP</param>
        /// <param name="ans">Security Questions Answer</param>
        /// <returns>True if the account was created, otherwise false.</returns>
        private static async Task<bool> CreateAccount(string username, string password, string IP, string ans)
        {
            string url = "https://mistermodzzforum.space/TerrorInstaller/Installer_Register.php";

            var PostData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("IP_Address", IP),
                new KeyValuePair<string, string>("VERIFICATION_QUESTION", " "),
                new KeyValuePair<string, string>("VERIFICATION_QUESTION_ANSWER", ans)
            });

            string resultContent = await PostAPI(url, PostData);

            return resultContent.Equals("New record created successfully");

            //if (resultContent == "Username Already Exits") {
            //    return false;
            //} else { 
            //    return false;
            //}
        }

        public static async Task<bool> Menu_Login()
        {
            return await Menu_Login(InjectorHelper.Username, InjectorHelper.Password);
        }

        private static async Task<bool> Menu_Login(string username, string password)
        {
            string url = "https://mistermodzzforum.space/TerrorInstaller/Menu_Login.php";

            FormUrlEncodedContent PostData = new(new[] {
                new KeyValuePair<string, string>("username", InjectorHelper.Username),
                new KeyValuePair<string, string>("password", InjectorHelper.Password)
            });

            string resultContent = await PostAPI(url, PostData);

            return resultContent.Equals("Access granted!");
        }

        /// <summary>
        /// Gets the injection key, used for injection validation.
        /// </summary>
        /// <returns>The injection key, otherwise an empty string.</returns>
        public static async Task<string> Get_Injection_KeyAsync()
        {
            string url = "https://mistermodzzforum.space/TerrorInstaller/Menu_Check_Injection_Key.php";

            FormUrlEncodedContent PostData = new(new[] {
                new KeyValuePair<string, string>("username", InjectorHelper.Username),
                new KeyValuePair<string, string>("password", InjectorHelper.Password),
            });

            string key = await PostAPI(url, PostData);

            Debug.WriteLine("Injection Key: " + key);

            return key.Trim();
        }

        /// <summary>
        /// Update the Injection Key, used for injection validation.
        /// </summary>
        /// <param name="key">Injection Key</param>
        /// <returns></returns>
        public static async Task<bool> Insert_Injection_KeyAsync(string key)
        {
            string url = "https://mistermodzzforum.space/TerrorInstaller/Installer_Insert_Injection_Key.php";

            FormUrlEncodedContent PostData = new(new[] {
                new KeyValuePair<string, string>("username", InjectorHelper.Username),
                new KeyValuePair<string, string>("password", InjectorHelper.Password),
                new KeyValuePair<string, string>("Injection_Key", $"{key}")
            });

            string resultContent = await PostAPI(url, PostData);

            return resultContent.Equals("Injection_Key Updated sucsessfully");
        }

        public static async Task<string> PostAPI(string url, FormUrlEncodedContent PostData)
        {
            string resultContent = string.Empty;

            try
            {
                HttpResponseMessage result;

                using (result = await WebClient.PostAsync(url, PostData))
                {
                    resultContent = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception) { }

            return resultContent;
        }

        /// <summary>
        /// Force focus of GTAV for 20 seconds.
        /// </summary>
        public static async Task ForceGTAVFocus() {
            //this.Hide();

            for (int i = 0; i < 201; i++)
            {
                await Task.Delay(100);
                InjectorHelper.SwitchToGTA5();
            }
        }
    }
}

///// <summary>
///// - Uninstalls Terror
///// - Downloads Terror
///// </summary>
///// <returns>True if the download was successfully, otherwise false.</returns>
//public static async Task<bool> DownloadTerrorAsync()
//{
//    BeginDownloading(null, EventArgs.Empty);

//    if (await InjectorHelper.CheckDownloadServer())
//        if (InjectorHelper.IsTerrorInstalled())
//            SilentUninstallTerror();

//    bool taskCancelled = false;

//    WebClient webClient = new();

//    if (!InjectorHelper.IsTerrorInstalled())
//    {
//        Uri downloadAddress = new("https://mistermodzzforum.space/Downloads/Terror.zip");

//        try
//        {
//            Directory.CreateDirectory(InjectorHelper.TerrorDocuments);

//            using (WebClient webClient = new())
//            {
//                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

//                using (CancellationTokenSource tokenSource = new())
//                {
//                    CancellationToken ct = tokenSource.Token;

//                    DownloadTimer(tokenSource);

//                    Task downloadFile = DownloadFileTaskAsync(webClient, downloadAddress, $"{InjectorHelper.TerrorDocuments}\\Terror.zip", ct);

//                    await downloadFile;
//                }
//            }
//        }
//        catch (WebException ex) //when (ex.Status == WebExceptionStatus.RequestCanceled)
//        {
//            taskCancelled = true;
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.ToString());
//        }
//    }

//    return File.Exists(@$"{InjectorHelper.TerrorDocuments}\Terror.zip") & !taskCancelled;
//}

//public static async Task DownloadFileTaskAsync(this WebClient webClient, Uri url, string fileName, CancellationToken cancellationToken)
//{
//    using (cancellationToken.Register(webClient.CancelAsync))
//    {
//        Task downloadFile = webClient.DownloadFileTaskAsync(url, fileName);

//        try
//        {
//            await downloadFile;
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine("");
//        }
//    }
//}

///// <summary>
///// Start a timer to auto-cancel the download if it takes longer than 2 minutes.
///// <br>Prevents deadlock.</br>
///// </summary>
//private static void DownloadTimer(CancellationTokenSource tokenSource)
//{
//    downloadTimer = new();

//    TimerTick = delegate (object sender, ElapsedEventArgs e)
//    {
//        downloadTimer.Elapsed -= TimerTick;
//        tokenSource.Cancel();
//    };

//    downloadTimer.Elapsed += TimerTick;
//    downloadTimer.Interval = 1; //120000 2 Minutes https://www.download-time.com/
//    downloadTimer.Enabled = true;
//    downloadTimer.AutoReset = false;
//}