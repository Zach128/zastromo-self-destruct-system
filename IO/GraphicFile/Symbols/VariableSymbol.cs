using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class VariableSymbol : Symbol
    {
        public VariableSymbol(string name) : base(name)
        {
        }

        public VariableSymbol(string name, IType type) : base(name, type) { }
    }
}
