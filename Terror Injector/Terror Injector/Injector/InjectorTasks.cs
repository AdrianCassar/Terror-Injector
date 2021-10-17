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
        public static event EventHandler<TaskEventArgs> UninstalCompleted = delegate { };
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
                    SilentUninstallTerror();

            bool taskCancelled = false;

            //100000ms = 100s = 1m 40s
            WebClient webClient = new();

            if (!InjectorHelper.IsTerrorInstalled())
            {
                Uri downloadAddress = new("https://mistermodzzforum.space/Downloads/KPmcvgQdA34aJ8BX.zip");

                try
                {
                    Directory.CreateDirectory(InjectorHelper.TerrorDocuments);

                    {
                        webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                        await webClient.DownloadFileTaskAsync(downloadAddress, $"{InjectorHelper.TerrorDocuments}\\Terror.zip");
                    }
                }
                //catch (WebException ex) when (ex.Status == WebExceptionStatus.RequestCanceled)
                catch (Exception ex) when (ex is WebException)
                {
                    taskCancelled = true;
                    Debug.WriteLine(ex.ToString());
                }
            }

            return File.Exists(@$"{InjectorHelper.TerrorDocuments}\Terror.zip") & !taskCancelled;
        }

        /// <summary>
        /// Occurs when an asynchronous file download operation completes.
        /// </summary>
        /// <param name="sender">Reference to the control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private static void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            bool Downloaded = !(e.Cancelled || e.Error != null);

            //If download failed remove leftovers
            if (!Downloaded)
            {
                SilentUninstallTerror();
            }

            DownloadCompleted(null, new TaskEventArgs() { Result = Downloaded });
        }

        /// <summary>
        /// Install Terror
        /// </summary>
        /// <returns>True if installation was successful, otherwise false.</returns>
        public static Task<bool> InstallTerrorAsync()
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

                    result = CreateMenuFiles() && LockFiles(true);
                }
                catch (Exception)
                {

                }
            }

            InstallationCompleted(null, new TaskEventArgs() { Result = result });

            return Task.FromResult(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool CreateMenuFiles()
        {
            bool result;

            try
            {
                if (!Directory.Exists(InjectorHelper.TerrorAppdata))
                {
                    Directory.CreateDirectory(InjectorHelper.TerrorAppdata);
                }

                if (!File.Exists(InjectorHelper.MenuName))
                {
                    File.WriteAllText(InjectorHelper.MenuName, "UpP1YrVpA74DW11Y.menu\r\n");
                }

                if (!File.Exists(InjectorHelper.InjectionKey))
                {
                    File.WriteAllText(InjectorHelper.InjectionKey, "KpPWBO2JrpgzGwpDx\r\n");
                }

                if (!File.Exists(InjectorHelper.InstallerLogin))
                {
                    File.WriteAllText(InjectorHelper.InstallerLogin, "Username: test\r\nPassword: test\r\n");
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
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
                string versionFile = @$"{InjectorHelper.TerrorDocuments}\{latestVersion}.txt";

                if (!(result = File.Exists(versionFile)))
                {
                    if (await DownloadTerrorAsync())
                        result = await InstallTerrorAsync();
                }
            }

            //Latest version installed
            //If installed check temp files.
            if (result)
            {
                if(CreateMenuFiles())
                    LockFiles(true);
            }

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

            bool result = SilentUninstallTerror();

            UninstalCompleted(null, new TaskEventArgs() { Result = result });

            return result;
        }

        /// <summary>
        /// Uninstall Terror without GUI changes.
        /// </summary>
        /// <returns>True if successfully uninstalled, otherwise false.</returns>
        private static bool SilentUninstallTerror()
        {
            bool IsLocked = true;

            //Prevent uninstalling while it's injected.
            if (!InjectorHelper.IsTerrorInjected())
            {
                if (IsLocked = LockFiles(false))
                {
                    try
                    {
                        if (Directory.Exists(InjectorHelper.TerrorDocuments))
                            Directory.Delete(InjectorHelper.TerrorDocuments, true);

                        if (Directory.Exists(InjectorHelper.TerrorAppdata))
                            Directory.Delete(InjectorHelper.TerrorAppdata, true);
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return IsLocked && !Directory.Exists(InjectorHelper.TerrorDocuments) && !Directory.Exists(InjectorHelper.TerrorAppdata);
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
            FileAccessControl Login = new(new FileInfo(InjectorHelper.InstallerLogin));

            LockedFiles.Add(Key);
            LockedFiles.Add(Login);

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
        /// Force focus of GTAV for 20 seconds.
        /// </summary>
        public static async Task ForceGTAVFocus()
        {
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