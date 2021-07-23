using System;
using System.Collections.Generic;
using System.Drawing;

namespace AudioVisualizer.AudioSpectrum.Drawers
{
    sealed class RectangleDrawer : BaseDrawer
    {
        private int startAt;
        private int finishAt;

        private const float pbPart = 0.8f;

        private float scaleCoefY;
        private int maxCountRect;

        private RectangleF[] gridRects;

        public override AnalyzerVisualConfig.VisualStyle VisualStyle { get { return AnalyzerVisualConfig.VisualStyle.Rectangle; } }

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

            scaleCoefY = (size.Height * pbPart) / 100;
            maxCountRect = (int)Math.Floor(size.Height * pbPart / (visualConfig.RectHeight + visualConfig.RectSpace));

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

            float scaleX = CurrentImage.Width / 2 * pbPart / 100;

            mainGraphics.FillRectangle(
                visualConfig.MaxBrush,
                CurrentImage.Width / 2 - (float)Math.Round(leftSpectrum.TotalLoad * scaleX) - 4,
                1,
                (float)Math.Round(leftSpectrum.TotalLoad * scaleX),
                10);

            mainGraphics.FillRectangle(
                visualConfig.MaxBrush,
                CurrentImage.Width / 2 + 4,
                1,
                (float)Math.Round(rightSpectrum.TotalLoad * scaleX),
                10);


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
                    CurrentImage.Height - maxLen - visualConfig.RectHeight,
                    visualConfig.ColWidth,
                    visualConfig.RectHeight);
            }

            if (maxRects.Length != 0)
            {
                mainGraphics.FillRectangles(visualConfig.MaxBrush, maxRects);
            }
        }

        private int[] DrawSpectrum(float[] spectrumCurData, float offset)
        {
            if (spectrumCurData.Length == 0)
            {
                return new int[0];
            }

            var rectsCount = new int[spectrumCurData.Length];
            var lowRects = new RectangleF[spectrumCurData.Length];
            var mediumRects = new RectangleF[spectrumCurData.Length];
            var highRects = new RectangleF[spectrumCurData.Length];

            for (int spectrumInd = 0; spectrumInd < spectrumCurData.Length; spectrumInd++)
            {
                float baseCurLen = spectrumCurData[spectrumInd];
                float curLen = baseCurLen * scaleCoefY;

                int y = 0;
                int nowSquare = 0;

                var lowRect = new RectangleF(
                        spectrumInd * (visualConfig.ColWidth + visualConfig.ColSpace) + offset,
                        CurrentImage.Height,
                        visualConfig.ColWidth,
                        0);

                var mediumRect = new RectangleF(
                        spectrumInd * (visualConfig.ColWidth + visualConfig.ColSpace) + offset,
                        CurrentImage.Height,
                        visualConfig.ColWidth,
                        0);

                var highRect = new RectangleF(
                        spectrumInd * (visualConfig.ColWidth + visualConfig.ColSpace) + offset,
                        CurrentImage.Height,
                        visualConfig.ColWidth,
                        0);

                while (y < curLen && baseCurLen > 0.5f)
                {
                    if (nowSquare > 2f * maxCountRect / 3f)
                    {
                        highRect.Height += visualConfig.RectSpace + visualConfig.RectHeight;
                        highRect.Y -= visualConfig.RectSpace + visualConfig.RectHeight;
                    }
                    else if (nowSquare > maxCountRect / 3f)
                    {
                        mediumRect.Height += visualConfig.RectSpace + visualConfig.RectHeight;
                        mediumRect.Y -= visualConfig.RectSpace + visualConfig.RectHeight;
                    }
                    else
                    {
                        lowRect.Height += visualConfig.RectSpace + visualConfig.RectHeight;
                        lowRect.Y -= visualConfig.RectSpace + visualConfig.RectHeight;
                    }

                    y += (visualConfig.RectHeight + visualConfig.RectSpace);
                    nowSquare++;
                }

                rectsCount[spectrumInd] = nowSquare;

                mediumRect.Y -= lowRect.Height;
                highRect.Y -= lowRect.Height;
                highRect.Y -= mediumRect.Height;

                lowRects[spectrumInd] = lowRect;
                mediumRects[spectrumInd] = mediumRect;
                highRects[spectrumInd] = highRect;
            }

            mainGraphics.FillRectangles(visualConfig.LowBrush, lowRects);
            mainGraphics.FillRectangles(visualConfig.MediumBrush, mediumRects);
            mainGraphics.FillRectangles(visualConfig.HighBrush, highRects);

            return rectsCount;
        }

    }
}
