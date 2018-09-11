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
using WinGraphicsController.view;

namespace TestWinBackGrnd.IO.GraphicFile.Interpreters
{
    public class ZULInterpreter : Interpreter
    {

        //Contains the Graphics object to be used in rendering
        public readonly Graphics graphic;
        public readonly ScreenGraphics gfxUtils;

        private int viewWidth = 0;
        private int viewHeight = 0;

        public ZULInterpreter(SymbolTable symbolTable, Graphics g) : base(symbolTable)
        {
            graphic = g;
            gfxUtils = new ScreenGraphics(g);
            VERSION_SUPPORTED = 0.1;
        }

        /// <summary>
        /// Parse function symbols, calling the associated method of the symbol if valid.
        /// </summary>
        /// <param name="function">The function symbol to be parsed, containing the function name and list of arguments.</param>
        /// <returns>An object returned by the processed function if valid, or null if invalid.</returns>
        public override object ProcessFunction(FunctionSymbol function)
        {
            switch(function.Name)
            {
                // name: point args: rupe, rupe
                // name: point args: float, float
                case "point":
                    if (ValidateFuncArgs(function, new Type[] { typeof(Rupe), typeof(Rupe) }))
                    {
                        return new ZPoint((Rupe)function.ArgAt(0), (Rupe)function.ArgAt(1));
                    } else if(ValidateFuncArgs(function, new Type[] { typeof(float), typeof(float) }))
                    {
                        return new ZPoint((float)function.ArgAt(0), (float)function.ArgAt(1));
                    }
                    else return null;
                // name: pen args: string, float
                case "pen":
                    if (ValidateFuncArgs(function, new Type[] { typeof(string), typeof(float) }))
                    {
                        return new ZPen((string)function.ArgAt(0), (float)function.ArgAt(1));
                    }
                    else return null;
                // name: line args: ZPoint, ZPoint
                case "line":
                    if (ValidateFuncArgs(function, new Type[] { typeof(ZPoint), typeof(ZPoint) }))
                    {
                        return new Line((ZPoint)function.ArgAt(0), (ZPoint)function.ArgAt(1));
                    }
                    else return null;
                // name: linesplt args: ZPoint, ZPoint
                case "linesplt":
                    if (ValidateFuncArgs(function, new Type[] { typeof(ZPoint), typeof(ZPoint) }))
                    {
                        //Calculate the midpoint of the two provided ZPoint coordinates
                        ZPoint a = (ZPoint)function.ArgAt(0);
                        ZPoint b = (ZPoint)function.ArgAt(1);
                        double midX = (a.X + b.X) / 2.0;
                        double midY = (a.Y + b.Y) / 2.0;

                        //return the new coordinate
                        return new ZPoint(midX, midY);
                    }
                    else return null;
                // name: draw args: Line
                case "draw":
                    if (ValidateFuncArgs(function, new Type[] { typeof(Line) }))
                        return Draw((Line) function.ArgAt(0));
                    else return null;
                // name: drawPoly args: List<ZPoint>
                case "drawPoly":
                    if (ValidateFuncArgs(function, new Type[] { typeof(List<>) }))
                    {
                        //Cast the generic object in argument 0 to a list of ZPoint
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

        public override bool ProcessSpecificSystemVar(string name, object val)
        {
            switch(name)
            {
                case "VIEW_WIDTH":
                    break;
                case "VIEW_HEIGHT":
                    break;
            }

            return false;
        }

        /// <summary>
        /// Call Graphics.DrawLine using the provided Line parameter.
        /// </summary>
        /// <param name="line">The line object to be drawn.</param>
        /// <returns></returns>
        public object Draw(Line line)
        {
            //Load the basePen variable to a System.Drawing.Pen object
            Pen pen = ((ZPen) LoadValue("basePen")).ToPen();

            if(graphic != null && line != null)
            {
                //Call the DrawLine method using the pen obtained and the points of the provided line
                graphic.DrawLine(pen, line.Point1, line.Point2);
            }

            //No need to return anything
            return null;
        }

        public object DrawPoly(ZPoint[] points)
        {
            //Load the basePen variable to a System.Drawing.Pen object
            Pen pen = ((ZPen)LoadValue("basePen")).ToPen();

            if (graphic != null && points != null)
            {
                //Convert the ZPoint array to a Point array using a Converter object
                Point[] poly = Array.ConvertAll<ZPoint, Point>(points, new Converter<ZPoint, Point>(pf =>
                {
                    return (Point)pf;
                }));

                //Call the DrawPolygon function using the poly array
                graphic.DrawPolygon(pen, poly);
            }

            //No need to return anything
            return null;
        }

    }
}
