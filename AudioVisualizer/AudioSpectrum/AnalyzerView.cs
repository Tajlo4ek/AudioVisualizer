using AudioVisualizer.AudioSpectrum.Drawers;
using System;
using System.Windows.Forms;

namespace AudioVisualizer.AudioSpectrum
{
    public class AnalyzerView
    {
        private readonly PictureBox mainPictureBox;

        private Spectrum leftSpectrum;
        private Spectrum rightSpectrum;

        private readonly Timer timer;

        private BaseDrawer drawer;

        public AnalyzerVisualConfig VisualConfig { get; private set; }

        public AnalyzerDataConfig DataConfig { get; private set; }

        public AnalyzerView(AnalyzerDataConfig dataConfig, AnalyzerVisualConfig visualConfig, PictureBox pictureBox)
        {
            mainPictureBox = pictureBox;

            timer = new Timer();
            timer.Tick += Timer_tick;
            timer.Interval = 10;
            timer.Stop();

            leftSpectrum = new Spectrum();
            rightSpectrum = new Spectrum();

            VisualConfig = visualConfig;
            VisualConfig.AddOnEdit(OnVisualEdit);

            DataConfig = dataConfig;
            DataConfig.AddOnEdit(OnDataEdit);

            OnVisualEdit();
            OnDataEdit();

            timer.Start();
        }


        private void Timer_tick(object sender, EventArgs e)
        {
            if (!Analyzer.IsInit)
            {
                return;
            }

            Analyzer.GetSpectrum(drawer.LineCount, VisualConfig.Gain, FftConfig.GetConfig(DataConfig.FftDataSize), ref leftSpectrum, ref rightSpectrum);
            drawer.CreateCurrentImage(leftSpectrum, rightSpectrum);
            mainPictureBox.Image = drawer.CurrentImage;
        }

        public void OnResize()
        {
            drawer.SetSize(mainPictureBox.Size);
            mainPictureBox.Image = drawer.CurrentImage;
        }

        private void OnVisualEdit(bool force)
        {
            if (force || drawer == null || drawer.VisualStyle != VisualConfig.Style)
            {
                drawer = DrawerFactory.GetDrawer(VisualConfig.Style);
                drawer.SetDataConfig(DataConfig);
            }
            drawer.SetVisualConfig(VisualConfig);

            OnResize();
        }

        private void OnVisualEdit()
        {
            OnVisualEdit(false);
        }

        private void OnDataEdit()
        {
            if (Analyzer.ActiveDeviceName != DataConfig.ActiveDeviceName)
            {
                Analyzer.Start(DataConfig.ActiveDeviceName);
            }
            OnResize();
        }

    }
}