using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile
{
    /// <summary>
    /// Represents a token/object mismatch in parsers and lexers.
    /// Use when syntax in a given context dictates an expected element but a different element is found and cannot be accepted.
    /// </summary>
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
