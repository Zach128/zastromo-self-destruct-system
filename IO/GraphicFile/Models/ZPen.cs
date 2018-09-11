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

        /// <summary>
        /// Creates a new ZPen object.
        /// </summary>
        /// <param name="colour">The colour value for the pen. Must be a valid HTML colour string (eg. '#00ff12') or the name of a valid colour from the Color class.</param>
        /// <param name="size">The size of the pen 'strokes'.</param>
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

        /// <summary>
        /// Converts the local ZPen object to a Graphics.Pen object.
        /// </summary>
        /// <returns>A Graphhics.Pen equivalent of this ZPen object.</returns>
        public Pen ToPen()
        {
            Color color;
            //Check if the colour is an HTML string
            if (Colour.StartsWith("#"))
            {
                color = ColorTranslator.FromHtml(Colour);
            }
            //Assume it is a valid colour name from the Color class
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
