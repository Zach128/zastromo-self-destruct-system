using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile
{
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException() : base()
        {
        }
        public MatchNotFoundException(string message) : base(message)
        {
        }
    }
}
