using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace AudioVisualizer.AudioSpectrum.Drawers
{
    sealed class CircleDrawer : BaseDrawer
    {
        private class Params
        {
            public float dAlpha;
            public float dAlphaDiv2;
            public float nowAngle;
            public float oneBlock;
        }

        private int startAt;
        private int finishAt;

        private float radiusIn;
        private float radiusOut;

        private const float offsetAngle = 10;

        public override AnalyzerVisualConfig.VisualStyle VisualStyle { get { return AnalyzerVisualConfig.VisualStyle.Circle; } }

        public override void SetSize(Size size)
        {
            base.SetSize(size);

            radiusOut = Math.Min(CurrentImage.Width, CurrentImage.Height) / 2;
            radiusIn = radiusOut / 2f;

            int needCount = (int)(Math.PI * radiusIn / 180 * (180 - offsetAngle) / (visualConfig.ColWidth + visualConfig.ColSpace));
            Analyzer.TestCountLine(needCount, dataConfig.FftDataSize, out int realCountLine, out startAt, out finishAt);
            LineCount = realCountLine;

            Console.WriteLine(needCount);
        }

        public override void CreateCurrentImage(Spectrum leftSpectrum, Spectrum rightSpectrum)
        {
            mainGraphics.Clear(visualConfig.BackgroundColor);
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

            var leftBlockCount = DrawSpectrum(left, false);
            var rightBlockCount = DrawSpectrum(right, true);

            DrawMaxSpectrum(leftMax, leftBlockCount, false);
            DrawMaxSpectrum(rightMax, rightBlockCount, true);

        }


        private Params CalcParams(int itemCount, bool isReverse)
        {
            Params @params = new Params
            {
                dAlpha = (isReverse ? 1 : -1) * (180 - offsetAngle) / (itemCount)
            };

            @params.dAlphaDiv2 = (@params.dAlpha / 2) / (1 + (float)visualConfig.ColSpace / visualConfig.ColWidth);
            @params.nowAngle = isReverse ? -90 + offsetAngle : -90 - offsetAngle;

            @params.oneBlock = visualConfig.RectHeight + visualConfig.RectSpace;

            return @params;
        }

        private void DrawMaxSpectrum(float[] spectrumMaxData, int[] blockCount, bool isReverse)
        {
            var param = CalcParams(spectrumMaxData.Length, isReverse);

            var poly = new PointF[4] { new PointF(), new PointF(), new PointF(), new PointF() };
            for (int spectrumId = 0; spectrumId < spectrumMaxData.Length; spectrumId++)
            {
                var maxValue = spectrumMaxData[spectrumId] / 100f * (radiusOut - radiusIn);
                var maxValueInBlock = blockCount[spectrumId] * param.oneBlock;

                if (maxValue < maxValueInBlock)
                {
                    maxValue = maxValueInBlock;
                }

                GetPointOnCircle(param.nowAngle - param.dAlphaDiv2, maxValue + radiusIn, ref poly[0]);
                GetPointOnCircle(param.nowAngle + param.dAlphaDiv2, maxValue + radiusIn, ref poly[1]);

                GetPointOnCircle(param.nowAngle + param.dAlphaDiv2, maxValue + radiusIn + param.oneBlock - visualConfig.RectSpace, ref poly[2]);
                GetPointOnCircle(param.nowAngle - param.dAlphaDiv2, maxValue + radiusIn + param.oneBlock - visualConfig.RectSpace, ref poly[3]);

                param.nowAngle += param.dAlpha;

                mainGraphics.FillPolygon(visualConfig.MaxBrush, poly);
            }
        }

        private int[] DrawSpectrum(float[] spectrumCurData, bool isReverse)
        {
            var param = CalcParams(spectrumCurData.Length, isReverse);
            var blockCount = new int[spectrumCurData.Length];

            var poly = new PointF[4] { new PointF(), new PointF(), new PointF(), new PointF() };
            for (int spectrumId = 0; spectrumId < spectrumCurData.Length; spectrumId++)
            {
                PointF start = new Point();
                PointF finish = new Point();
                GetPointOnCircle(param.nowAngle, radiusIn - 5, ref start);
                GetPointOnCircle(param.nowAngle, radiusOut, ref finish);
                var brush = new LinearGradientBrush(start, finish, visualConfig.LowLevelColor, visualConfig.HighLevelColor);

                blockCount[spectrumId] = (int)(spectrumCurData[spectrumId] / 100f * (radiusOut - radiusIn) / param.oneBlock);
                var spData = blockCount[spectrumId] * param.oneBlock;

                GetPointOnCircle(param.nowAngle - param.dAlphaDiv2, radiusIn, ref poly[0]);
                GetPointOnCircle(param.nowAngle + param.dAlphaDiv2, radiusIn, ref poly[1]);

                GetPointOnCircle(param.nowAngle + param.dAlphaDiv2, spData + radiusIn, ref poly[2]);
                GetPointOnCircle(param.nowAngle - param.dAlphaDiv2, spData + radiusIn, ref poly[3]);

                param.nowAngle += param.dAlpha;

                mainGraphics.FillPolygon(brush, poly);
            }

            if (visualConfig.RectSpace != 0)
            {
                Pen gridPen = new Pen(visualConfig.BackgroundColor, visualConfig.RectSpace);
                for (float radius = radiusIn; radius < radiusOut; radius += param.oneBlock)
                {
                    mainGraphics.DrawEllipse(
                        gridPen,
                        CurrentImage.Width / 2 - radius,
                        CurrentImage.Height / 2 - radius,
                        radius * 2,
                        radius * 2);
                }
            }
            mainGraphics.FillEllipse(visualConfig.BackgroundBrush, CurrentImage.Width / 2 - radiusIn, CurrentImage.Height / 2 - radiusIn, radiusIn * 2, radiusIn * 2);

            return blockCount;
        }

        private void GetPointOnCircle(float angleDeg, float radius, ref PointF point)
        {
            var radians = angleDeg * Math.PI / 180;
            point.X = (float)Math.Cos(radians) * radius + CurrentImage.Width / 2;
            point.Y = (float)Math.Sin(radians) * radius + CurrentImage.Height / 2;
        }
    }
}