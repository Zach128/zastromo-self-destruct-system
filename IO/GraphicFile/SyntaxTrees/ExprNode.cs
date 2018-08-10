using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

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

        public override string ToString()
        {
            if (EvalType != NodeType.INVALID)
            {
                return "{" + base.ToString() + "type=" + EvalType.ToString() + "}";
            }
            return base.ToString();
        }

    }
}
