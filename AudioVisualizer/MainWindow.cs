﻿using AudioVisualizer.AudioSpectrum;
using AudioVisualizer.DataSaver;
using Newtonsoft.Json;
using System;
using System.Drawing;
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

            ShowInTaskbar = false;
            notifyIconMin.Visible = true;

            //Common.WindowUtils.ShowBehindDesktop(this.Handle);
            //this.Location = new System.Drawing.Point(0, 0);
            //FormBorderStyle = FormBorderStyle.None;
            //this.Size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                return;
            }

            analyzerView.OnResize();

            var json = JsonConvert.SerializeObject(this.Size, Formatting.Indented);
            Saver.Save(Saver.DataType.WindowSize, json);
        }

        private void NotifyIconMin_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WindowState = FormWindowState.Normal;
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


    }
}
