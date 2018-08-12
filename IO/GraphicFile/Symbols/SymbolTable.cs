using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class SymbolTable
    {
        private readonly Dictionary<string, Symbol> symbols;

        public SymbolTable()
        {
            symbols = new Dictionary<string, Symbol>();
            DefineSystemSymbolTypes();
        }

        public void Define(Symbol symbol) { symbols[symbol.Name] = symbol; }
        public Symbol Resolve(string name) { return symbols[name]; }

        public override string ToString()
        {
            return symbols.ToString();
        }

        public void DefineSystemSymbolTypes()
        {
            Define(new SystemTypeSymbol("int"));
            Define(new SystemTypeSymbol("decimal"));
            Define(new SystemTypeSymbol("float"));
            Define(new SystemTypeSymbol("rupe"));
        }

    }
}
