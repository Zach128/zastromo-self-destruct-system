using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Symbols;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public abstract class ExprNode : AST
    {

        public ExprNode() : base(new Token(TokenType.NONE)) { }
        public ExprNode(Token token) : base(token) { }
        
        public NodeType EvalType { get; set; }

        public IType VarType;

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
