using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    class LocalScope : BaseScope
    {
        public LocalScope(string scopeName, IScope enclosingScope = null) : base("local_" + scopeName, enclosingScope)
        {
        }
    }
}
