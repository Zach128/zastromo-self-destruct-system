using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public class RootNode : ExprNode
    {
        public RootNode() : base() { EvalType = NodeType.GLOBAL; }

        public override void Visit(IZULVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
