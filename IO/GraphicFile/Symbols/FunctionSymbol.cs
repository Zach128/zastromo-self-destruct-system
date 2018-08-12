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

        protected readonly IScope enclosingScope;

        public FunctionSymbol(string name, IType returnType, IScope enclosingScope) : base(name, returnType)
        {
        }

        public string ScopeName { get; }

        public IScope EnclosingScope { get; }

        public void Define(Symbol symbol)
        {
            throw new NotImplementedException();
        }

        public Symbol Resolve(string name)
        {
            throw new NotImplementedException();
        }
    }
}
