using AudioVisualizer.AudioSpectrum.Drawers;
using System;
using System.Windows.Forms;
using AudioVisualizer.ExtDrawer;

namespace AudioVisualizer.AudioSpectrum
{
    public class AnalyzerView
    {
        private readonly PictureBox mainPictureBox;

        private Spectrum leftSpectrum;
        private Spectrum rightSpectrum;

        private readonly Timer timer;

        private BaseDrawer drawer;
        private ExtBaseDrawer extDrawer;

        public AnalyzerVisualConfig VisualConfig { get; private set; }

        public AnalyzerDataConfig DataConfig { get; private set; }

        public ExtDrawerDataConfig ExtConfig { get; private set; }

        public AnalyzerView(PictureBox pictureBox)
        {
            mainPictureBox = pictureBox;

            timer = new Timer();
            timer.Tick += Timer_tick;
            timer.Interval = 25;
            timer.Stop();

            leftSpectrum = new Spectrum();
            rightSpectrum = new Spectrum();

            VisualConfig = new AnalyzerVisualConfig();
            VisualConfig.LoadDefault();
            VisualConfig.AddOnEdit(OnVisualEdit);

            DataConfig = new AnalyzerDataConfig();
            DataConfig.LoadDefault();
            DataConfig.AddOnEdit(OnDataEdit);

            ExtConfig = new ExtDrawerDataConfig();
            ExtConfig.LoadDefault();
            ExtConfig.AddOnEdit(OnExtEdit);

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

        public void SetVisualConfig(AnalyzerVisualConfig config)
        {
            this.VisualConfig = config;
            config.AddOnEdit(OnVisualEdit);
            drawer.SetVisualConfig(config);
            OnVisualEdit(true);
        }

        public void SetDataConfig(AnalyzerDataConfig config)
        {
            this.DataConfig = config;
            config.AddOnEdit(OnDataEdit);
            drawer.SetDataConfig(config);
            OnDataEdit();
        }

        public void SetExtDataConfig(ExtDrawerDataConfig config)
        {
            this.ExtConfig = config;
            config.AddOnEdit(OnExtEdit);
            OnExtEdit();
        }


        public void OnResize()
        {
            drawer.SetSize(mainPictureBox.Size);
            mainPictureBox.Image = drawer.CurrentImage;
        }

        private void OnVisualEdit(bool force)
        {
            extDrawer?.SetGain(VisualConfig.Gain);

            if (force || drawer == null || drawer.VisualStyle != VisualConfig.Style)
            {
                drawer = DrawerFactory.GetDrawer(VisualConfig.Style);
                drawer.SetDataConfig(DataConfig);
            }
            drawer.SetVisualConfig(VisualConfig);
            timer.Interval = VisualConfig.UpdateMSec;

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

        private void OnExtEdit()
        {

            extDrawer?.Dispose();
            return;
            /*try
            {
                switch (ExtConfig.Type)
                {
                    case ExtDrawerDataConfig.ExtDrawerType.COM:
                        extDrawer = new ComExtDrawer(ExtConfig.Url);
                        break;
                    case ExtDrawerDataConfig.ExtDrawerType.None:
                        extDrawer = null;
                        break;
                }
            }
            finally
            {
                extDrawer?.SetGain(VisualConfig.Gain);
            }*/

        }

    }
}