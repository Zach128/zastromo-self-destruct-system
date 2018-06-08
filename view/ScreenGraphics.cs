using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGraphicsController.view
{
    class ScreenGraphics
    {
        private RectangleF screenPix;
        private RectangleF ScreenPix
        {
            get { return screenPix; }
            set
            {
                screenPix = value;
                rmX = ScreenPix.Width / 100;
                rmY = ScreenPix.Height / 100;
            }
        }

        private float rmX = 0;
        private float rmY = 0;

        public ScreenGraphics(Graphics g)
        {
            ScreenPix = g.VisibleClipBounds;
        }

        public PointF NewPointRM(float percX, float percY)
        {
            return new PointF(PixX(percX), PixY(percY));
        }

        public PointF PointAlongLine(PointF begin, PointF end, float perc = 50f)
        {
            float relX1, relY1, relX2, relY2;
            float weight = Math.Max(0, Math.Min(100f, perc)) / 100f;

            //Get rm values of points
            relX1 = begin.X / rmX;
            relX2 = end.X / rmX;
            relY1 = begin.Y / rmY;
            relY2 = end.Y / rmY;

            //Calculate distance between points through relatives
            float x = relX1 + ((relX2 - relX1) * weight);
            float y = relY1 + ((relY2 - relY1) * weight);

            return NewPointRM(x, y);
        }

        /// <summary>
        /// Convert a percentage to a pixel X coordinate for use with the ScreenPix bound.
        /// </summary>
        /// <param name="percX"> </param>
        /// <returns></returns>
        private float PixX(float percX) { return (rmX * percX); }
        /// <summary>
        /// Convert a percentage to a pixel Y coordinate for use with the ScreenPix bound.
        /// </summary>
        /// <param name="percY"> </param>
        /// <returns></returns>
        private float PixY(float percY) { return (rmY * percY); }

    }
}
