using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    class ClassTypeSymbol : Symbol, IType
    {

        public ClassTypeSymbol(string name) : base(name) { SymbolType = OBJECT; }
        public string GetName() { return name; }

    }
}
