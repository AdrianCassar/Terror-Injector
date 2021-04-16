
namespace Terror_Injector
{
    partial class Terror_Injector
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Terror_Injector));
            this.lblStatus = new System.Windows.Forms.Label();
            this.timerAnimateText = new System.Windows.Forms.Timer(this.components);
            this.lblLatestVersion = new System.Windows.Forms.Label();
            this.lblInstallDir = new System.Windows.Forms.Label();
            this.lblInstalledVersion = new System.Windows.Forms.Label();
            this.lblDetected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(450, 120);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Injecting";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerAnimateText
            // 
            this.timerAnimateText.Enabled = true;
            this.timerAnimateText.Interval = 1000;
            this.timerAnimateText.Tick += new System.EventHandler(this.timerAnimateText_Tick);
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.AutoSize = true;
            this.lblLatestVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLatestVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLatestVersion.ForeColor = System.Drawing.Color.Red;
            this.lblLatestVersion.Location = new System.Drawing.Point(0, 0);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblLatestVersion.Size = new System.Drawing.Size(59, 18);
            this.lblLatestVersion.TabIndex = 1;
            this.lblLatestVersion.Text = "Latest Ver:";
            // 
            // lblInstallDir
            // 
            this.lblInstallDir.AutoSize = true;
            this.lblInstallDir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInstallDir.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblInstallDir.ForeColor = System.Drawing.Color.Red;
            this.lblInstallDir.Location = new System.Drawing.Point(0, 102);
            this.lblInstallDir.Name = "lblInstallDir";
            this.lblInstallDir.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblInstallDir.Size = new System.Drawing.Size(59, 18);
            this.lblInstallDir.TabIndex = 3;
            this.lblInstallDir.Text = "Install Dir:";
            // 
            // lblInstalledVersion
            // 
            this.lblInstalledVersion.AutoSize = true;
            this.lblInstalledVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInstalledVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblInstalledVersion.ForeColor = System.Drawing.Color.Red;
            this.lblInstalledVersion.Location = new System.Drawing.Point(0, 18);
            this.lblInstalledVersion.Name = "lblInstalledVersion";
            this.lblInstalledVersion.Size = new System.Drawing.Size(73, 13);
            this.lblInstalledVersion.TabIndex = 4;
            this.lblInstalledVersion.Text = "Installed Ver:";
            // 
            // lblDetected
            // 
            this.lblDetected.AutoSize = true;
            this.lblDetected.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDetected.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDetected.ForeColor = System.Drawing.Color.Red;
            this.lblDetected.Location = new System.Drawing.Point(0, 84);
            this.lblDetected.Name = "lblDetected";
            this.lblDetected.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblDetected.Size = new System.Drawing.Size(91, 18);
            this.lblDetected.TabIndex = 5;
            this.lblDetected.Text = "Detected Status:";
            // 
            // Terror_Injector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(450, 120);
            this.ControlBox = false;
            this.Controls.Add(this.lblDetected);
            this.Controls.Add(this.lblInstalledVersion);
            this.Controls.Add(this.lblInstallDir);
            this.Controls.Add(this.lblLatestVersion);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Terror_Injector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terror Injector";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Terror_Injector_LoadAsync);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timerAnimateText;
        private System.Windows.Forms.Label lblLatestVersion;
        private System.Windows.Forms.Label lblInstallDir;
        private System.Windows.Forms.Label lblInstalledVersion;
        private System.Windows.Forms.Label lblDetected;
    }
}

