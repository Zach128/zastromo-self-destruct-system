using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    [Obsolete("Unimplemented. Use MonolithicSymbolTable for scope declaration.", true)]
    class LocalScope : BaseScope
    {
        public LocalScope(string scopeName, IScope enclosingScope = null) : base("local_" + scopeName, enclosingScope)
        {
        }
    }
}
