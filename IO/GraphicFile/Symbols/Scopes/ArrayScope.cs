using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    [Obsolete("Unimplemented. Use MonolithicSymbolTable for scope declaration.", true)]
    public class ArrayScope : BaseScope
    {
        public ArrayScope(string scopeName, IScope enclosingScope = null) : base(scopeName, enclosingScope)
        {
        }
    }
}
