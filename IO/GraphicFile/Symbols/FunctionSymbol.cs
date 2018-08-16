using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    class FunctionSymbol : Symbol, IScope
    {

        protected readonly SymbolTable symbols;
        protected readonly IScope enclosingScope;

        public FunctionSymbol(string name, IType returnType, IScope enclosingScope) : base(name, returnType)
        {
            symbols = new SymbolTable(this);
            SymbolType = FUNCTION_REF;
        }

        public string ScopeName { get; }

        public IScope EnclosingScope { get; }

        public void Define(Symbol symbol)
        {
            symbols.Define(symbol);
        }

        public Symbol Resolve(string name)
        {
            Symbol symbol = symbols.Resolve(name);
            if (symbol != null) return symbol;
            if (EnclosingScope != null) return EnclosingScope.Resolve(name);
            return null;
        }
    }
}
