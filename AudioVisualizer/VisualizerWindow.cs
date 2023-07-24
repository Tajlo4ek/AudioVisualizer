using AudioVisualizer.AudioSpectrum;
using AudioVisualizer.DataSaver;
using Common;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AudioVisualizer
{
    public partial class VisualizerWindow : Form
    {
        private readonly AnalyzerView analyzerView;
        private readonly ConfigWindow configWindow;

        readonly Dispatcher uiThread;

        private bool onDesktop;

        public VisualizerWindow()
        {
            InitializeComponent();

            configWindow = new ConfigWindow();
            analyzerView = new AnalyzerView(configWindow.DataConfig, configWindow.VisualConfig, pbMain);

            configWindow.VisualConfig.AddOnEdit(OnVisualConfigEdit);
            uiThread = Dispatcher.CurrentDispatcher;

            new Thread(() =>
            {
                Thread.Sleep(5000);
                uiThread.Invoke(() => {
                    SetOnDesktop(configWindow.VisualConfig.OnDesktop);
                });               
            }).Start();



            ShowInTaskbar = false;
            notifyIconMin.Visible = true;
        }

        private void OnVisualConfigEdit()
        {
            if (onDesktop != configWindow.VisualConfig.OnDesktop)
            {
                SetOnDesktop(configWindow.VisualConfig.OnDesktop);
            }
        }

        private void SetOnDesktop(bool val)
        {
            if (val)
            {
                WindowUtils.ShowBehindDesktop(this.Handle);
                this.Location = new System.Drawing.Point(0, 0);
                FormBorderStyle = FormBorderStyle.None;
                this.Size = Screen.PrimaryScreen.Bounds.Size;
                pbMain.Margin = new Padding(0, 0, 0, 40);
            }
            else
            {
                if (onDesktop == true)
                {
                    
                }

                FormBorderStyle = FormBorderStyle.Sizable;
                pbMain.Margin = new Padding(0, 0, 0, 0);
            }

            onDesktop = val;
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
            WindowUtils.RestartExplorer();
        }
        
        private void MiExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
