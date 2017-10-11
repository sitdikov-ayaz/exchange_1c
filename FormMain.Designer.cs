namespace ExchangeVS
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AutoExchangeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.FileStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FlagFileStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FlagFileDeleteCurrentNodeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FlagFileDeleteAllNodesStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TagsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TagsOnStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TagsOffStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CloseAllSessionsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EchangeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DownloadStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DownloadAndAutoExchangeOnStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UploadStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UploadAndAutoExchangeOnStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timerAutoExchangeStart = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(513, 429);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(513, 479);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(513, 419);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(505, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 419);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 10);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AutoExchangeButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(98, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // AutoExchangeButton
            // 
            this.AutoExchangeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AutoExchangeButton.CheckOnClick = true;
            this.AutoExchangeButton.Image = global::Exchange.Properties.Resources.g1;
            this.AutoExchangeButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AutoExchangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AutoExchangeButton.Name = "AutoExchangeButton";
            this.AutoExchangeButton.Size = new System.Drawing.Size(88, 22);
            this.AutoExchangeButton.Text = "Автообмен";
            this.AutoExchangeButton.Click += new System.EventHandler(this.AutoExchangeButton_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileStripMenuItem,
            this.EditStripMenuItem,
            this.EchangeStripMenuItem});
            this.toolStrip2.Location = new System.Drawing.Point(3, 25);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(208, 25);
            this.toolStrip2.TabIndex = 0;
            // 
            // FileStripMenuItem
            // 
            this.FileStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsStripMenuItem,
            this.UpdateToolStripMenuItem,
            this.ExitStripMenuItem});
            this.FileStripMenuItem.Name = "FileStripMenuItem";
            this.FileStripMenuItem.Size = new System.Drawing.Size(49, 25);
            this.FileStripMenuItem.Text = "Файл";
            // 
            // SettingsStripMenuItem
            // 
            this.SettingsStripMenuItem.Name = "SettingsStripMenuItem";
            this.SettingsStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.SettingsStripMenuItem.Text = "Настройки";
            this.SettingsStripMenuItem.Click += new System.EventHandler(this.SettingsStripMenuItem_Click);
            // 
            // UpdateToolStripMenuItem
            // 
            this.UpdateToolStripMenuItem.Name = "UpdateToolStripMenuItem";
            this.UpdateToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.UpdateToolStripMenuItem.Text = "Обновить программу";
            this.UpdateToolStripMenuItem.Click += new System.EventHandler(this.UpdateToolStripMenuItem_Click);
            // 
            // ExitStripMenuItem
            // 
            this.ExitStripMenuItem.Name = "ExitStripMenuItem";
            this.ExitStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.ExitStripMenuItem.Text = "Выход";
            this.ExitStripMenuItem.Click += new System.EventHandler(this.ExitStripMenuItem_Click);
            // 
            // EditStripMenuItem
            // 
            this.EditStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FlagFileStripMenuItem,
            this.TagsStripMenuItem,
            this.toolStripSeparator1,
            this.CloseAllSessionsStripMenuItem});
            this.EditStripMenuItem.Name = "EditStripMenuItem";
            this.EditStripMenuItem.Size = new System.Drawing.Size(62, 25);
            this.EditStripMenuItem.Text = "Правка";
            // 
            // FlagFileStripMenuItem
            // 
            this.FlagFileStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FlagFileDeleteCurrentNodeStripMenuItem,
            this.FlagFileDeleteAllNodesStripMenuItem});
            this.FlagFileStripMenuItem.Name = "FlagFileStripMenuItem";
            this.FlagFileStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.FlagFileStripMenuItem.Text = "Флаг-файл";
            // 
            // FlagFileDeleteCurrentNodeStripMenuItem
            // 
            this.FlagFileDeleteCurrentNodeStripMenuItem.Name = "FlagFileDeleteCurrentNodeStripMenuItem";
            this.FlagFileDeleteCurrentNodeStripMenuItem.Size = new System.Drawing.Size(339, 22);
            this.FlagFileDeleteCurrentNodeStripMenuItem.Text = "Удалить флаг-файл выгрузки текущего узла";
            this.FlagFileDeleteCurrentNodeStripMenuItem.Click += new System.EventHandler(this.FlagFileDeleteCurrentNodeStripMenuItem_Click);
            // 
            // FlagFileDeleteAllNodesStripMenuItem
            // 
            this.FlagFileDeleteAllNodesStripMenuItem.Name = "FlagFileDeleteAllNodesStripMenuItem";
            this.FlagFileDeleteAllNodesStripMenuItem.Size = new System.Drawing.Size(339, 22);
            this.FlagFileDeleteAllNodesStripMenuItem.Text = "Удалить флаг-файлы выгрузки всех узлов";
            this.FlagFileDeleteAllNodesStripMenuItem.Click += new System.EventHandler(this.FlagFileDeleteAllNodesStripMenuItem_Click);
            // 
            // TagsStripMenuItem
            // 
            this.TagsStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TagsOnStripMenuItem,
            this.TagsOffStripMenuItem});
            this.TagsStripMenuItem.Name = "TagsStripMenuItem";
            this.TagsStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.TagsStripMenuItem.Text = "Пометки";
            // 
            // TagsOnStripMenuItem
            // 
            this.TagsOnStripMenuItem.Name = "TagsOnStripMenuItem";
            this.TagsOnStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.TagsOnStripMenuItem.Text = "Включить все";
            this.TagsOnStripMenuItem.Click += new System.EventHandler(this.TagsOnStripMenuItem_Click);
            // 
            // TagsOffStripMenuItem
            // 
            this.TagsOffStripMenuItem.Name = "TagsOffStripMenuItem";
            this.TagsOffStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.TagsOffStripMenuItem.Text = "Отключить все";
            this.TagsOffStripMenuItem.Click += new System.EventHandler(this.TagsOffStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(266, 6);
            // 
            // CloseAllSessionsStripMenuItem
            // 
            this.CloseAllSessionsStripMenuItem.Name = "CloseAllSessionsStripMenuItem";
            this.CloseAllSessionsStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.CloseAllSessionsStripMenuItem.Text = "Закрыть все соединения с базой";
            this.CloseAllSessionsStripMenuItem.Click += new System.EventHandler(this.CloseAllSessionsStripMenuItem_Click);
            // 
            // EchangeStripMenuItem
            // 
            this.EchangeStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DownloadStripMenuItem,
            this.DownloadAndAutoExchangeOnStripMenuItem,
            this.UploadStripMenuItem,
            this.UploadAndAutoExchangeOnStripMenuItem});
            this.EchangeStripMenuItem.Name = "EchangeStripMenuItem";
            this.EchangeStripMenuItem.Size = new System.Drawing.Size(56, 25);
            this.EchangeStripMenuItem.Text = "Обмен";
            // 
            // DownloadStripMenuItem
            // 
            this.DownloadStripMenuItem.Name = "DownloadStripMenuItem";
            this.DownloadStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.DownloadStripMenuItem.Text = "Загрузить";
            this.DownloadStripMenuItem.Click += new System.EventHandler(this.DownloadStripMenuItem_Click);
            // 
            // DownloadAndAutoExchangeOnStripMenuItem
            // 
            this.DownloadAndAutoExchangeOnStripMenuItem.Name = "DownloadAndAutoExchangeOnStripMenuItem";
            this.DownloadAndAutoExchangeOnStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.DownloadAndAutoExchangeOnStripMenuItem.Text = "Загрузить и включить автообмен";
            this.DownloadAndAutoExchangeOnStripMenuItem.Click += new System.EventHandler(this.DownloadAndAutoExchangeOnStripMenuItem_Click);
            // 
            // UploadStripMenuItem
            // 
            this.UploadStripMenuItem.Name = "UploadStripMenuItem";
            this.UploadStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.UploadStripMenuItem.Text = "Выгрузить";
            this.UploadStripMenuItem.Click += new System.EventHandler(this.UploadStripMenuItem_Click);
            // 
            // UploadAndAutoExchangeOnStripMenuItem
            // 
            this.UploadAndAutoExchangeOnStripMenuItem.Name = "UploadAndAutoExchangeOnStripMenuItem";
            this.UploadAndAutoExchangeOnStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.UploadAndAutoExchangeOnStripMenuItem.Text = "Выгрузить и включить автообмен";
            this.UploadAndAutoExchangeOnStripMenuItem.Click += new System.EventHandler(this.UploadAndAutoExchangeOnStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "g1.bmp");
            this.imageList1.Images.SetKeyName(1, "g2.bmp");
            // 
            // timerAutoExchangeStart
            // 
            this.timerAutoExchangeStart.Enabled = true;
            this.timerAutoExchangeStart.Interval = 1000;
            this.timerAutoExchangeStart.Tick += new System.EventHandler(this.timerAutoExchangeStart_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 479);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обмен";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripMenuItem FileStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingsStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FlagFileStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FlagFileDeleteCurrentNodeStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FlagFileDeleteAllNodesStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TagsStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TagsOnStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TagsOffStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EchangeStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DownloadStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DownloadAndAutoExchangeOnStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UploadStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UploadAndAutoExchangeOnStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem CloseAllSessionsStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AutoExchangeButton;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timerAutoExchangeStart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripMenuItem UpdateToolStripMenuItem;
    }
}

