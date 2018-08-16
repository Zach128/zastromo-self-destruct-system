using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    public class BaseScope : IScope
    {

        protected readonly SymbolTable symbols;
        protected readonly string scopeName;

        public BaseScope(string scopeName, IScope enclosingScope = null)
        {
            this.scopeName = scopeName;
            this.EnclosingScope = enclosingScope;
            symbols = new SymbolTable(this);
        }

        public void Define(Symbol symbol)
        {
            symbols.Define(symbol);
        }

        public IScope EnclosingScope { get; }

        public Symbol Resolve(string name)
        {
            Symbol symbol = symbols.Resolve(name);
            if (symbol != null) return symbol;
            if (EnclosingScope != null) return EnclosingScope.Resolve(name);
            return null;
        }

        public string ScopeName => scopeName;
    }
}
