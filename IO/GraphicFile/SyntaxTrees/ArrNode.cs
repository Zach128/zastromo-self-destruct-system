using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public class ArrNode : ExprNode
    {

        public ArrNode() : base(new Token(TokenType.NONE))
        {
            EvalType = NodeType.ARR;
        }
        public ArrNode(ExprNode[] elementNodes) : base(new Token(TokenType.NONE))
        {
            EvalType = NodeType.ARR;
            foreach (ExprNode elem in elementNodes) AddChild(elem);
        }
        public ArrNode(Token token, ExprNode[] elementNodes) : base(token)
        {
            EvalType = NodeType.ARR;
            foreach (ExprNode elem in elementNodes) AddChild(elem);
        }

        public void AddElement(ExprNode element)
        {
            AddChild(element);
        }

        public AST this[int index]
        {
            get
            {
                if (index >= 0 && index < ChildCount()) return Child(index);
                else throw new IndexOutOfRangeException("Invalid index: " + index + ". min: 0, max: " + (ChildCount() - 1));
            }
        }

        public override void Visit(IZULVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
