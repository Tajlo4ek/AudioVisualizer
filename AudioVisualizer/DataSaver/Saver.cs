using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioVisualizer.DataSaver
{
    public static class Saver
    {
        public enum DataType
        {
            VisualConfig,
            DataConfig,
            WindowSize,
        }

        private static readonly Dictionary<DataType, string> saveFileName = new Dictionary<DataType, string>
        {
            {DataType.DataConfig, "config.json" },
            {DataType.VisualConfig, "visual.json" },
            {DataType.WindowSize, "size.json" },
        };

        static readonly string saveFolder = Application.StartupPath + "\\saves\\";

        public static bool Save(DataType type, string data)
        {
            try
            {
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                var path = saveFolder + saveFileName[type];

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(data);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static string Load(DataType type)
        {
            var path = saveFolder + saveFileName[type];

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }

            return "";
        }

    }


}
