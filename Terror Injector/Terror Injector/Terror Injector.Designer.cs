
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
            this.btnChangeOpenKey = new System.Windows.Forms.Button();
            this.menuSelect = new Terror_Injector.CustomComboBox();
            this.ToolStripBtns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GitHubIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(207, 100);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 40);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "+";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.SizeChanged += new System.EventHandler(this.lblStatus_SizeChanged);
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
            this.lblInstallDir.Location = new System.Drawing.Point(0, 180);
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
            this.lblDetected.Location = new System.Drawing.Point(0, 160);
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
            this.btnInject.Location = new System.Drawing.Point(86, 90);
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
            this.ToolStripBtns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolStripBtns.BackColor = System.Drawing.Color.Black;
            this.ToolStripBtns.ClickThrough = true;
            this.ToolStripBtns.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolStripBtns.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ToolStripBtns.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripBtns.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.ToolStripBtns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnClose,
            this.toolStripSeparator1,
            this.toolStripBtnUninstall});
            this.ToolStripBtns.Location = new System.Drawing.Point(283, 175);
            this.ToolStripBtns.Name = "ToolStripBtns";
            this.ToolStripBtns.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ToolStripBtns.Size = new System.Drawing.Size(166, 25);
            this.ToolStripBtns.TabIndex = 9;
            this.ToolStripBtns.Text = "toolStrip1";
            this.ToolStripBtns.Visible = false;
            // 
            // toolStripBtnClose
            // 
            this.toolStripBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnClose.ForeColor = System.Drawing.Color.Red;
            this.toolStripBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnClose.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnClose.Name = "toolStripBtnClose";
            this.toolStripBtnClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripBtnClose.Size = new System.Drawing.Size(44, 25);
            this.toolStripBtnClose.Text = "Close";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripBtnUninstall
            // 
            this.toolStripBtnUninstall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnUninstall.ForeColor = System.Drawing.Color.Red;
            this.toolStripBtnUninstall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnUninstall.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnUninstall.Name = "toolStripBtnUninstall";
            this.toolStripBtnUninstall.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripBtnUninstall.Size = new System.Drawing.Size(101, 25);
            this.toolStripBtnUninstall.Text = "Uninstall Terror";
            // 
            // GitHubIcon
            // 
            this.GitHubIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GitHubIcon.Image = global::Terror_Injector.Properties.Resources.GitHub_Mark_Light_32px;
            this.GitHubIcon.Location = new System.Drawing.Point(417, 0);
            this.GitHubIcon.Margin = new System.Windows.Forms.Padding(2);
            this.GitHubIcon.Name = "GitHubIcon";
            this.GitHubIcon.Size = new System.Drawing.Size(32, 32);
            this.GitHubIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.GitHubIcon.TabIndex = 10;
            this.GitHubIcon.TabStop = false;
            this.GitHubIcon.Click += new System.EventHandler(this.GitHubIcon_Click);
            // 
            // btnChangeOpenKey
            // 
            this.btnChangeOpenKey.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnChangeOpenKey.AutoSize = true;
            this.btnChangeOpenKey.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangeOpenKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeOpenKey.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnChangeOpenKey.ForeColor = System.Drawing.Color.Red;
            this.btnChangeOpenKey.Location = new System.Drawing.Point(190, 8);
            this.btnChangeOpenKey.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangeOpenKey.Name = "btnChangeOpenKey";
            this.btnChangeOpenKey.Size = new System.Drawing.Size(70, 27);
            this.btnChangeOpenKey.TabIndex = 11;
            this.btnChangeOpenKey.Text = "Open Key";
            this.btnChangeOpenKey.UseVisualStyleBackColor = true;
            this.btnChangeOpenKey.Visible = false;
            this.btnChangeOpenKey.Click += new System.EventHandler(this.btnChangeOpenKey_Click);
            // 
            // menuSelect
            // 
            this.menuSelect.BackColor = System.Drawing.Color.Black;
            this.menuSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menuSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.menuSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.menuSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuSelect.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuSelect.ForeColor = System.Drawing.Color.Red;
            this.menuSelect.FormattingEnabled = true;
            this.menuSelect.Items.AddRange(new object[] {
            "Terror",
            "MisterModzZ v2"});
            this.menuSelect.Location = new System.Drawing.Point(163, 53);
            this.menuSelect.Name = "menuSelect";
            this.menuSelect.Size = new System.Drawing.Size(125, 26);
            this.menuSelect.TabIndex = 13;
            this.menuSelect.Visible = false;
            this.menuSelect.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.menuSelect_DrawItem);
            this.menuSelect.SelectedIndexChanged += new System.EventHandler(this.menuSelect_SelectedIndexChanged);
            // 
            // frmTerrorInjector
            // 
            this.AcceptButton = this.btnChangeOpenKey;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.btnChangeOpenKey;
            this.ClientSize = new System.Drawing.Size(450, 200);
            this.ControlBox = false;
            this.Controls.Add(this.menuSelect);
            this.Controls.Add(this.GitHubIcon);
            this.Controls.Add(this.lblInstalledVersion);
            this.Controls.Add(this.lblLatestVersion);
            this.Controls.Add(this.lblDetected);
            this.Controls.Add(this.lblInstallDir);
            this.Controls.Add(this.ToolStripBtns);
            this.Controls.Add(this.btnChangeOpenKey);
            this.Controls.Add(this.btnInject);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(464, 250);
            this.MinimizeBox = false;
            this.Name = "frmTerrorInjector";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terror Injector";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Terror_Injector_LoadAsync);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTerrorInjector_KeyDown);
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
        private System.Windows.Forms.Button btnChangeOpenKey;
        private CustomComboBox menuSelect;
    }
}

