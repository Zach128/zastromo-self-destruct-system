using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class ConstantSymbol : Symbol
    {

        public readonly object value;

        public ConstantSymbol(string name, object val) : base(name)
        {
            SymbolType = CONSTANT;
            value = val;
        }

        public ConstantSymbol(string name, IType type, object val) : base(name, type) {
            SymbolType = CONSTANT;
            value = val;
        }
    }
}
