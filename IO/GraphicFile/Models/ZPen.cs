using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Models
{
    public class ZPen
    {

        private string colour;
        private float size;

        public ZPen(string colour, float size)
        {
            Colour = colour;
            Size = size;
        }

        public override string ToString()
        {
            return "<pen Colour: " + Colour + ", Size: " + Size + ">";
        }

        public string Colour { get => colour; set => colour = value; }
        public float Size { get => size; set => size = value; }

        public Pen ToPen()
        {
            Color color;
            if (Colour.StartsWith("#"))
            {
                color = ColorTranslator.FromHtml(Colour);
            }
            else
            {
                color = Color.FromName(Colour);
            }
            return new Pen(color, Size);
        }

        public static implicit operator Pen(ZPen z)
        {
            return z.ToPen();
        }

        public static implicit operator ZPen(Pen p)
        {
            return new ZPen(p.Color.ToString(), p.Width);
        }

    }
}
