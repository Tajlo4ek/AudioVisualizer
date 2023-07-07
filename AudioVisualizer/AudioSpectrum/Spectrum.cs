using System;
using System.Collections;

namespace AudioVisualizer.AudioSpectrum
{
    public class Spectrum
    {
        float[] currentData;
        float[] maxData;

        public int DataCount { get { return currentData.Length; } }
        public float TotalLoad { get; private set; }

        public float MoveDownSpeed { get; set; }


        public Spectrum()
        {
            currentData = new float[0];
            maxData = new float[0];
            MoveDownSpeed = 0.75f;
        }

        public void SetData(float[] values, float totalLoad)
        {
            if (currentData.Length != values.Length)
            {
                currentData = new float[values.Length];
                maxData = new float[values.Length];
            }

            for (int ind = 0; ind < values.Length; ind++)
            {
                var next = maxData[ind] - MoveDownSpeed;
                maxData[ind] = next > 0 ? next : 0;

                var cur = values[ind];
                if (cur > maxData[ind])
                {
                    maxData[ind] = cur;
                }

                if (cur >= currentData[ind])
                {
                    currentData[ind] = cur;
                }
                else
                {
                    var delta = currentData[ind] - cur;
                    currentData[ind] -= delta / 2;
                }
            }

            TotalLoad = totalLoad;
        }

        public float[] GetCurrentData()
        {
            return currentData;
        }

        public float[] GetMaxData()
        {
            return maxData;
        }
    }

}
