using System.Drawing;

namespace AudioVisualizer.AudioSpectrum.Drawers
{
    public abstract class BaseDrawer
    {
        protected AnalyzerVisualConfig visualConfig;
        protected AnalyzerDataConfig dataConfig;

        public abstract AnalyzerVisualConfig.VisualStyle VisualStyle { get; }

        public Bitmap CurrentImage { get; protected set; }

        protected Graphics mainGraphics;

        public int LineCount { get; protected set; }


        public BaseDrawer()
        {
            CurrentImage = new Bitmap(1, 1);
            CurrentImage.SetPixel(0, 0, Color.Black);
        }

        public virtual void SetSize(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                CurrentImage = new Bitmap(size.Width, size.Height);
            }
            else
            {
                CurrentImage = new Bitmap(1, 1);
            }

            mainGraphics = Graphics.FromImage(CurrentImage);
            mainGraphics.Clear(visualConfig.BackgroundColor);
        }

        public abstract void CreateCurrentImage(Spectrum leftSpectrum, Spectrum rightSpectrum);

        public void SetDataConfig(AnalyzerDataConfig dataConfig)
        {
            this.dataConfig = dataConfig;
        }

        public void SetVisualConfig(AnalyzerVisualConfig visualConfig)
        {
            this.visualConfig = visualConfig;
        }

    }
}
