using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace AudioVisualizer.ExtDrawer
{
    public class ExtDrawerDataConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ExtDrawerType
        {
            None,
            COM,
        };

        [JsonIgnore]
        private Action onEdit;

        [JsonProperty]
        public string Url { get; private set; }

        [JsonProperty]
        public bool AutoOn { get; private set; }

        [JsonProperty]
        public ExtDrawerType Type { get; private set; }


        public void SetData(string url, ExtDrawerType type)
        {
            this.Url = url;
            this.Type = type;
            onEdit?.Invoke();
        }

        public void SetAutoOn(bool val)
        {
            this.AutoOn = val;
        }

        public void AddOnEdit(Action onEdit)
        {
            this.onEdit += onEdit;
        }

        public string GetSaveJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public void LoadDefault()
        {
            Url = "";
            AutoOn = false;
            Type = ExtDrawerType.None;
            onEdit?.Invoke();
        }

        public static ExtDrawerDataConfig Parse(string json)
        {
            ExtDrawerDataConfig config;
            try
            {
                config = JsonConvert.DeserializeObject<ExtDrawerDataConfig>(json);
            }
            catch (Exception)
            {
                config = null;
            }

            if (config == null)
            {
                config = new ExtDrawerDataConfig();
                config.LoadDefault();
            }

            return config;
        }


    }
}
