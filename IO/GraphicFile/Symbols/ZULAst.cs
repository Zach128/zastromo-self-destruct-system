using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    [Obsolete("Not ideal for use. Remnant of alpha development of v0.1")]
    public class ZULAst : ExprNode
    {

        public Symbol symbol;
        public IScope scope;

        public ZULAst(Symbol s, IScope enclosedScope, Token token) : base(token)
        {
            symbol = s;
            scope = enclosedScope;
        }

        public ZULAst(Symbol s, IScope enclosedScope) : base()
        {
            symbol = s;
            scope = enclosedScope;
        }

        public override void Visit(IZULVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
