﻿/*
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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Terror_Injector.Injector;
using Timer = System.Timers.Timer;

namespace Terror_Injector
{
    public partial class frmTerrorInjector : Form
    {
        private int counter;
        private DetectedStatus IsDetected;

        private readonly Timer timer = new() { Interval = 1000, Enabled = true };

        private bool IsChangingKey;

        public frmTerrorInjector()
        {
            InitializeComponent();
        }

        public async void StartInjection()
        {
            DllInjectionResult Result;

            int SelectedIndex = 0;

            this.Invoke((Action)(() => {
                SelectedIndex = menuSelect.SelectedIndex;
            }));

            //InjectorTasks.CreateMenuFiles();

            if (SelectedIndex == 0)
            {
                Result = InjectorHelper.StartInjection($"{InjectorHelper.TerrorDocuments}\\{InjectorHelper.GetTerrorMenuName()}");
            }
            else
            {
                //5LsNU8sd8BFwKa.5LsNU
                Result = InjectorHelper.StartInjection($"{InjectorHelper.MisterModzZDocuments}\\{InjectorHelper.GetMisterModzZMenuName()}");
                //InjectorTasks.CreateLogin();
            }

            string caption;
            string title;
            int sleep = 0;

            switch (Result)
            {
                case DllInjectionResult.Success:
                    caption = string.Empty;
                    title = string.Empty;

                    if (SelectedIndex == 0)
                    {
                        sleep = 20000;
                    }

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
                Action action;

                bool LoginCreated = false;

                bool created = await InjectorTasks.CreateAccount();

                bool installed = false;

                if (created) {
                    Debug.WriteLine("Created Account");

                    installed = await InjectorTasks.CreateMenuFilesAsync();
                }

                if (installed)
                    Debug.WriteLine("Created Menu Files");

                bool loggedin = await InjectorTasks.Menu_Login();

                if (loggedin)
                    Debug.WriteLine("Logged into Account");

                InjectorHelper.SwitchToGTA5();

                await Task.Delay(sleep);

                if (Result == DllInjectionResult.Success)
                {
                    action = new Action(() =>
                    {
                        timer.Stop();
                        lblStatus.Text = "Successfully Injected";

                        if (loggedin)
                            LoginCreated = InjectorTasks.CreateLogin();
                    });
                }
                else
                {
                    bool deleted = await InjectorTasks.DeleteAccountAsync();

                    if (deleted)
                        Debug.WriteLine("Deleted Account");

                    action = new Action(() =>
                    {
                        timer.Stop();
                        lblStatus.Text = "Injection Failed";

                        if (Result != DllInjectionResult.InjectionFailed)
                        {
                            InjectorHelper.SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
                            MessageBox.Show(caption, title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    });
                }

                UIInvoker(lblStatus, action);

                await Task.Delay(3000);

                InjectorHelper.SwitchToGTA5();

                UIInvoker(this, new Action(async () =>
                {
                    if (Result == DllInjectionResult.Success)
                    {
                        if (loggedin && LoginCreated)
                        {
                            this.Hide();

                            await Task.Delay(TimeSpan.FromSeconds(60));
                            InjectorTasks.DeleteLogin();

                            bool deleted = await InjectorTasks.DeleteAccountAsync();

                            if(deleted)
                                Debug.WriteLine("Deleted Account");

                            Debug.WriteLine("InstallerLogin: " + File.Exists(InjectorHelper.InstallerLogin).ToString());
                        }
                    }

                    this.Close();
                }));
            });
        }

        private delegate void UpdateUI(Control control, Action action);

        private void UIInvoker(Control control, Action action)
        {
            if ((bool)(control?.InvokeRequired))
            {
                control.BeginInvoke(new UpdateUI(UIInvoker), new object[] { control, action });
            }
            else
            {
                action();
            }
        }

        private async Task CheckForInjectorUpdates()
        {
            if (await InjectorHelper.UpdateAvailableAsync())
            {
                DialogResult result = await Task.Run(() => MessageBox.Show("An update is available.\n\nDownload Now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)); ;

                if (result == DialogResult.Yes)
                {
                    GitHubIcon_Click(null, null);

                    this.Close();
                }
            }
        }

        private async Task CheckForMenuUpdates()
        {
            UpdateInstallDir();
            UpdateInstallVer();

            if (await InjectorHelper.IsOnline())
            {
                if (!await InjectorTasks.UpdateTerror())
                {
                    //Failed to update/install
                }
            }

            UpdateInstallDir();
            UpdateInstallVer();
        }

        private void SetupEvents()
        {
            InjectorTasks.DownloadCompleted += InjectorTasks_OnDownloadCompleted;
            InjectorTasks.InstallationCompleted += InjectorTasks_OnInstallationCompleted;
            InjectorTasks.UninstallCompleted += InjectorTasks_OnUninstallCompleted;
            InjectorTasks.UpdateCompleted += InjectorTasks_OnUpdateCompleted;

            InjectorTasks.BeginDownloading += InjectorTasks_BeginDownloading;
            InjectorTasks.BeginInstalling += InjectorTasks_BeginInstalling;
            InjectorTasks.BeginUninstalling += InjectorTasks_BeginUninstalling;

            toolStripBtnClose.Click += ToolStripBtnClose_Click;
            toolStripBtnUninstall.Click += ToolStripBtnUninstall_Click;

            toolStripBtnClose.MouseEnter += ToolStripBtn_MouseEnter;
            toolStripBtnUninstall.MouseEnter += ToolStripBtn_MouseEnter;

            toolStripBtnClose.MouseLeave += ToolStripBtn_MouseLeave;
            toolStripBtnUninstall.MouseLeave += ToolStripBtn_MouseLeave;

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            //InjectorHelper.GetModuleVersionId();
            //Debug.WriteLine(InjectorHelper.BinaryHash());
        }

        private async void Terror_Injector_LoadAsync(object sender, EventArgs e)
        {
            menuSelect.SelectedIndex = 0;

            ToolStripBtns.ClickThrough = true;

            SetupEvents();

            InjectorHelper.SwitchToGTA5();

            InjectorTasks.DeleteLogin();

            UpdatelblDetected();

            await CheckForInjectorUpdates();
            await CheckForMenuUpdates();

            //if (InjectorHelper.IsTerrorInstalled())
            //{
            //    //if(InjectorTasks.CreateMenuFiles())
            //    //    InjectorTasks.Lock(true);
            //}

            if (InjectorHelper.IsTerrorInstalled()) {
                btnChangeOpenKey.Visible = true;
            }

            //if (InjectorHelper.IsMisterModzZInstalled()) {}

            menuSelect.Visible = true;

            await WaitForGTAV();

            InjectorHelper.SwitchToGTA5();

            timer.Stop();

            if (!IsChangingKey)
                btnInject.Visible = true;
                this.AcceptButton = btnInject;
        }

        private async void BtnInject_Click(object sender, EventArgs e)
        {
            counter = 0;
            timer.Start();

            btnChangeOpenKey.Visible = false;
            this.KeyPreview = false;

            btnInject.Visible = false;
            menuSelect.Visible = false;

            if (menuSelect.SelectedItem.ToString() == "MisterModzZ v2")
            {
                if (await InjectorTasks.DownloadMisterModzZAsync())
                    if (await InjectorTasks.InstallMisterModzZAsync())
                    {
                        Debug.WriteLine("MisterModzZ Installed Success");
                    }
            }

            UpdateStatusLabel("Injecting");

            WarningMessage(IsDetected);
        }

        private async void WarningMessage(DetectedStatus IsDetected)
        {
            string warningMessage = $"Terror is currently {IsDetected.ToString().ToLower()}, continue injecting?";

            if (IsDetected == DetectedStatus.Unknown)
            {
                warningMessage = $"The detection status of {warningMessage}";
            }

            DialogResult MSGResult;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            if (InjectorHelper.IsTerrorInjected())
            {
                warningMessage = "Terror is currently injected.\n\nOr\n\nTerror is auto-unloaded if you're not connected to the Internet while it's being injected.\n\nHowever, once Terror is injected it will work while completely offline.\n\nIf this is the case then you must restart GTA 5, connect to the Internet and try again.";
                buttons = MessageBoxButtons.OK;
            }

            MSGResult = await Task.Run(() => MessageBox.Show(warningMessage, $"Terror Injector", buttons, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly));

            if (MSGResult == DialogResult.No || MSGResult == DialogResult.OK)
            {
                //Hmmm
                UIInvoker(this, new Action(() => { this.Close(); }));
            }
            else
            {
                if (InjectorHelper.IsTerrorInstalled() || InjectorHelper.IsMisterModzZInstalled())
                {
                    await Task.Run(() => StartInjection());
                }
                else
                {
                    RestartApplication();
                }
            }
        }

        private void InjectorTasks_BeginUninstalling(object sender, EventArgs e)
        {
            btnInject.Visible = false;

            UpdateStatusLabel("Uninstalling");
        }

        private void InjectorTasks_BeginInstalling(object sender, EventArgs e)
        {
            UpdateStatusLabel("Installing");
        }

        private void InjectorTasks_BeginDownloading(object sender, EventArgs e)
        {
            UpdateStatusLabel("Downloading");
        }

        private void InjectorTasks_OnUninstallCompleted(object sender, TaskEventArgs e)
        {
            switch (menuSelect.SelectedItem.ToString())
            {
                case "Terror":
                    UpdateStatusLabel($"{(e.Result ? "Terror Uninstalled" : "Terror Uninstall Failed")}");
                    break;
                case "MisterModzZ v2":
                    UpdateStatusLabel($"{(e.Result ? "MisterModzZ Uninstalled" : "MisterModzZ Uninstall Failed")}");
                    break;
                default:
                    break;
            }
        }

        private void InjectorTasks_OnDownloadCompleted(object sender, TaskEventArgs e)
        {
            OnDownloadCompleted(e);
        }

        private void InjectorTasks_OnInstallationCompleted(object sender, TaskEventArgs e)
        {
            OnInstallationCompleted(e);
        }

        private void InjectorTasks_OnUpdateCompleted(object sender, TaskEventArgs e)
        {
            //if (e.Result)
            ToolStripBtns.Visible = true;
        }

        private async void OnInstallationCompleted(TaskEventArgs e)
        {
            if (!e.Result)
            {
                UpdateStatusLabel("Restarting");

                string message = "An error occurred restarting.";

                if (InjectorHelper.IsTerrorInjected())
                    message = $"Terror is currently injected in GTA5 please close GTA5 and try again.\n\n{message}";

                await Task.Run(() => MessageBox.Show(message, $"Terror Injector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly));

                RestartApplication();
            }
        }

        private async void OnDownloadCompleted(TaskEventArgs e)
        {
            if (!e.Result)
            {
                string label;
                string title;
                string msg;

                if (!await InjectorHelper.IsOnline())
                {
                    label = "You're Offline";
                    title = "You're Offline";
                    msg = "Please connect to the Internet to download Terror.";
                }
                else
                {
                    label = "Servers Offline";
                    title = "Download Servers Offline";
                    msg = "Terror download servers are offline.\n\nTry again later.";
                }

                timer.Stop();
                UpdateStatusLabel(label);

                await Task.Delay(500);

                if (!InjectorHelper.IsTerrorInstalled())
                {
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    this.Close();
                }
            }
        }

        private void UpdateStatusLabel(string Message)
        {
            UIInvoker(lblStatus, new Action(() => { lblStatus.Text = Message; }));
        }

        private async void UpdatelblDetected()
        {
            UpdateStatusLabel("Contacting Servers");
            IsDetected = await InjectorHelper.IsDetected();

            lblDetected.Text = $"{lblDetected.Text} {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(IsDetected.ToString().ToLower())}";
        }

        private void UpdateInstallDir()
        {
            UIInvoker(lblInstallDir, new Action(() =>
            {
                if (Directory.Exists(InjectorHelper.TerrorDocuments))
                    lblInstallDir.Text = $"Install Dir: Documents\\Terror";
                else
                    lblInstallDir.Text = $"Install Dir: N/A";
            }));
        }

        private async void UpdateInstallVer()
        {
            string installedVersion = InjectorHelper.GetInstalledVersion();
            string latestVersion = string.Empty;
            string versionFile = string.Empty;

            if (await InjectorHelper.CheckDownloadServer())
            {
                latestVersion = await InjectorHelper.GetLatestVersionAsync();
                versionFile = $"{InjectorHelper.TerrorDocuments}\\{latestVersion}.txt";

                latestVersion = latestVersion.Replace("Version ", string.Empty);
            }

            UIInvoker(lblLatestVersion, new Action(() =>
            {
                lblLatestVersion.Text = $"Latest Ver: {(!string.IsNullOrEmpty(latestVersion) ? latestVersion : "N/A")}";
            }));

            UIInvoker(lblInstalledVersion, new Action(() =>
            {
                lblInstalledVersion.Text = $"Installed Ver: {(!string.IsNullOrEmpty(installedVersion) ? installedVersion : "N/A")}";
            }));
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            AnimateStatus();
        }

        private void AnimateStatus()
        {
            counter++;

            string dots = string.Empty;

            for (int i = 0; i < counter; i++)
                dots += '.';

            if (counter == 4)
                counter = 0;

            UpdateStatusLabel($"{lblStatus.Text.Split('.')[0]}{dots}");
        }

        private async Task WaitForGTAV()
        {
            UpdateStatusLabel("Waiting for GTA5");

            while (!InjectorHelper.IsGTA5Running())
            {
                await Task.Delay(5000);
            }
        }

        private void RestartApplication()
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void ToolStripBtnUninstall_Click(object sender, EventArgs e)
        {
            bool uninstalled = false;

            switch (menuSelect.SelectedItem.ToString())
            {
                case "Terror":
                    if (InjectorHelper.IsTerrorInstalled())
                    {
                        timer.Stop();

                        if (!InjectorHelper.IsTerrorInjected())
                        {
                            if (uninstalled = InjectorTasks.UninstallTerror()) {
                                btnInject.Visible = false;

                                UpdateInstallDir();
                                UpdateInstallVer();
                            }
                        }
                        else
                            UpdateStatusLabel("Terror currently injected.");
                    }
                    break;
                case "MisterModzZ v2":
                    if (InjectorHelper.IsMisterModzZInstalled())
                    {
                        timer.Stop();

                        if (!InjectorHelper.IsMisterModzZInjected()) {
                            btnInject.Visible = false;

                            uninstalled = InjectorTasks.UninstallMisterModzZ();
                        }
                        else
                            UpdateStatusLabel("MisterModzZ currently injected.");
                    }
                    break;
            }

            if(uninstalled)
            {
                btnInject.Visible = false;

                if (InjectorHelper.IsTerrorInstalled() || InjectorHelper.IsMisterModzZInstalled()) {
                    Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromSeconds(8));

                        if (InjectorHelper.IsGTA5Running())
                        {
                            this.Invoke((Action)(() => {
                                btnInject.Visible = true;
                            }));
                        } else {
                            UpdateStatusLabel("Waiting for GTA5");

                            counter = 0;
                            timer.Start();
                        }

                        this.Invoke((Action)(() => {
                            menuSelect.SelectedIndex = InjectorHelper.IsTerrorInstalled() ? 0 : 1;
                        }));
                    });
                } else
                {
                    btnInject.Visible = false;
                    btnChangeOpenKey.Visible = false;
                }
            }
        }

        private void ToolStripBtnClose_Click(object sender, EventArgs e)
        {
            InjectorTasks.DeleteLogin();

            this.Close();
        }

        private void BtnInject_MouseEnter(object sender, EventArgs e)
        {
            btnInject.ForeColor = Color.Green;
        }

        private void BtnInject_MouseLeave(object sender, EventArgs e)
        {
            btnInject.ForeColor = Color.Red;
        }

        private void ToolStripBtn_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void ToolStripBtn_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void GitHubIcon_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", "https://github.com/MoistyMarley/Terror-Injector");
        }

        #region WinAPI DllImports
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        private void DragForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        
        private void btnChangeOpenKey_Click(object sender, EventArgs e)
        {
            ToggleChangeKey();
        }

        private async void frmTerrorInjector_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsChangingKey)
            {
                //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys
                bool NumPad = e.KeyValue >= (int)Keys.NumPad0 && e.KeyValue <= (int)Keys.NumPad9;
                bool Functions = e.KeyValue >= (int)Keys.F1 && e.KeyValue <= (int)Keys.F24;
                bool Digits = e.KeyValue >= (int)Keys.D0 && e.KeyValue <= (int)Keys.D9;
                bool Asterisk = e.KeyValue == (int)Keys.Multiply;

                bool KeyChanged = NumPad || Functions || Digits || Asterisk;

                if (KeyChanged)
                    UpdateStatusLabel(Asterisk ? "*" : e.KeyCode.ToString());

                IsChangingKey = KeyChanged;

                if (IsChangingKey)
                {
                    await Task.Delay(1000);
                    bool IsSet = InjectorHelper.SetOpenKey(e.KeyValue);

                    if (!IsSet)
                        MessageBox.Show("Failed to change key. \n\n try uninstalling");

                    ToggleChangeKey();
                }
            }
        }

        private void ToggleChangeKey() {
            //bool coll = lblInstallDir.Bounds.IntersectsWith(ToolStripBtns.Bounds);
            
            if (IsChangingKey)
            {
                btnChangeOpenKey.Text = "Open Key";

                menuSelect.Visible = true;

                if (InjectorHelper.IsGTA5Running())
                {
                    btnInject.Visible = true;
                    this.AcceptButton = btnInject;

                    UpdateStatusLabel("");
                }
                else
                {
                    UpdateStatusLabel("Waiting for GTA5");

                    counter = 0;
                    timer.Start();
                }
            }
            else
            {
                btnChangeOpenKey.Text = "Back";

                timer.Stop();

                btnInject.Visible = false;
                menuSelect.Visible = false;

                UpdateStatusLabel("Keys: F1-F22, NumPad, 0-9, or *");
            }

            IsChangingKey = !IsChangingKey;
        }

        private void lblStatus_SizeChanged(object sender, EventArgs e)
        {
            lblStatus.Left = (this.ClientSize.Width - lblStatus.Size.Width) / 2;
            lblStatus.Top = (this.ClientSize.Height - lblStatus.Size.Height) / 2;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;      // WS_EX_COMPOSITED
                return handleParam;
            }
        }

        private void menuSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnInject.Focus();

            bool IsInstalled = InjectorHelper.IsTerrorInstalled() || InjectorHelper.IsMisterModzZInstalled();

            if (IsInstalled && InjectorHelper.IsGTA5Running()) {
                lblStatus.Text = "";
                btnInject.Visible = true;
            }

            switch (menuSelect.SelectedItem.ToString())
            {
                case "Terror":
                    toolStripBtnUninstall.Text = "Uninstall Terror";
                    break;
                case "MisterModzZ v2":
                    toolStripBtnUninstall.Text = "Uninstall MisterModzZ";
                    break;
                default:
                    break;
            }
        }

        //https://stackoverflow.com/questions/11817062/align-text-in-combobox
        private void menuSelect_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            CustomComboBox cbx = sender as CustomComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }
    }
}
