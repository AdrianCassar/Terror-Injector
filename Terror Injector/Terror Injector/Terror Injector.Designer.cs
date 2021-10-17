
namespace Terror_Injector
{
    partial class frmTerrorInjector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTerrorInjector));
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLatestVersion = new System.Windows.Forms.Label();
            this.lblInstallDir = new System.Windows.Forms.Label();
            this.lblInstalledVersion = new System.Windows.Forms.Label();
            this.lblDetected = new System.Windows.Forms.Label();
            this.btnInject = new System.Windows.Forms.Button();
            this.ToolStripBtns = new Terror_Injector.ToolStripEx();
            this.toolStripBtnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnUninstall = new System.Windows.Forms.ToolStripButton();
            this.GitHubIcon = new System.Windows.Forms.PictureBox();
            this.ToolStripBtns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GitHubIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(450, 150);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "+";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.AutoSize = true;
            this.lblLatestVersion.BackColor = System.Drawing.Color.Black;
            this.lblLatestVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLatestVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLatestVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLatestVersion.ForeColor = System.Drawing.Color.Red;
            this.lblLatestVersion.Location = new System.Drawing.Point(0, 0);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblLatestVersion.Size = new System.Drawing.Size(60, 20);
            this.lblLatestVersion.TabIndex = 1;
            this.lblLatestVersion.Text = "Latest Ver:";
            this.lblLatestVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
            // 
            // lblInstallDir
            // 
            this.lblInstallDir.AutoSize = true;
            this.lblInstallDir.BackColor = System.Drawing.Color.Black;
            this.lblInstallDir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInstallDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblInstallDir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblInstallDir.ForeColor = System.Drawing.Color.Red;
            this.lblInstallDir.Location = new System.Drawing.Point(0, 130);
            this.lblInstallDir.Name = "lblInstallDir";
            this.lblInstallDir.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblInstallDir.Size = new System.Drawing.Size(59, 20);
            this.lblInstallDir.TabIndex = 3;
            this.lblInstallDir.Text = "Install Dir:";
            this.lblInstallDir.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
            // 
            // lblInstalledVersion
            // 
            this.lblInstalledVersion.AutoSize = true;
            this.lblInstalledVersion.BackColor = System.Drawing.Color.Black;
            this.lblInstalledVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInstalledVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblInstalledVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblInstalledVersion.ForeColor = System.Drawing.Color.Red;
            this.lblInstalledVersion.Location = new System.Drawing.Point(0, 20);
            this.lblInstalledVersion.Name = "lblInstalledVersion";
            this.lblInstalledVersion.Size = new System.Drawing.Size(73, 15);
            this.lblInstalledVersion.TabIndex = 4;
            this.lblInstalledVersion.Text = "Installed Ver:";
            this.lblInstalledVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
            // 
            // lblDetected
            // 
            this.lblDetected.AutoSize = true;
            this.lblDetected.BackColor = System.Drawing.Color.Black;
            this.lblDetected.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDetected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDetected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDetected.ForeColor = System.Drawing.Color.Red;
            this.lblDetected.Location = new System.Drawing.Point(0, 110);
            this.lblDetected.Name = "lblDetected";
            this.lblDetected.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblDetected.Size = new System.Drawing.Size(92, 20);
            this.lblDetected.TabIndex = 5;
            this.lblDetected.Text = "Detected Status:";
            this.lblDetected.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
            // 
            // btnInject
            // 
            this.btnInject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInject.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnInject.ForeColor = System.Drawing.Color.Red;
            this.btnInject.Location = new System.Drawing.Point(86, 48);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(279, 55);
            this.btnInject.TabIndex = 7;
            this.btnInject.Text = "Inject";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Visible = false;
            this.btnInject.Click += new System.EventHandler(this.BtnInject_Click);
            this.btnInject.MouseEnter += new System.EventHandler(this.BtnInject_MouseEnter);
            this.btnInject.MouseLeave += new System.EventHandler(this.BtnInject_MouseLeave);
            // 
            // ToolStripBtns
            // 
            this.ToolStripBtns.AutoSize = false;
            this.ToolStripBtns.BackColor = System.Drawing.Color.Black;
            this.ToolStripBtns.ClickThrough = true;
            this.ToolStripBtns.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolStripBtns.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ToolStripBtns.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripBtns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnClose,
            this.toolStripSeparator1,
            this.toolStripBtnUninstall});
            this.ToolStripBtns.Location = new System.Drawing.Point(296, 125);
            this.ToolStripBtns.Name = "ToolStripBtns";
            this.ToolStripBtns.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ToolStripBtns.Size = new System.Drawing.Size(154, 25);
            this.ToolStripBtns.Stretch = true;
            this.ToolStripBtns.TabIndex = 9;
            this.ToolStripBtns.Text = "toolStrip1";
            this.ToolStripBtns.Visible = false;
            // 
            // toolStripBtnClose
            // 
            this.toolStripBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnClose.ForeColor = System.Drawing.Color.Red;
            this.toolStripBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnClose.Name = "toolStripBtnClose";
            this.toolStripBtnClose.Size = new System.Drawing.Size(44, 22);
            this.toolStripBtnClose.Text = "Close";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripBtnUninstall
            // 
            this.toolStripBtnUninstall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnUninstall.ForeColor = System.Drawing.Color.Red;
            this.toolStripBtnUninstall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnUninstall.Name = "toolStripBtnUninstall";
            this.toolStripBtnUninstall.Size = new System.Drawing.Size(101, 22);
            this.toolStripBtnUninstall.Text = "Uninstall Terror";
            this.toolStripBtnUninstall.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // GitHubIcon
            // 
            this.GitHubIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GitHubIcon.Image = global::Terror_Injector.Properties.Resources.GitHub_Mark_Light_32px;
            this.GitHubIcon.Location = new System.Drawing.Point(418, 0);
            this.GitHubIcon.Name = "GitHubIcon";
            this.GitHubIcon.Size = new System.Drawing.Size(32, 32);
            this.GitHubIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.GitHubIcon.TabIndex = 10;
            this.GitHubIcon.TabStop = false;
            this.GitHubIcon.Click += new System.EventHandler(this.GitHubIcon_Click);
            // 
            // frmTerrorInjector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(450, 150);
            this.ControlBox = false;
            this.Controls.Add(this.GitHubIcon);
            this.Controls.Add(this.ToolStripBtns);
            this.Controls.Add(this.btnInject);
            this.Controls.Add(this.lblInstalledVersion);
            this.Controls.Add(this.lblLatestVersion);
            this.Controls.Add(this.lblDetected);
            this.Controls.Add(this.lblInstallDir);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTerrorInjector";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terror Injector";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Terror_Injector_LoadAsync);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
            this.ToolStripBtns.ResumeLayout(false);
            this.ToolStripBtns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GitHubIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLatestVersion;
        private System.Windows.Forms.Label lblInstallDir;
        private System.Windows.Forms.Label lblInstalledVersion;
        private System.Windows.Forms.Label lblDetected;
        private System.Windows.Forms.Button btnInject;
        private Terror_Injector.ToolStripEx ToolStripBtns;
        private System.Windows.Forms.ToolStripButton toolStripBtnClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripBtnUninstall;
        private System.Windows.Forms.PictureBox GitHubIcon;
    }
}

