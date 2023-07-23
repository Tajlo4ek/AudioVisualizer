using AudioVisualizer.AudioSpectrum;
using System;
using System.Windows.Forms;
using AudioVisualizer.ExtDrawer;

namespace AudioVisualizer
{
    public partial class ConfigWindow : Form
    {
        private readonly AnalyzerView analyzerView;
        private AnalyzerVisualConfig visualConfigurator;
        private AnalyzerDataConfig dataConfigurator;
        private ExtDrawerDataConfig extDrawerDataConfig;

        public ConfigWindow() : this(null)
        {
        }

        public ConfigWindow(AnalyzerView analyzerView)
        {
            InitializeComponent();

            if (analyzerView != null)
            {
                this.analyzerView = analyzerView;
                this.visualConfigurator = analyzerView.VisualConfig;
                this.dataConfigurator = analyzerView.DataConfig;
                this.extDrawerDataConfig = analyzerView.ExtConfig;

                LoadConfig();

                nudColSpace.ValueChanged += ConfigColumnNud_ValueChanged;
                nudColCount.ValueChanged += ConfigColumnNud_ValueChanged;
                nudRectHeight.ValueChanged += ConfigRectNud_ValueChanged;
                nudRectSpace.ValueChanged += ConfigRectNud_ValueChanged;

                cbVisual.SelectedValueChanged += CbVisual_SelectedValueChanged;
                cbFftSize.SelectedValueChanged += CbFftSize_SelectedValueChanged;

                tbGain.ValueChanged += TbGain_ValueChanged;
            }
        }        

        private void ParseDataConfig()
        {
            if (dataConfigurator != null)
            {
                cbDevice.Items.Clear();
                foreach (var device in Analyzer.GetDeviceList())
                {
                    cbDevice.Items.Add(device);
                }

                cbDevice.SelectedItem = dataConfigurator.ActiveDeviceName;


                foreach (var type in Enum.GetValues(typeof(FftConfig.FftDataSizes)))
                {
                    cbFftSize.Items.Add(type.ToString());
                }
                cbFftSize.SelectedItem = dataConfigurator.FftDataSize.ToString();

            }

        }

        private void ParseVisualConfig()
        {
            nudColSpace.Value = visualConfigurator.ColSpace;
            nudColCount.Value = visualConfigurator.ColWidth;
            nudRectSpace.Value = visualConfigurator.RectSpace;
            nudRectHeight.Value = visualConfigurator.RectHeight;

            foreach (var type in Enum.GetValues(typeof(AnalyzerVisualConfig.VisualStyle)))
            {
                cbVisual.Items.Add(type.ToString());
            }
            cbVisual.SelectedItem = visualConfigurator.Style.ToString();

            tbGain.Value = (int)(visualConfigurator.Gain * 10);
        }

        private void ParseExtDrawerData()
        {
            foreach (var type in Enum.GetValues(typeof(ExtDrawerDataConfig.ExtDrawerType)))
            {
                cbExtDrawerType.Items.Add(type.ToString());
            }
            cbExtDrawerType.SelectedItem = extDrawerDataConfig.Type.ToString();

            foreach (var port in ComExtDrawer.GetPorts())
            {
                cbComPorts.Items.Add(port);
            }
            cbComPorts.SelectedItem = extDrawerDataConfig.Url;
        }

        private void ConfigColumnNud_ValueChanged(object sender, EventArgs e)
        {
            visualConfigurator.SetColumnConfig((int)nudColSpace.Value, (int)nudColCount.Value);
            SaveConfig();
        }

        private void ConfigRectNud_ValueChanged(object sender, EventArgs e)
        {
            visualConfigurator.SetRectConfig((int)nudRectSpace.Value, (int)nudRectHeight.Value);
            SaveConfig();
        }


        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            dataConfigurator.SetActiveDevise(cbDevice.SelectedItem.ToString());
            SaveConfig();
        }


        private void BtnColorH_Click(object sender, EventArgs e)
        {
            colorDialog.Color = visualConfigurator.HighLevelColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                visualConfigurator.HighLevelColor = colorDialog.Color;
                SaveConfig();
            }
        }

        private void BtnColorL_Click(object sender, EventArgs e)
        {
            colorDialog.Color = visualConfigurator.LowLevelColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                visualConfigurator.LowLevelColor = colorDialog.Color;
                SaveConfig();
            }
        }

        private void BtnColorBack_Click(object sender, EventArgs e)
        {
            colorDialog.Color = visualConfigurator.BackgroundColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                visualConfigurator.BackgroundColor = colorDialog.Color;
                SaveConfig();
            }
        }


        private void CbFftSize_SelectedValueChanged(object sender, EventArgs e)
        {
            var dataSize = (FftConfig.FftDataSizes)Enum.Parse(typeof(FftConfig.FftDataSizes), cbFftSize.SelectedItem.ToString());
            dataConfigurator.SetFftDataSize(dataSize);
            SaveConfig();
        }

        private void CbVisual_SelectedValueChanged(object sender, EventArgs e)
        {
            var style = (AnalyzerVisualConfig.VisualStyle)Enum.Parse(typeof(AnalyzerVisualConfig.VisualStyle), cbVisual.SelectedItem.ToString());
            visualConfigurator.SetStyle(style);
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
            DataSaver.Saver.Save(DataSaver.Saver.DataType.VisualConfig, visualConfigurator.GetSaveJson());
            DataSaver.Saver.Save(DataSaver.Saver.DataType.DataConfig, dataConfigurator.GetSaveJson());
            DataSaver.Saver.Save(DataSaver.Saver.DataType.ExtConfig, extDrawerDataConfig.GetSaveJson());
        }

        private void LoadConfig()
        {
            var json = DataSaver.Saver.Load(DataSaver.Saver.DataType.VisualConfig);
            var configVisual = AnalyzerVisualConfig.Parse(json);
            analyzerView.SetVisualConfig(configVisual);
            visualConfigurator = analyzerView.VisualConfig;
            ParseVisualConfig();

            json = DataSaver.Saver.Load(DataSaver.Saver.DataType.DataConfig);
            var configData = AnalyzerDataConfig.Parse(json);
            analyzerView.SetDataConfig(configData);
            dataConfigurator = analyzerView.DataConfig;
            ParseDataConfig();

            json = DataSaver.Saver.Load(DataSaver.Saver.DataType.ExtConfig);
            var configExt = ExtDrawerDataConfig.Parse(json);
            analyzerView.SetExtDataConfig(configExt);
            extDrawerDataConfig = analyzerView.ExtConfig;
            ParseExtDrawerData();

            analyzerView.SetVisualConfig(visualConfigurator);
            analyzerView.SetDataConfig(dataConfigurator);
        }

        private void TbGain_ValueChanged(object sender, EventArgs e)
        {
            visualConfigurator.SetGain((float)tbGain.Value / 10);
            SaveConfig();
        }

        private void BtnExtDrawerSet_Click(object sender, EventArgs e)
        {
            var type = (ExtDrawerDataConfig.ExtDrawerType)Enum.Parse(typeof(ExtDrawerDataConfig.ExtDrawerType), cbExtDrawerType.SelectedItem.ToString());
            extDrawerDataConfig.SetData(cbComPorts.SelectedItem.ToString(), type);
            SaveConfig();
        }

        private void BtnExtDrawerStop_Click(object sender, EventArgs e)
        {
            cbExtDrawerType.SelectedItem = ExtDrawerDataConfig.ExtDrawerType.None.ToString();
            BtnExtDrawerSet_Click(null, null);
        }
    }
}
