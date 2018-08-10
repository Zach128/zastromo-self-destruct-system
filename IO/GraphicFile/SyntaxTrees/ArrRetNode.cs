using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public class ArrRetNode : ExprNode
    {
        public const int ARR_NAME = 0;
        public const int INDEX = 1;

        public ArrRetNode(ExprNode arrNode, ExprNode indexNode) : base(new Token(TokenType.NONE))
        {
            EvalType = NodeType.ARRRET;
            AddChild(arrNode);
            AddChild(indexNode);
        }

        public ArrRetNode(Token assignToken, ExprNode arrNode, ExprNode indexNode) : base(assignToken)
        {
            EvalType = NodeType.ARRRET;
            AddChild(arrNode);
            AddChild(indexNode);
        }

        public AST this[int index]
        {
            get
            {
                if (index == ARR_NAME)
                { return Child(0); }
                else if (index == INDEX)
                { return Child(1); }
                else return null;
            }
        }
    }
}
