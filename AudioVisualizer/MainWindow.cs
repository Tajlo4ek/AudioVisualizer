using AudioVisualizer.AudioSpectrum;
using AudioVisualizer.DataSaver;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace AudioVisualizer
{
    public partial class MainWindow : Form
    {
        private readonly AnalyzerView analyzerView;
        private readonly ConfigWindow configWindow;

        public MainWindow()
        {
            InitializeComponent();
            analyzerView = new AnalyzerView(pbMain);
            configWindow = new ConfigWindow(analyzerView);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIconMin.Visible = true;
                return;
            }

            analyzerView.OnResize();

            var json = JsonConvert.SerializeObject(this.Size, Formatting.Indented);
            Saver.Save(Saver.DataType.WindowSize, json);
        }

        private void NotifyIconMin_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            notifyIconMin.Visible = false;
            WindowState = FormWindowState.Normal;
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


    }
}
