﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AudioVisualizer.AudioSpectrum.Drawers
{
    sealed class RectangleDrawer : BaseDrawer
    {
        private int startAt;
        private int finishAt;

        private const float pbPart = 0.8f;

        private float scaleCoefY;

        private RectangleF[] gridRects;
        private LinearGradientBrush backgroundBrush;

        private void UpdateBackBrush()
        {
            backgroundBrush = new LinearGradientBrush(
               new PointF(0, CurrentImage.Height),
               new PointF(0, CurrentImage.Height * (1 - pbPart) - 5),
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

            scaleCoefY = (size.Height * pbPart) / 100;

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
