using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Drawing;

namespace AudioVisualizer.AudioSpectrum
{
    public class AnalyzerVisualConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum VisualStyle
        {
            Rectangle,
            Circle,
        }

        [JsonIgnore]
        private Action onEdit;

        [JsonIgnore]
        public SolidBrush LowBrush { get; private set; }

        [JsonIgnore]
        public SolidBrush MediumBrush { get; private set; }

        [JsonIgnore]
        public SolidBrush HighBrush { get; private set; }

        [JsonIgnore]
        public SolidBrush MaxBrush { get; private set; }

        [JsonIgnore]
        public SolidBrush BackgroundBrush { get; private set; }

        [JsonIgnore]
        public Pen MaxPen { get; private set; }


        [JsonProperty]
        public VisualStyle Style { get; private set; }

        [JsonProperty]
        public int ColSpace { get; private set; }

        [JsonProperty]
        public int ColWidth { get; private set; }

        [JsonProperty]
        public int RectHeight { get; private set; }

        [JsonProperty]
        public int RectSpace { get; private set; }

        [JsonProperty]
        public float Gain { get; private set; }


        [JsonProperty]
        public Color BackgroundColor
        {
            get
            {
                return BackgroundBrush.Color;
            }
            set
            {
                BackgroundBrush.Color = value;
            }
        }

        [JsonProperty]
        public Color LowLevelColor
        {
            get
            {
                return LowBrush.Color;
            }
            set
            {
                LowBrush.Color = value;
            }
        }

        [JsonProperty]
        public Color MediumLevelColor
        {
            get
            {
                return MediumBrush.Color;
            }
            set
            {
                MediumBrush.Color = value;
            }
        }

        [JsonProperty]
        public Color HighLevelColor
        {
            get
            {
                return HighBrush.Color;
            }
            set
            {
                HighBrush.Color = value;
            }
        }

        [JsonProperty]
        public Color MaxLevelColor
        {
            get
            {
                return MaxBrush.Color;
            }
            set
            {
                MaxBrush.Color = value;
                MaxPen.Color = value;
            }
        }


        public AnalyzerVisualConfig()
        {
            LowBrush = new SolidBrush(Color.Empty);
            MediumBrush = new SolidBrush(Color.Empty);
            HighBrush = new SolidBrush(Color.Empty);
            MaxBrush = new SolidBrush(Color.Empty);
            BackgroundBrush = new SolidBrush(Color.Empty);

            MaxPen = new Pen(Color.Empty);
        }

        public void AddOnEdit(Action onEdit)
        {
            this.onEdit += onEdit;
        }

        public void LoadDefault()
        {
            SetColumnConfig(2, 6);
            SetRectConfig(1, 1);

            LowLevelColor = Color.Green;
            MediumLevelColor = Color.Yellow;
            HighLevelColor = Color.Red;
            MaxLevelColor = Color.FromArgb(255, 0, 255, 255);
            Gain = 1;

            BackgroundColor = Color.Black;

            Style = VisualStyle.Rectangle;
        }

        public void SetColumnConfig(int colSpace, int colWidth)
        {
            this.ColSpace = colSpace;
            this.ColWidth = colWidth;

            onEdit?.Invoke();
        }

        public void SetRectConfig(int rectSpace, int rectHeight)
        {
            this.RectSpace = rectSpace;
            this.RectHeight = rectHeight;

            onEdit?.Invoke();
        }

        public void SetStyle(VisualStyle style)
        {
            Style = style;
            onEdit?.Invoke();
        }

        public void SetGain(float gain)
        {
            if (gain < 1)
            {
                gain = 1;
            }

            this.Gain = gain;
            onEdit?.Invoke();
        }

        public string GetSaveJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static AnalyzerVisualConfig Parse(string json)
        {
            AnalyzerVisualConfig config;
            try
            {
                config = JsonConvert.DeserializeObject<AnalyzerVisualConfig>(json);
            }
            catch (Exception)
            {
                config = null;
            }

            if (config == null)
            {
                config = new AnalyzerVisualConfig();
                config.LoadDefault();
            }

            return config;
        }

    }
}
