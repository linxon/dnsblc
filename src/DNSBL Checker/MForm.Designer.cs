namespace DNSBL_Checker
{
    partial class MForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MForm));
            this.ResultBox = new System.Windows.Forms.ListView();
            this.Ip_address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Is_black_listed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.On_Server = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResBoxContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearRBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ScanBtn = new System.Windows.Forms.Button();
            this.UpdateLink = new System.Windows.Forms.LinkLabel();
            this.StopBtn = new System.Windows.Forms.Button();
            this.labelAllCount = new System.Windows.Forms.Label();
            this.labelBackCount = new System.Windows.Forms.Label();
            this.labelWhiteCount = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AddressEdit = new System.Windows.Forms.ComboBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ResBoxContextMenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResultBox
            // 
            this.ResultBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultBox.BackColor = System.Drawing.Color.Azure;
            this.ResultBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Ip_address,
            this.Is_black_listed,
            this.On_Server});
            this.ResultBox.ContextMenuStrip = this.ResBoxContextMenuStrip;
            this.ResultBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.ResultBox.GridLines = true;
            this.ResultBox.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ResultBox.Location = new System.Drawing.Point(4, 31);
            this.ResultBox.MultiSelect = false;
            this.ResultBox.Name = "ResultBox";
            this.ResultBox.Size = new System.Drawing.Size(584, 389);
            this.ResultBox.TabIndex = 2;
            this.ResultBox.UseCompatibleStateImageBehavior = false;
            this.ResultBox.View = System.Windows.Forms.View.Details;
            // 
            // Ip_address
            // 
            this.Ip_address.Text = "Адресс";
            this.Ip_address.Width = 180;
            // 
            // Is_black_listed
            // 
            this.Is_black_listed.Text = "Статус";
            // 
            // On_Server
            // 
            this.On_Server.Text = "Сервер";
            this.On_Server.Width = 220;
            // 
            // ResBoxContextMenuStrip
            // 
            this.ResBoxContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveAsToolStripMenuItem,
            this.ClearRBoxToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ExitToolStripMenuItem});
            this.ResBoxContextMenuStrip.Name = "ResBoxContextMenuStrip";
            this.ResBoxContextMenuStrip.Size = new System.Drawing.Size(156, 76);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Enabled = false;
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.SaveAsToolStripMenuItem.Text = "Соханить как...";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // ClearRBoxToolStripMenuItem
            // 
            this.ClearRBoxToolStripMenuItem.Name = "ClearRBoxToolStripMenuItem";
            this.ClearRBoxToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.ClearRBoxToolStripMenuItem.Text = "Очистить";
            this.ClearRBoxToolStripMenuItem.Click += new System.EventHandler(this.ClearRBoxToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.ScanBtn);
            this.panel1.Controls.Add(this.UpdateLink);
            this.panel1.Controls.Add(this.StopBtn);
            this.panel1.Controls.Add(this.labelAllCount);
            this.panel1.Controls.Add(this.labelBackCount);
            this.panel1.Controls.Add(this.labelWhiteCount);
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Location = new System.Drawing.Point(594, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 421);
            this.panel1.TabIndex = 1;
            // 
            // ScanBtn
            // 
            this.ScanBtn.AutoEllipsis = true;
            this.ScanBtn.Location = new System.Drawing.Point(6, 4);
            this.ScanBtn.Name = "ScanBtn";
            this.ScanBtn.Size = new System.Drawing.Size(102, 39);
            this.ScanBtn.TabIndex = 9;
            this.ScanBtn.Text = "&Сканировать";
            this.ScanBtn.UseVisualStyleBackColor = true;
            this.ScanBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // UpdateLink
            // 
            this.UpdateLink.AutoSize = true;
            this.UpdateLink.Location = new System.Drawing.Point(10, 77);
            this.UpdateLink.Name = "UpdateLink";
            this.UpdateLink.Size = new System.Drawing.Size(95, 13);
            this.UpdateLink.TabIndex = 8;
            this.UpdateLink.TabStop = true;
            this.UpdateLink.Text = "Обновить список";
            this.UpdateLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // StopBtn
            // 
            this.StopBtn.Enabled = false;
            this.StopBtn.Location = new System.Drawing.Point(6, 49);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(102, 25);
            this.StopBtn.TabIndex = 7;
            this.StopBtn.Text = "С&топ";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // labelAllCount
            // 
            this.labelAllCount.AutoSize = true;
            this.labelAllCount.Location = new System.Drawing.Point(0, 118);
            this.labelAllCount.Name = "labelAllCount";
            this.labelAllCount.Size = new System.Drawing.Size(49, 13);
            this.labelAllCount.TabIndex = 6;
            this.labelAllCount.Text = "Всего: 0";
            // 
            // labelBackCount
            // 
            this.labelBackCount.AutoSize = true;
            this.labelBackCount.ForeColor = System.Drawing.Color.Red;
            this.labelBackCount.Location = new System.Drawing.Point(0, 144);
            this.labelBackCount.Name = "labelBackCount";
            this.labelBackCount.Size = new System.Drawing.Size(105, 13);
            this.labelBackCount.TabIndex = 5;
            this.labelBackCount.Text = "В черном списке: 0";
            // 
            // labelWhiteCount
            // 
            this.labelWhiteCount.AutoSize = true;
            this.labelWhiteCount.ForeColor = System.Drawing.Color.Green;
            this.labelWhiteCount.Location = new System.Drawing.Point(0, 131);
            this.labelWhiteCount.Name = "labelWhiteCount";
            this.labelWhiteCount.Size = new System.Drawing.Size(88, 13);
            this.labelWhiteCount.TabIndex = 3;
            this.labelWhiteCount.Text = "Нейтральные: 0";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(6, 396);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(102, 25);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "&Выход";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.AddressEdit);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(596, 28);
            this.panel2.TabIndex = 2;
            // 
            // AddressEdit
            // 
            this.AddressEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressEdit.BackColor = System.Drawing.SystemColors.Window;
            this.AddressEdit.ForeColor = System.Drawing.SystemColors.Desktop;
            this.AddressEdit.FormattingEnabled = true;
            this.AddressEdit.Location = new System.Drawing.Point(4, 5);
            this.AddressEdit.Name = "AddressEdit";
            this.AddressEdit.Size = new System.Drawing.Size(584, 21);
            this.AddressEdit.TabIndex = 0;
            this.AddressEdit.Tag = "";
            this.AddressEdit.Text = "google.com";
            this.AddressEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressEdit_KeyDown);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 424);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(714, 22);
            this.statusStrip.TabIndex = 4;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(160, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // MForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 446);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ResultBox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "dnsblc";
            this.ResBoxContextMenuStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ResultBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ColumnHeader Ip_address;
        private System.Windows.Forms.ColumnHeader Is_black_listed;
        private System.Windows.Forms.ColumnHeader On_Server;
        private System.Windows.Forms.Label labelBackCount;
        private System.Windows.Forms.Label labelWhiteCount;
        private System.Windows.Forms.Label labelAllCount;
        private System.Windows.Forms.Button StopBtn;
        public System.Windows.Forms.ComboBox AddressEdit;
        private System.Windows.Forms.LinkLabel UpdateLink;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ContextMenuStrip ResBoxContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ClearRBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.Button ScanBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

