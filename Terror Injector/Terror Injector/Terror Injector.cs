using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terror_Injector.Injector;

namespace Terror_Injector
{
    public enum DetectedStatus
    {
        Detected,
        Unknown,
        Undetcted,
    }

    public partial class Terror_Injector : Form
    {
        private int counter;
        private readonly string TerrorDocuments = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Documents\\Terror";
        private readonly string TerrorAppdata = $"{ Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Terror";

        #region WinAPI DllImports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        public Terror_Injector()
        {
            InitializeComponent();
        }

        public async void StartInjection()
        {
            DllInjectionResult Result;

            Result = Inject_DLL($"{TerrorDocuments}\\{GetTerrorMenuName()}");
            //Result = DllInjectionResult.InjectionFailed

            string caption;
            string title;
            int sleep = 3000;

            switch (Result)
            {
                case DllInjectionResult.Success:
                    caption = "Injection Successful\n\nEnjoy :)";
                    title = "Injection Successful";
                    sleep = 20000;
                    break;
                case DllInjectionResult.DllNotFound:
                    DialogResult MSGResult = await Task.Run(() => MessageBox.Show("Terror Menu not found.\n\nWould you like to download and install Terror?", "Terror Injector", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly));

                    if (MSGResult == DialogResult.Yes)
                    {
                        RestartApplication();
                    }

                    caption = "Terror Menu File not found.";
                    title = "Terror Injector";
                    break;
                case DllInjectionResult.ProcessNotFound:
                    //\n\nIf GTA5 is running in task manager, try running \"As Administrator\"
                    caption = "Unable to find GTA5 process!";
                    title = "Terror Injector";
                    break;
                case DllInjectionResult.InjectionFailed:
                default:
                    caption = "Injection Failed";
                    title = "Terror Injector";
                    break;
            }

            await Task.Run(async () =>
            {
                SwitchToGTA5();

                await Task.Delay(sleep);

                Action action;

                if (Result == DllInjectionResult.Success)
                {
                    action = new Action(() =>
                    {
                        timerAnimateText.Stop();
                        lblStatus.Text = "Successfully Injected";
                    });
                }
                else
                {
                    action = new Action(() =>
                    {
                        timerAnimateText.Stop();
                        lblStatus.Text = "Injection Failed";

                        if (Result != DllInjectionResult.InjectionFailed)
                        {
                            SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
                            MessageBox.Show(caption, title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    });
                }

                UIInvoker(lblStatus, action);

                await Task.Delay(3000);

                SwitchToGTA5();

                UIInvoker(this, new Action(() =>
                {
                    this.Close();
                }));
            });
        }

        private bool IsGTA5Running()
        {
            return Process.GetProcessesByName("GTA5").FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero) != null;
        }

        private void SwitchToGTA5()
        {
            Process GTAV_Process = Process.GetProcessesByName("GTA5").FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero);

            if (GTAV_Process != null)
            {
                ShowWindow(GTAV_Process.MainWindowHandle, 3);
                SetForegroundWindow(GTAV_Process.MainWindowHandle);
            }
        }

        private DllInjectionResult Inject_DLL(string Path)
        {
            return Injection.GetInstance.Inject("GTA5", Path);
        }

        private string GetTerrorMenuName()
        {
            string MenuNamePath = $"{TerrorAppdata}\\MenuName.txt";
            string MenuName = string.Empty;

            if (File.Exists(MenuNamePath))
            {
                try
                {
                    MenuName = File.ReadAllText(MenuNamePath).Trim();
                }
                catch (Exception)
                {

                }
            }

            return MenuName;
        }

        private DetectedStatus IsDetected()
        {
            string detectedStatus = GetDetectedStatus();
            DetectedStatus result;

            switch (detectedStatus)
            {
                case "Detected":
                    result = DetectedStatus.Detected;
                    break;
                case "Undetected":
                    result = DetectedStatus.Undetcted;
                    break;
                default:
                    result = DetectedStatus.Unknown;
                    break;
            }

            return result;
        }

        private string GetDetectedStatus()
        {
            return GetWebRequest(new Uri("https://mistermodzzforum.space/authserver/FreeMenuDetected.php"));
        }

        private string GetWebRequest(Uri url)
        {
            string result;
            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd().Trim();
                }
            }
            catch (Exception)
            {

                result = string.Empty;
            }

            return result;
        }

        private string GetLatestVersion()
        {
            return GetWebRequest(new Uri("https://mistermodzzforum.space/authserver/Terrormenu.php"));
        }

        private void UIInvoker(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke((MethodInvoker)delegate
                {
                    action();
                });
            }
            else
            {
                action();
            }
        }

        private string GetInstalledVersion()
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

                    DirectoryInfo Dir = new DirectoryInfo(TerrorDocuments);
                    FileInfo File = Dir.GetFiles("*.txt").FirstOrDefault((file) => file.Name.StartsWith("Version"));

                    if (File != null)
                    {
                        version = Path.GetFileNameWithoutExtension(File.Name);
                    }
                }
            }

            return version;
        }

        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        private bool IsTerrorInjected()
        {
            return IsTerrorInstalled() && IsFileLocked(new FileInfo($"{TerrorDocuments}\\{GetTerrorMenuName()}")) && IsGTA5Running();
        }

        private async Task<bool> CheckMenuVersion()
        {
            bool result = false;

            UIInvoker(lblInstallDir, new Action(() =>
            {
                if (Directory.Exists(TerrorDocuments))
                {
                    lblInstallDir.Text = $"Install Dir: {TerrorDocuments}";
                }
                else
                {
                    lblInstallDir.Text = $"Install Dir: N/A";
                }
            }));

            string installedVersion = GetInstalledVersion();
            string latestVersion = string.Empty;
            string versionFile = string.Empty;

            if (CheckDownloadServer())
            {
                latestVersion = GetLatestVersion();
                versionFile = $"{TerrorDocuments}\\{latestVersion}.txt";

                if (!(result = File.Exists(versionFile)))
                {
                    if (await Task.Run(() => DownloadTerror("Downloading")))
                    {
                        if (await Task.Run(InstallTerrorAsync))
                        {
                            result = await Task.Run(CheckMenuVersion);
                        }
                    }
                }
            }

            UIInvoker(lblInstalledVersion, new Action(() =>
            {
                if (File.Exists(versionFile))
                {
                    lblLatestVersion.Text = $"Latest Ver: {latestVersion.Replace("Version ", string.Empty)}";
                }
                else
                {
                    lblLatestVersion.Text = $"Latest Ver: N/A";
                }

                if (!installedVersion.Equals(string.Empty))
                {
                    lblInstalledVersion.Text = $"Install Ver: {installedVersion.Replace("Version ", string.Empty)}";
                }
                else
                {
                    lblInstalledVersion.Text = $"Install Ver: N/A";
                }
            }));

            return result;
        }

        private bool PingWeb(string url) {
            IPStatus result;

            try
            {
                result = new Ping().Send(url).Status;
            }
            catch (Exception)
            {
                result = IPStatus.TimedOut;
            }

            return result == IPStatus.Success;
        }

        private bool CheckDownloadServer()
        {
            return PingWeb("mistermodzzforum.space");
        }

        private bool IsOnline()
        {
            return PingWeb("google.com");
        }

        private async void UninstallTerror()
        {
            try
            {
                if (Directory.Exists(TerrorDocuments))
                {
                    Directory.Delete(TerrorDocuments, true);
                }

                if (Directory.Exists(TerrorAppdata))
                {
                    Directory.Delete(TerrorAppdata, true);
                }
            }
            catch (Exception)
            {
                await Task.Run(() => MessageBox.Show("Error", $"Terror Injector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly));
            }
        }

        private async Task<bool> DownloadTerror(string caption)
        {
            UIInvoker(lblStatus, new Action(() =>
            {
                lblStatus.Text = caption;
            }));

            bool result = false;

            await Task.Run(async () =>
            {
                if (CheckDownloadServer())
                {
                    await Task.Run(() => UninstallTerror());

                    Uri downloadAddress = new("https://mistermodzzforum.space/Downloads/Terror.zip");

                    using WebClient webClient = new();
                    try
                    {
                        Directory.CreateDirectory(TerrorDocuments);
                        webClient.DownloadFile(downloadAddress, $"{TerrorDocuments}\\Terror.zip");

                        result = true;
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
                else
                {
                    UIInvoker(lblStatus, new Action(() =>
                    {
                        lblStatus.Text = "Servers Offline";
                    }));

                    MessageBox.Show("Terror download servers are offline.\n\nTry again later.", "Download Servers Offline", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    UIInvoker(this, new Action(() =>
                    {
                        this.Close();
                    }));
                }
            });

            return result;
        }

        private async Task<bool> InstallTerrorAsync()
        {
            UIInvoker(lblInstallDir, new Action(() =>
            {
                lblStatus.Text = "Installing";
            }));

            bool result = false;

            string TerrorZip = $"{TerrorDocuments}\\Terror.zip";

            if (File.Exists(TerrorZip))
            {
                try
                {
                    if (!IsTerrorInstalled())
                    {
                        Directory.CreateDirectory(TerrorAppdata);
                        Directory.CreateDirectory(TerrorDocuments);
                    }

                    ZipFile.ExtractToDirectory(TerrorZip, TerrorDocuments, true);
                    File.Delete(TerrorZip);

                    File.WriteAllText($"{TerrorAppdata}\\MenuName.txt", "UpP1YrVpA74DW11Y.menu");
                }
                catch (Exception)
                {
                    UIInvoker(lblStatus, new Action(() =>
                    {
                        lblStatus.Text = "Restarting";
                    }));

                    string message = "An error occurred restarting.";

                    if (IsTerrorInjected())
                    {
                        message = $"Terror is currently injected in GTA5 please close GTA5 and try again.\n\n{message}";
                    }

                    await Task.Run(() => MessageBox.Show(message, $"Terror Injector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly));

                    RestartApplication();
                }

                result = true;
            }

            return result;
        }

        private bool IsTerrorInstalled()
        {
            bool result;

            if (result = Directory.Exists(TerrorDocuments))
            {
                int count = Directory.GetDirectories(TerrorDocuments).Length + Directory.GetFiles(TerrorDocuments).Length;

                result = count >= 6 && File.Exists($"{TerrorDocuments}\\{GetTerrorMenuName()}");
            }

            return result;
        }

        private void timerAnimateText_Tick(object sender, EventArgs e)
        {
            counter++;

            string dots = string.Empty;

            for (int i = 0; i < counter; i++)
                dots += '.';

            if (counter == 4)
                counter = 0;

            lblStatus.Text = $"{lblStatus.Text.Split('.')[0]}{dots}";
        }

        private async void Terror_Injector_LoadAsync(object sender, EventArgs e)
        {
            SwitchToGTA5();

            lblStatus.Text = "Contacting Servers";
            DetectedStatus isDetected = await Task.Run(() => IsDetected());

            lblDetected.Text = $"{lblDetected.Text} {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(isDetected.ToString().ToLower())}";

            if (!IsTerrorInstalled())
            {
                if (await Task.Run(() => DownloadTerror("Downloading")))
                {
                    await Task.Run(InstallTerrorAsync);
                }
            }

            await Task.Run(CheckMenuVersion);

            lblStatus.Text = "Injecting";
            string warningMessage = $"Terror is currently {isDetected.ToString().ToLower()}, continue injecting?";

            if (isDetected == DetectedStatus.Unknown)
            {
                warningMessage = $"The detection status of {warningMessage}";
            }

            DialogResult MSGResult;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            if (IsTerrorInjected())
            {
                warningMessage = "Terror is already injected.\n\nOr\n\nTerror is auto-unloaded if you're not connected to the Internet while it's being injected.\n\nHowever, once Terror is injected it will work while completely offline.\n\nIf this is the case then you must restart GTA 5, connect to the Internet and try again.";
                buttons = MessageBoxButtons.OK;
            }

            MSGResult = await Task.Run(() => MessageBox.Show(warningMessage, $"Terror Injector", buttons, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly));

            if (MSGResult == DialogResult.No || MSGResult == DialogResult.OK)
            {
                this.Close();
            }
            else
            {
                if (IsTerrorInstalled())
                {
                    await Task.Run(() => StartInjection());
                }
                else
                {
                    RestartApplication();
                }
            }

            timerAnimateText.Start();
        }

        private void RestartApplication()
        {
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
