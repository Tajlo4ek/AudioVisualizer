using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer.AudioSpectrum.Drawers
{
    public static class DrawerFactory
    {
        public static BaseDrawer GetDrawer(AnalyzerVisualConfig.VisualStyle visualStyle)
        {
            switch (visualStyle)
            {
                default:
                case AnalyzerVisualConfig.VisualStyle.Rectangle:
                    return new RectangleDrawer();

                case AnalyzerVisualConfig.VisualStyle.Circle:
                    return new CircleDrawer();
            }
        }

    }
}
