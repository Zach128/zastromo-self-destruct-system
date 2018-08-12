using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class BuiltInTypeSymbol : Symbol
    {
        public BuiltInTypeSymbol(string name) : base(name)
        {
        }

        public BuiltInTypeSymbol(string name, IType type) : base(name, type) { }
    }
}
