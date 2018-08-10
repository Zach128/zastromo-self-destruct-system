using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public class FuncNode : ExprNode
    {

        public const int FUNC_NAME = 0;

        public FuncNode(ExprNode nameNode, ExprNode[] paramNode) : base(new Token(TokenType.NONE))
        {
            EvalType = NodeType.FUNC;
            AddChild(nameNode);
            foreach (ExprNode param in paramNode) AddChild(param);
        }
        public FuncNode(Token funcToken, ExprNode nameNode, ExprNode[] paramNode) : base(funcToken)
        {
            EvalType = NodeType.FUNC;
            AddChild(nameNode);
            foreach (ExprNode param in paramNode) AddChild(param);
        }

        public AST this[int index]
        {
            get
            {
                return Child(index);
            }
        }

    }
}
