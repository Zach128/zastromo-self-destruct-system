using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public class DeclNode : ExprNode
    {

        public const int TYPE_DEF = 0;
        public const int DEF = 1;

        public DeclNode(ExprNode typeDefNode, ExprNode newDefNode) : base(new Token(TokenType.NONE))
        {
            EvalType = NodeType.DECL;
            AddChild(typeDefNode);
            AddChild(newDefNode);
        }
        public DeclNode(Token token, ExprNode typeDefNode, ExprNode newDefNode) : base(token)
        {
            EvalType = NodeType.DECL;
            AddChild(typeDefNode);
            AddChild(newDefNode);
        }

        public AST this[int index]
        {
            get
            {
                if (index == TYPE_DEF)
                { return Child(0); }
                else if (index == DEF)
                { return Child(1); }
                else throw new IndexOutOfRangeException("Index " + index + " out of bounds; must be 0 or 1.");
            }
        }

        public override void Visit(IZULVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
