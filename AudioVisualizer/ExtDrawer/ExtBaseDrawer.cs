using System;
using System.Text;
using System.Threading;
using AudioVisualizer.AudioSpectrum;

namespace AudioVisualizer.ExtDrawer
{
    public abstract class ExtBaseDrawer : IDisposable
    {
        protected const char StopChar = '|';
        protected const char SplitChar = ';';
        protected readonly string SetLineCountCommand = "line";
        protected readonly string SetMaxDataCommand = "maxData";
        protected readonly string SendSpectrumCommand = "sendSpectrum";

        protected string url;

        private int lineCount;

        protected float Scale { get; private set; }

        private float gain;

        private Spectrum leftSpectrum;
        private Spectrum rightSpectrum;

        private readonly FftConfig fftConfig = FftConfig.GetConfig(FftConfig.FftDataSizes.FFT_8192);

        private readonly Timer timer;

        // take more the 5hz, low then 20kHz, index in spectrum
        protected int startAt;
        protected int finishAt;

        protected bool haveError;

        public ExtBaseDrawer(string url)
        {
            timer = new Timer(Timer_Tick, null, 100, 25);
            gain = 1;

            leftSpectrum = new Spectrum();
            rightSpectrum = new Spectrum();

            this.url = url;
            Scale = -1;
        }

        public void SetGain(float val)
        {
            this.gain = val;
        }

        private void Timer_Tick(object state)
        {
            if (haveError == false)
            {
                Analyzer.GetSpectrum(lineCount, gain, fftConfig, ref leftSpectrum, ref rightSpectrum);
                if (lineCount > 0 && Scale > 0)
                {
                    SendData(GetDataString());
                }
            }
        }

        private string GetDataString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SendSpectrumCommand);

            var specData = leftSpectrum.GetCurrentData();
            if (specData.Length < finishAt)
            {
                specData = new float[finishAt + 1];
            }

            for (int i = startAt; i < finishAt; i++)
            {
                sb.Append((int)Math.Round(specData[i] * Scale));
                sb.Append(SplitChar);
            }

            specData = rightSpectrum.GetCurrentData();
            if (specData.Length < finishAt)
            {
                specData = new float[finishAt + 1];
            }

            for (int i = startAt; i < finishAt; i++)
            {
                sb.Append((int)Math.Round(specData[i] * Scale));
                sb.Append(SplitChar);
            }
            sb.Append(StopChar.ToString());

            return sb.ToString();
        }

        protected void AnalyseData(String data)
        {
            Console.WriteLine(data);

            if (data.StartsWith(SetLineCountCommand))
            {
                var str = data.Replace(SetLineCountCommand, "");
                SetLineCount(int.Parse(str));
            }
            else if (data.StartsWith(SetMaxDataCommand))
            {
                var str = data.Replace(SetMaxDataCommand, "");
                SetScale(int.Parse(str));
            }
        }

        public abstract void SendData(String data);

        private void SetLineCount(int needCount)
        {
            Analyzer.TestCountLine(needCount, fftConfig.FftDataSize, out int realCountLine, out startAt, out finishAt);
            lineCount = realCountLine;
        }

        private void SetScale(int maxLen)
        {
            Scale = maxLen / 100.0f;
        }

        public virtual void Dispose()
        {
            timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            timer.Dispose();
        }

    }

}
