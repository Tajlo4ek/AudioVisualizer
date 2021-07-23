using System;
using System.Drawing;

namespace AudioVisualizer.AudioSpectrum.Drawers
{
    sealed class CircleDrawer : BaseDrawer
    {
        private int startAt;
        private int finishAt;

        private float radiusIn;
        private float radiusOut;

        public override AnalyzerVisualConfig.VisualStyle VisualStyle { get { return AnalyzerVisualConfig.VisualStyle.Circle; } }

        public override void SetSize(Size size)
        {
            base.SetSize(size);

            radiusOut = Math.Min(CurrentImage.Width, CurrentImage.Height) / 2;
            radiusIn = radiusOut / 2;

            int needCount = (int)Math.Round(Math.PI * radiusIn / 3);
            Analyzer.TestCountLine(needCount, dataConfig.FftDataSize, out int realCountLine, out startAt, out finishAt);
            LineCount = realCountLine;
        }

        public override void CreateCurrentImage(Spectrum leftSpectrum, Spectrum rightSpectrum)
        {
            mainGraphics.Clear(visualConfig.BackgroundColor);
            mainGraphics.DrawEllipse(visualConfig.MaxPen, CurrentImage.Width / 2 - radiusIn, CurrentImage.Height / 2 - radiusIn, radiusIn * 2, radiusIn * 2);

            Draw(leftSpectrum, rightSpectrum);
        }

        public void Draw(Spectrum leftSpectrum, Spectrum rightSpectrum)
        {
            if (finishAt - startAt >= leftSpectrum.DataCount) { return; }

            var left = new float[finishAt - startAt];
            Array.Copy(leftSpectrum.GetCurrentData(), startAt, left, 0, finishAt - startAt);

            var leftMax = new float[finishAt - startAt];
            Array.Copy(leftSpectrum.GetMaxData(), startAt, leftMax, 0, finishAt - startAt);

            var right = new float[finishAt - startAt];
            Array.Copy(rightSpectrum.GetCurrentData(), startAt, right, 0, finishAt - startAt);

            var rightMax = new float[finishAt - startAt];
            Array.Copy(rightSpectrum.GetMaxData(), startAt, rightMax, 0, finishAt - startAt);

            DrawSpectrum(left, leftMax, false);
            DrawSpectrum(right, rightMax, true);


        }

        public void DrawSpectrum(float[] spectrumCurData, float[] spectrumMaxData, bool isReverse)
        {
            float moveAngle = 10;

            float alpha = isReverse ? -90 + moveAngle : -90 - moveAngle;
            float dAlpha = (isReverse ? 1 : -1) * (180 -  moveAngle) / (spectrumCurData.Length);

            var points = new PointF[spectrumCurData.Length + 2];

            GetPointOnCircle(alpha - dAlpha, radiusIn, out float x, out float y);
            points[0] = new PointF(x, y);

            for (int spectrumId = 0; spectrumId < spectrumCurData.Length; spectrumId++)
            {
                var spData = spectrumCurData[spectrumId] / 100f;

                GetPointOnCircle(alpha, radiusIn, out float x1, out float y1);
                GetPointOnCircle(alpha, (radiusOut - radiusIn) * spData + radiusIn, out float x2, out float y2);

                mainGraphics.DrawLine(Pens.Gray, x1, y1, x2, y2);

                var spMaxData = spectrumMaxData[spectrumId] / 100f;

                GetPointOnCircle(alpha, (radiusOut - radiusIn) * spMaxData + radiusIn, out float xMax, out float yMax);
                points[spectrumId + 1] = new PointF(xMax, yMax);

                alpha += dAlpha;
            }

            GetPointOnCircle(alpha, radiusIn, out x, out y);
            points[points.Length - 1] = new PointF(x, y);

            mainGraphics.DrawLines(visualConfig.MaxPen, points);
        }

        private void GetPointOnCircle(float angleDeg, float radius, out float x, out float y)
        {
            var radians = angleDeg * Math.PI / 180;
            x = (float)Math.Cos(radians) * radius + CurrentImage.Width / 2;
            y = (float)Math.Sin(radians) * radius + CurrentImage.Height / 2;
        }

    }
}
