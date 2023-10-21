using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using AudioVisualizer.Utils;

namespace AudioVisualizer.AudioSpectrum.Drawers
{
    sealed class RectangleDrawer : BaseDrawer
    {
        private int startAt;
        private int finishAt;

        private const float pbPart = 0.5f;
        private int imageHeight;

        private float scaleCoefY;

        private RectangleF[] gridRects;
        private LinearGradientBrush backgroundBrush;

        private void UpdateBackBrush()
        {
            backgroundBrush = new LinearGradientBrush(
               new PointF(0, CurrentImage.Height),
               new PointF(0, CurrentImage.Height - imageHeight - 5),
               visualConfig.LowLevelColor,
               visualConfig.HighLevelColor);
        }

        public override AnalyzerVisualConfig.VisualStyle VisualStyle { get { return AnalyzerVisualConfig.VisualStyle.Rectangle; } }

        public override void SetVisualConfig(AnalyzerVisualConfig visualConfig)
        {
            base.SetVisualConfig(visualConfig);
            UpdateBackBrush();
        }

        public override void CreateCurrentImage(Spectrum leftSpectrum, Spectrum rightSpectrum)
        {
            mainGraphics.Clear(visualConfig.BackgroundColor);

            Draw(leftSpectrum, rightSpectrum);
        }

        public override void SetSize(Size size)
        {
            base.SetSize(size);

            var totalColWidth = visualConfig.ColWidth + visualConfig.ColSpace;
            var needCount = (int)Math.Floor(((float)size.Width - totalColWidth * 2) / (totalColWidth * 2));

            Analyzer.TestCountLine(needCount, dataConfig.FftDataSize, out int realCountLine, out startAt, out finishAt);
            LineCount = realCountLine;

            if (LineCount < 0)
            {
                LineCount = 0;
            }

            imageHeight = CurrentImage.Height * pbPart > 300 ? 300 : (int)(CurrentImage.Height * pbPart);
            scaleCoefY = (imageHeight) / 100;

            var gridRects = new List<RectangleF>();
            var yPos = visualConfig.RectHeight + visualConfig.RectSpace;

            while (yPos < CurrentImage.Height)
            {
                gridRects.Add(new RectangleF(
                    0,
                    CurrentImage.Height - yPos,
                    CurrentImage.Width,
                    visualConfig.RectSpace));

                yPos += visualConfig.RectHeight + visualConfig.RectSpace;
            }

            this.gridRects = gridRects.ToArray();
            UpdateBackBrush();
        }


        private void Draw(Spectrum leftSpectrum, Spectrum rightSpectrum)
        {
            var left = new float[finishAt - startAt];
            Array.Copy(leftSpectrum.GetCurrentData(), startAt, left, 0, finishAt - startAt);

            var leftMax = new float[finishAt - startAt];
            Array.Copy(leftSpectrum.GetMaxData(), startAt, leftMax, 0, finishAt - startAt);

            var right = new float[finishAt - startAt];
            Array.Copy(rightSpectrum.GetCurrentData(), startAt, right, 0, finishAt - startAt);
            Array.Reverse(right);

            var rightMax = new float[finishAt - startAt];
            Array.Copy(rightSpectrum.GetMaxData(), startAt, rightMax, 0, finishAt - startAt);
            Array.Reverse(rightMax);

            var rightOffset = CurrentImage.Width - (finishAt - startAt) * (visualConfig.ColSpace + visualConfig.ColWidth) + visualConfig.ColSpace;

            var leftRectCount = DrawSpectrum(left, 0);
            var rightRectCount = DrawSpectrum(right, rightOffset);

            DrawGrid();

            DrawLoad(leftSpectrum.TotalLoad, CurrentImage.Width / 2, 0);
            DrawLoad(rightSpectrum.TotalLoad, CurrentImage.Width / 2, CurrentImage.Width / 2);

            DrawSpectrumMax(leftMax, leftRectCount, 0);
            DrawSpectrumMax(rightMax, rightRectCount, rightOffset);
        }

        private void DrawGrid()
        {
            if (gridRects.Length != 0)
            {
                mainGraphics.FillRectangles(visualConfig.BackgroundBrush, gridRects);
            }
        }

        private void DrawLoad(float load, float size, float rightOffset)
        {
            float outRadius = size / 2f / 2;
            float inRadius = outRadius * 0.95f;

            const int steps = 100;
            const double startPos = Math.PI;
            const double partSize = Math.PI;

            double alpha = startPos;
            double alphaDx = partSize / steps;

            Color startColor = Color.Green;
            Color endColor = Color.Red;
            var colors = ColorUtils.GetGradients(startColor, endColor, steps);

            PointF center = new PointF(size / 2 + rightOffset, outRadius + 20);

            Color lineColor = endColor;

            int step = 0;
            foreach (var color in colors)
            {
                PointF p1 = new PointF(inRadius * (float)Math.Cos(alpha) + center.X, inRadius * (float)Math.Sin(alpha) + center.Y);
                PointF p2 = new PointF(outRadius * (float)Math.Cos(alpha) + center.X, outRadius * (float)Math.Sin(alpha) + center.Y);
                PointF p3 = new PointF(outRadius * (float)Math.Cos(alpha + alphaDx) + center.X, outRadius * (float)Math.Sin(alpha + alphaDx) + center.Y);
                PointF p4 = new PointF(inRadius * (float)Math.Cos(alpha + alphaDx) + center.X, inRadius * (float)Math.Sin(alpha + alphaDx) + center.Y);

                mainGraphics.FillPolygon(
                   new SolidBrush(color),
                   new PointF[] { p1, p2, p3, p4 });

                alpha += alphaDx;

                if (load - 1 < step && load >= step)
                {
                    lineColor = color;
                }
                step++;
            }

            float centerRadius = inRadius * 0.15f / 2;
            mainGraphics.FillEllipse(new SolidBrush(lineColor), center.X - centerRadius, center.Y - centerRadius, centerRadius * 2, centerRadius * 2);

            float loadInPi = (float)(partSize * load / 100 + startPos);
            PointF p1Center = new PointF(centerRadius * (float)Math.Cos(loadInPi + Math.PI / 2) + center.X, centerRadius * (float)Math.Sin(loadInPi + Math.PI / 2) + center.Y);
            PointF p2Center = new PointF(centerRadius * (float)Math.Cos(loadInPi - Math.PI / 2) + center.X, centerRadius * (float)Math.Sin(loadInPi - Math.PI / 2) + center.Y);
            PointF p3Center = new PointF(inRadius * (float)Math.Cos(loadInPi) + center.X, inRadius * (float)Math.Sin(loadInPi) + center.Y);

            mainGraphics.FillPolygon(new SolidBrush(lineColor), new PointF[] { p1Center, p2Center, p3Center });

        }

        private void DrawSpectrumMax(float[] spectrumMaxData, int[] rectsCount, float offset)
        {
            var maxRects = new RectangleF[spectrumMaxData.Length];

            for (int spectrumInd = 0; spectrumInd < spectrumMaxData.Length; spectrumInd++)
            {
                float maxLen = spectrumMaxData[spectrumInd] * scaleCoefY;
                float onRectLen = rectsCount[spectrumInd] * (visualConfig.RectHeight + visualConfig.RectSpace);

                if (maxLen < onRectLen)
                {
                    maxLen = onRectLen;
                }

                maxRects[spectrumInd] = new RectangleF(
                    spectrumInd * (visualConfig.ColWidth + visualConfig.ColSpace) + offset,
                    CurrentImage.Height - maxLen,
                    visualConfig.ColWidth,
                    visualConfig.RectHeight);
            }

            if (maxRects.Length != 0)
            {
                mainGraphics.FillRectangles(backgroundBrush, maxRects);
            }
        }

        private int[] DrawSpectrum(float[] spectrumCurData, float offset)
        {
            if (spectrumCurData.Length == 0)
            {
                return new int[0];
            }

            var backRect = new RectangleF[spectrumCurData.Length];
            var rectsCount = new int[spectrumCurData.Length];

            for (int spectrumInd = 0; spectrumInd < spectrumCurData.Length; spectrumInd++)
            {
                float baseCurLen = spectrumCurData[spectrumInd];
                int countRect = (int)(baseCurLen * scaleCoefY / (visualConfig.RectSpace + visualConfig.RectHeight));
                float curLen = countRect * (visualConfig.RectSpace + visualConfig.RectHeight);

                backRect[spectrumInd] = new RectangleF(
                    spectrumInd * (visualConfig.ColWidth + visualConfig.ColSpace) + offset,
                    CurrentImage.Height - curLen,
                    visualConfig.ColWidth,
                    curLen);

                rectsCount[spectrumInd] = countRect;
            }

            mainGraphics.FillRectangles(backgroundBrush, backRect);

            return rectsCount;
        }

    }
}
