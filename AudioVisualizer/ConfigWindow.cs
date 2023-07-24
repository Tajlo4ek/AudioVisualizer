using AudioVisualizer.AudioSpectrum;
using System;
using System.Windows.Forms;

namespace AudioVisualizer
{
    public partial class ConfigWindow : Form
    {
        public AnalyzerVisualConfig VisualConfig { get; private set; }
        public AnalyzerDataConfig DataConfig { get; private set; }

        public ConfigWindow()
        {
            InitializeComponent();
            LoadConfig();

            nudColSpace.ValueChanged += ConfigColumnNud_ValueChanged;
            nudColCount.ValueChanged += ConfigColumnNud_ValueChanged;
            nudRectHeight.ValueChanged += ConfigRectNud_ValueChanged;
            nudRectSpace.ValueChanged += ConfigRectNud_ValueChanged;

            cbVisual.SelectedValueChanged += CbVisual_SelectedValueChanged;
            cbFftSize.SelectedValueChanged += CbFftSize_SelectedValueChanged;

            tbGain.ValueChanged += TbGain_ValueChanged;

            cbOnDesktop.CheckedChanged += CbOnDesktop_CheckedChanged;
        }

        private void ParseDataConfig()
        {
            if (DataConfig != null)
            {
                cbDevice.Items.Clear();
                foreach (var device in Analyzer.GetDeviceList())
                {
                    cbDevice.Items.Add(device);
                }

                cbDevice.SelectedItem = DataConfig.ActiveDeviceName;


                foreach (var type in Enum.GetValues(typeof(FftConfig.FftDataSizes)))
                {
                    cbFftSize.Items.Add(type.ToString());
                }
                cbFftSize.SelectedItem = DataConfig.FftDataSize.ToString();

            }

        }

        private void ParseVisualConfig()
        {
            nudColSpace.Value = VisualConfig.ColSpace;
            nudColCount.Value = VisualConfig.ColWidth;
            nudRectSpace.Value = VisualConfig.RectSpace;
            nudRectHeight.Value = VisualConfig.RectHeight;
            cbOnDesktop.Checked = VisualConfig.OnDesktop;

            foreach (var type in Enum.GetValues(typeof(AnalyzerVisualConfig.VisualStyle)))
            {
                cbVisual.Items.Add(type.ToString());
            }
            cbVisual.SelectedItem = VisualConfig.Style.ToString();

            tbGain.Value = (int)(VisualConfig.Gain * 10);
        }

        private void ConfigColumnNud_ValueChanged(object sender, EventArgs e)
        {
            VisualConfig.SetColumnConfig((int)nudColSpace.Value, (int)nudColCount.Value);
            SaveConfig();
        }

        private void ConfigRectNud_ValueChanged(object sender, EventArgs e)
        {
            VisualConfig.SetRectConfig((int)nudRectSpace.Value, (int)nudRectHeight.Value);
            SaveConfig();
        }

        private void CbOnDesktop_CheckedChanged(object sender, EventArgs e)
        {
            VisualConfig.SetOnDesktop(cbOnDesktop.Checked);
            SaveConfig();
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            DataConfig.SetActiveDevise(cbDevice.SelectedItem.ToString());
            SaveConfig();
        }


        private void BtnColorH_Click(object sender, EventArgs e)
        {
            colorDialog.Color = VisualConfig.HighLevelColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                VisualConfig.HighLevelColor = colorDialog.Color;
                SaveConfig();
            }
        }

        private void BtnColorL_Click(object sender, EventArgs e)
        {
            colorDialog.Color = VisualConfig.LowLevelColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                VisualConfig.LowLevelColor = colorDialog.Color;
                SaveConfig();
            }
        }

        private void BtnColorBack_Click(object sender, EventArgs e)
        {
            colorDialog.Color = VisualConfig.BackgroundColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                VisualConfig.BackgroundColor = colorDialog.Color;
                SaveConfig();
            }
        }

        private void CbFftSize_SelectedValueChanged(object sender, EventArgs e)
        {
            var dataSize = (FftConfig.FftDataSizes)Enum.Parse(typeof(FftConfig.FftDataSizes), cbFftSize.SelectedItem.ToString());
            DataConfig.SetFftDataSize(dataSize);
            SaveConfig();
        }

        private void CbVisual_SelectedValueChanged(object sender, EventArgs e)
        {
            var style = (AnalyzerVisualConfig.VisualStyle)Enum.Parse(typeof(AnalyzerVisualConfig.VisualStyle), cbVisual.SelectedItem.ToString());
            VisualConfig.SetStyle(style);
            SaveConfig();
        }

        private void ConfigWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            SaveConfig();
        }

        private void SaveConfig()
        {
            DataSaver.Saver.Save(DataSaver.Saver.DataType.VisualConfig, VisualConfig.GetSaveJson());
            DataSaver.Saver.Save(DataSaver.Saver.DataType.DataConfig, DataConfig.GetSaveJson());
        }

        private void LoadConfig()
        {
            var json = DataSaver.Saver.Load(DataSaver.Saver.DataType.VisualConfig);
            VisualConfig = AnalyzerVisualConfig.Parse(json);
            ParseVisualConfig();

            json = DataSaver.Saver.Load(DataSaver.Saver.DataType.DataConfig);
            DataConfig = AnalyzerDataConfig.Parse(json);
            ParseDataConfig();
        }

        private void TbGain_ValueChanged(object sender, EventArgs e)
        {
            VisualConfig.SetGain((float)tbGain.Value / 10);
            SaveConfig();
        }
    }
}
