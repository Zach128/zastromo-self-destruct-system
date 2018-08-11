using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public abstract class ExprNode : AST
    {
        public ExprNode(Token token) : base(token)
        {
        }

        private NodeType evalType;
        public NodeType EvalType
        {
            get
            { return evalType; }
            protected set
            { evalType = value; }
        }

        public new ExprNode Child(int index)
        {
            return (ExprNode) base.Child(index);
        }

        public override string ToString()
        {
            if (EvalType != NodeType.INVALID)
            {
                return "{" + base.ToString() + "type=" + EvalType.ToString() + "}";
            }
            return base.ToString();
        }

        public abstract void Visit(IZULVisitor visitor);

    }
}
