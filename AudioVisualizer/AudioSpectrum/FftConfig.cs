using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AudioVisualizer.AudioSpectrum
{
    public class FftConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum FftDataSizes
        {
            FFT_1024,
            FFT_2048,
            FFT_4096,
            FFT_8192,
            FFT_16384,
        }

        private static readonly FftConfig FftConfig1024 = new FftConfig
        {
            FftDataSize = FftDataSizes.FFT_1024,
            DataSize = 1024,
            DataPow2 = 10,
            BASSData = Un4seen.Bass.BASSData.BASS_DATA_FFT1024
        };

        private static readonly FftConfig FftConfig2048 = new FftConfig
        {
            FftDataSize = FftDataSizes.FFT_2048,
            DataSize = 2048,
            DataPow2 = 11,
            BASSData = Un4seen.Bass.BASSData.BASS_DATA_FFT2048
        };

        private static readonly FftConfig FftConfig4096 = new FftConfig
        {
            FftDataSize = FftDataSizes.FFT_4096,
            DataSize = 4096,
            DataPow2 = 12,
            BASSData = Un4seen.Bass.BASSData.BASS_DATA_FFT4096
        };

        private static readonly FftConfig FftConfig8192 = new FftConfig
        {
            FftDataSize = FftDataSizes.FFT_8192,
            DataSize = 8192,
            DataPow2 = 13,
            BASSData = Un4seen.Bass.BASSData.BASS_DATA_FFT8192
        };

        private static readonly FftConfig FftConfig16384 = new FftConfig
        {
            FftDataSize = FftDataSizes.FFT_16384,
            DataSize = 16384,
            DataPow2 = 14,
            BASSData = Un4seen.Bass.BASSData.BASS_DATA_FFT16384
        };

        public FftDataSizes FftDataSize { get; private set; }

        public int DataSize { get; private set; }

        public int DataPow2 { get; private set; }

        public Un4seen.Bass.BASSData BASSData { get; private set; }


        private FftConfig()
        {

        }

        public static FftConfig GetConfig(FftDataSizes fftDataSizes)
        {
            switch (fftDataSizes)
            {
                default:
                case FftDataSizes.FFT_1024:
                    return FftConfig1024;
                case FftDataSizes.FFT_2048:
                    return FftConfig2048;
                case FftDataSizes.FFT_4096:
                    return FftConfig4096;
                case FftDataSizes.FFT_8192:
                    return FftConfig8192;
                case FftDataSizes.FFT_16384:
                    return FftConfig16384;
            }
        }

    }

}
