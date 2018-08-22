using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Interpreters;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Symbols;

namespace TestWinBackGrnd.IO.GraphicFile.Interpreters
{
    public class ZULInterpreter : Interpreter
    {

        public readonly Graphics graphic;

        public ZULInterpreter(SymbolTable symbolTable, Graphics g) : base(symbolTable)
        {
            graphic = g;
        }

        public override object ProcessFunction(FunctionSymbol function)
        {
            switch(function.Name)
            {
                case "point":
                    if (ValidateFuncArgs(function, new Type[] { typeof(Rupe), typeof(Rupe) }))
                    {
                        return new ZPoint((Rupe)function.ArgAt(0), (Rupe)function.ArgAt(1));
                    } else if(ValidateFuncArgs(function, new Type[] { typeof(float), typeof(float) }))
                    {
                        return new ZPoint((float)function.ArgAt(0), (float)function.ArgAt(1));
                    }
                    else return null;

                case "pen":
                    if (ValidateFuncArgs(function, new Type[] { typeof(string), typeof(float) }))
                    {
                        return new ZPen((string)function.ArgAt(0), (float)function.ArgAt(1));
                    }
                    else return null;

                case "line":
                    if (ValidateFuncArgs(function, new Type[] { typeof(ZPoint), typeof(ZPoint) }))
                    {
                        return new Line((ZPoint)function.ArgAt(0), (ZPoint)function.ArgAt(1));
                    }
                    else return null;

                case "linesplt":
                    if (ValidateFuncArgs(function, new Type[] { typeof(ZPoint), typeof(ZPoint) }))
                    {
                        ZPoint a = (ZPoint)function.ArgAt(0);
                        ZPoint b = (ZPoint)function.ArgAt(1);
                        double midX = (a.X + b.X) / 2.0;
                        double midY = (a.Y + b.Y) / 2.0;
                        return new ZPoint(midX, midY);
                    }
                    else return null;

                case "draw":
                    if (ValidateFuncArgs(function, new Type[] { typeof(Line) }))
                        return Draw((Line) function.ArgAt(0));
                    else return null;

                case "drawPoly":
                    if (ValidateFuncArgs(function, new Type[] { typeof(List<>) }))
                    {
                        object arg0 = function.ArgAt(0);
                        ZPoint[] points = ((IEnumerable)function.ArgAt(0)).Cast<ZPoint>()
                            .Select(x => x)
                            .ToArray();

                        return DrawPoly(points);
                    }
                        
                    else return null;
                default:
                    return null;
            }
        }

        public object Draw(Line line)
        {
            Pen pen = ((ZPen) LoadValue("basePen")).ToPen();
            graphic.DrawLine(pen, line.Point1, line.Point2);
            return null;
        }

        public object DrawPoly(ZPoint[] points)
        {
            Pen pen = ((ZPen)LoadValue("basePen")).ToPen();
            if (graphic != null && points != null)
            {
                Point[] poly = Array.ConvertAll<ZPoint, Point>(points, new Converter<ZPoint, Point>(pf =>
                {
                    return (Point)pf;
                }));
                graphic.DrawPolygon(pen, poly);
            }
            return null;
        }

    }
}
