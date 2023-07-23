using AudioVisualizer.AudioSpectrum;
using AudioVisualizer.DataSaver;
using Common;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AudioVisualizer
{
    public partial class MainWindow : Form
    {
        private readonly AnalyzerView analyzerView;
        private readonly ConfigWindow configWindow;

        private bool isBackDesktop = true;

        public MainWindow()
        {
            InitializeComponent();
            analyzerView = new AnalyzerView(pbMain);
            configWindow = new ConfigWindow(analyzerView);

            ShowInTaskbar = false;
            notifyIconMin.Visible = true;

            isBackDesktop = false;
            //Common.WindowUtils.ShowBehindDesktop(this.Handle);
            //this.Location = new System.Drawing.Point(0, 0);
            //FormBorderStyle = FormBorderStyle.None;
            //this.Size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            //pbMain.Margin = new System.Windows.Forms.Padding(0, 0, 0, 40);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                return;
            }

            analyzerView?.OnResize();

            var json = JsonConvert.SerializeObject(this.Size, Formatting.Indented);
            Saver.Save(Saver.DataType.WindowSize, json);
        }

        private void NotifyIconMin_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TopMost = true;
                WindowState = FormWindowState.Normal;
                TopMost = miOverAll.Checked;
            }
        }

        private void MiOverAll_Click(object sender, EventArgs e)
        {
            miOverAll.Checked = !miOverAll.Checked;
            TopMost = miOverAll.Checked;
        }

        private void MiOptions_Click(object sender, EventArgs e)
        {
            configWindow.Show();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                var json = Saver.Load(Saver.DataType.WindowSize);
                var config = JsonConvert.DeserializeObject<System.Drawing.Size>(json);
                this.Size = config;
            }
            catch (Exception)
            {

            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isBackDesktop)
            {
                WindowUtils.RestartExplorer();
            }
        }

        private void MiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
