using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public class ArrayNode : ExprNode
    {

        public const int ARR_NAME = -1;

        public ArrayNode(ExprNode nameNode, ExprNode[] elementNodes) : base(new Token(TokenType.NONE))
        {
            EvalType = NodeType.ARR;
            AddChild(nameNode);
            foreach (ExprNode elem in elementNodes) AddChild(elem);
        }
        public ArrayNode(Token token, ExprNode nameNode, ExprNode[] elementNodes) : base(token)
        {
            EvalType = NodeType.ARR;
            AddChild(nameNode);
            foreach (ExprNode elem in elementNodes) AddChild(elem);
        }

        public AST this[int index]
        {
            get
            {
                if (index >= -1 && index < ChildCount()) return Child(index + 1);
                else throw new IndexOutOfRangeException("Invalid index: " + index + ". min: -1, max: " + (ChildCount() - 1));
            }
        }

        public override void Visit(IZULVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}
