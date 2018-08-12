using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    class GlobalScope : BaseScope
    {
        public GlobalScope(string scopeName = "global", IScope enclosingScope = null) : base(scopeName, enclosingScope)
        {
        }
    }
}
