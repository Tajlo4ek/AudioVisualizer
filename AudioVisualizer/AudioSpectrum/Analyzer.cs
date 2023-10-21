using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace AudioVisualizer.AudioSpectrum
{
    public static class Analyzer
    {
        private static readonly WASAPIPROC apiProcess = new WASAPIPROC(Process);

        private const int DeviceFreq = 44100;

        private const int minFreq = 20;
        private const int maxFreq = 20000;

        public static bool IsInit { get; private set; } = false;

        private static int ActiveDeviceId;

        public static string ActiveDeviceName
        {
            get
            {
                if (IsInit)
                {
                    return deviceList[ActiveDeviceId];
                }
                else
                {
                    return "";
                }
            }
        }

        private static List<string> deviceList;
        private static readonly object locker = new object();

        public static bool Start(string deviceName)
        {
            if (IsInit == false)
            {
                IsInit = true;

                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, false);
                var isInit = Bass.BASS_Init(0, DeviceFreq, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                RefreshDeviceList();

                if (isInit == false)
                {
                    IsInit = false;
                    return false;
                }
            }

            lock (locker)
            {
                ActiveDeviceId = deviceList.IndexOf(deviceName);

                if (ActiveDeviceId != -1)
                {
                    var array = deviceList[ActiveDeviceId].Split(' ');
                    var devindex = Convert.ToInt32(array[0]);

                    bool result = BassWasapi.BASS_WASAPI_Init(devindex, 0, 0, BASSWASAPIInit.BASS_WASAPI_BUFFER, 1f, 0.05f, apiProcess, IntPtr.Zero);
                    if (!result)
                    {
                        IsInit = false;
                    }
                    else
                    {
                        BassWasapi.BASS_WASAPI_Start();
                        IsInit = true;
                    }
                }
                else
                {
                    IsInit = false;
                    return false;
                }
            }

            return true;
        }

        public static ReadOnlyCollection<string> GetDeviceList()
        {
            lock (locker)
            {
                if (deviceList == null)
                {
                    deviceList = new List<string>();

                    var countDevice = BassWasapi.BASS_WASAPI_GetDeviceCount();

                    for (int i = 0; i < countDevice; i++)
                    {
                        var device = BassWasapi.BASS_WASAPI_GetDeviceInfo(i);
                        if (device.IsEnabled && device.IsLoopback)
                        {
                            string deviceName = System.Text.RegularExpressions.Regex.Replace(device.name, @"[^\u0020-\u007E]", string.Empty);
                            deviceList.Add(string.Format("{0} - {1}", i, deviceName));
                        }
                    }
                }

                return deviceList.AsReadOnly();
            }
        }

        public static void RefreshDeviceList()
        {
            lock (locker)
            {
                deviceList = null;
                GetDeviceList();
            }
        }

        public static bool GetSpectrum(int colCount, float gain, FftConfig fftConfig, ref Spectrum left, ref Spectrum right)
        {
            float[] _fft = new float[fftConfig.DataSize * 2];
            int ret = BassWasapi.BASS_WASAPI_GetData(_fft, (int)(fftConfig.BASSData | BASSData.BASS_DATA_FFT_INDIVIDUAL));

            float[] leftFft = new float[_fft.Length / 2];
            float[] rightFft = new float[_fft.Length / 2];

            for (int i = 0; i < _fft.Length / 2; i++)
            {
                var ind = i * 2;
                leftFft[i] = _fft[ind];
                rightFft[i] = _fft[ind + 1];
            }

            if (ret < 0)
            {
                Console.WriteLine(Bass.BASS_ErrorGetCode());
                return false;
            }

            int level = BassWasapi.BASS_WASAPI_GetLevel();

            left.SetData(GetSpectrum(leftFft, colCount, gain, fftConfig), (float)Un4seen.Bass.Utils.LowWord32(level) / Int16.MaxValue * gain * 100);
            right.SetData(GetSpectrum(rightFft, colCount, gain, fftConfig), (float)Un4seen.Bass.Utils.HighWord32(level) / Int16.MaxValue * gain * 100);

            return true;
        }

        private static float[] GetSpectrum(float[] fft, int colCount, float gain, FftConfig fftConfig)
        {
            var res = new float[colCount];

            int b0 = 0;

            for (int x = 0; x < colCount; x++)
            {
                float peak = 0;
                int b1 = (int)Math.Pow(2, (float)x * fftConfig.DataPow2 / (colCount - 1));

                if (b1 > fftConfig.DataSize - 1)
                {
                    b1 = fftConfig.DataSize - 1;
                }
                if (b1 <= b0)
                {
                    b1 = b0 + 1;
                }

                for (; b0 < b1; b0++)
                {
                    if (peak < fft[1 + b0])
                    {
                        peak = fft[1 + b0];
                    }
                }

                if (peak > 1)
                {
                    peak = 1;
                }

                int y = (int)(Math.Sqrt(peak) * gain * 100);

                if (y > 100) y = 100;
                if (y < 0) y = 0;

                res[x] = y;
            }
            return res;
        }

        private static int Process(IntPtr buffer, int length, IntPtr user)
        {
            return length;
        }


        private static void TestCountLine(int lineCount, FftConfig fftConfig, out int indMin, out int indMax)
        {
            float lineFreq = (float)DeviceFreq / fftConfig.DataSize;

            indMin = 0;
            indMax = lineCount;
            int b1Prev = 0;

            var b0 = 0;

            for (int x = 0; x < lineCount; x++)
            {
                var b1 = (int)Math.Round(Math.Pow(2, (float)x * fftConfig.DataPow2 / (lineCount - 1)));

                if (b1 > fftConfig.DataSize - 1)
                {
                    b1 = fftConfig.DataSize - 1;
                }
                if (b1 <= b0)
                {
                    b1 = b0 + 1;
                }

                for (; b0 < b1; b0++)
                {
                }

                if (b1 * lineFreq < minFreq)
                {
                    indMin = x;
                }

                if (b1Prev * lineFreq < maxFreq)
                {
                    indMax = x;
                }

                b1Prev = b1;
            }

            indMax++;
        }

        public static void TestCountLine(int needCount, FftConfig.FftDataSizes dataSize, out int realCount, out int indMin, out int indMax)
        {
            var fftConfig = FftConfig.GetConfig(dataSize);

            realCount = needCount;
            TestCountLine(realCount, fftConfig, out indMin, out indMax);
            while (indMax - indMin < needCount)
            {
                realCount++;
                TestCountLine(realCount, fftConfig, out indMin, out indMax);
            }
        }


        public static int GetIndexFreq(int lineCount, FftConfig.FftDataSizes dataSize, int needFreq)
        {
            var fftConfig = FftConfig.GetConfig(dataSize);
            float lineFreq = (float)DeviceFreq / fftConfig.DataSize;

            var b0 = 0;

            for (int x = 0; x < lineCount; x++)
            {
                var b1 = (int)Math.Round(Math.Pow(2, (float)x * fftConfig.DataPow2 / (lineCount - 1)));

                if (b1 > fftConfig.DataSize - 1)
                {
                    b1 = fftConfig.DataSize - 1;
                }
                if (b1 <= b0)
                {
                    b1 = b0 + 1;
                }

                for (; b0 < b1; b0++)
                {
                }

                if (b1 * lineFreq > needFreq)
                {
                    return x > 0 ? x - 1 : 0;
                }
            }

            return lineCount - 1;
        }


    }
}