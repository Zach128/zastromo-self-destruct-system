using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    class MonolithicSymbolTable : SymbolTable, IScope
    {
        public MonolithicSymbolTable() : base(null)
        {
            ScopeName = "_global";
            InitTypes(this);
        }

        public string ScopeName { get; }

        public IScope EnclosingScope => null;
    }
}
