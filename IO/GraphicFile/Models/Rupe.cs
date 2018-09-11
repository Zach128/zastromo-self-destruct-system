using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Models
{
    public struct Rupe
    {
        private readonly float value;
        public Rupe(float val) { this.value = Math.Max(Math.Min(val, 100), -100); }
        public float Value { get { return value; } }
        public static implicit operator Rupe(float f) { return new Rupe(f); }
        public static implicit operator float(Rupe r) { return r.Value; }
    }
}
