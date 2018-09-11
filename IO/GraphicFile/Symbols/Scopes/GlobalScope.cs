using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    [Obsolete("Unimplemented. Use MonolithicSymbolTable for scope declaration.", true)]
    class GlobalScope : BaseScope
    {
        public GlobalScope(string scopeName = "global", IScope enclosingScope = null) : base(scopeName, enclosingScope)
        {
        }
    }
}
