using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Models
{
    public class Line
    {
        private readonly ZPoint point1;
        private readonly ZPoint point2;

        public Line(ZPoint p1, ZPoint p2)
        {
            point1 = p1;
            point2 = p2;
        }

        public ZPoint Point1 => point1;
        public ZPoint Point2 => point2;

        public override string ToString()
        {
            return "<" + Point1.ToString() + ", " + Point2.ToString() + ">";
        }

    }
}
