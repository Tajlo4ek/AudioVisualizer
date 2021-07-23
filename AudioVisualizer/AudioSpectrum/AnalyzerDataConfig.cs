using Newtonsoft.Json;
using System;

namespace AudioVisualizer.AudioSpectrum
{
    public class AnalyzerDataConfig
    {
        [JsonIgnore]
        private Action onEdit;

        [JsonProperty]
        public string ActiveDeviceName { get; private set; }

        [JsonProperty]
        public FftConfig.FftDataSizes FftDataSize { get; private set; }

        public AnalyzerDataConfig()
        {
        }

        public void AddOnEdit(Action onEdit)
        {
            this.onEdit += onEdit;
        }

        public void SetFftDataSize(FftConfig.FftDataSizes fftDataSize)
        {
            this.FftDataSize = fftDataSize;
            onEdit?.Invoke();
        }

        public void SetActiveDevise(string name)
        {
            ActiveDeviceName = name;
            onEdit?.Invoke();
        }

        public string GetSaveJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public void LoadDefault()
        {
            ActiveDeviceName = "";
            FftDataSize = FftConfig.FftDataSizes.FFT_2048;
            onEdit?.Invoke();
        }

        public static AnalyzerDataConfig Parse(string json)
        {
            AnalyzerDataConfig config;
            try
            {
                config = JsonConvert.DeserializeObject<AnalyzerDataConfig>(json);
            }
            catch (Exception)
            {
                config = new AnalyzerDataConfig();
                config.LoadDefault();
            }

            return config;
        }

    }
}
