using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Models
{
    public class ZPoint
    {
        public ZPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public ZPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public ZPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public ZPoint(Rupe x, Rupe y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public override string ToString()
        {
            return "<x: " + X + ", y: " + Y + ">";
        }

        public static implicit operator Point(ZPoint z) { return new Point((int) z.X, (int) z.Y); }
        public static implicit operator ZPoint(Point point) { return new ZPoint(point.X, point.Y); }

    }
}
