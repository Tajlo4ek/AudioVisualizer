namespace AudioVisualizer
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.pbMain = new System.Windows.Forms.PictureBox();
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miOverAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMin = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            this.cmsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbMain
            // 
            this.pbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbMain.Location = new System.Drawing.Point(0, 0);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(617, 376);
            this.pbMain.TabIndex = 4;
            this.pbMain.TabStop = false;
            // 
            // cmsMain
            // 
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOverAll,
            this.miOptions});
            this.cmsMain.Name = "cmsMain";
            this.cmsMain.Size = new System.Drawing.Size(173, 48);
            // 
            // miOverAll
            // 
            this.miOverAll.Name = "miOverAll";
            this.miOverAll.Size = new System.Drawing.Size(172, 22);
            this.miOverAll.Text = "Поверх всех окон";
            this.miOverAll.Click += new System.EventHandler(this.MiOverAll_Click);
            // 
            // miOptions
            // 
            this.miOptions.Name = "miOptions";
            this.miOptions.Size = new System.Drawing.Size(172, 22);
            this.miOptions.Text = "Настройки";
            this.miOptions.Click += new System.EventHandler(this.MiOptions_Click);
            // 
            // notifyIconMin
            // 
            this.notifyIconMin.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIconMin.BalloonTipText = "asdsa";
            this.notifyIconMin.BalloonTipTitle = "asfasf";
            this.notifyIconMin.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMin.Icon")));
            this.notifyIconMin.Text = "Visualizer";
            this.notifyIconMin.Click += new System.EventHandler(this.NotifyIconMin_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 376);
            this.ContextMenuStrip = this.cmsMain;
            this.Controls.Add(this.pbMain);
            this.MinimumSize = new System.Drawing.Size(350, 150);
            this.Name = "MainWindow";
            this.Text = "Visualizer";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            this.cmsMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbMain;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem miOverAll;
        private System.Windows.Forms.ToolStripMenuItem miOptions;
        private System.Windows.Forms.NotifyIcon notifyIconMin;
    }
}

