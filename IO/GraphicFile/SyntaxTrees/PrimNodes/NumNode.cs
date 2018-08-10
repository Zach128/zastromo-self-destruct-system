using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes
{
    public class NumNode : ExprNode
    {
        public NumNode(Token token) : base(token)
        {
            switch(token.type)
            {
                case TokenType.INTEGER:
                    EvalType = NodeType.INTEGER;
                    break;
                case TokenType.DECIMAL:
                    EvalType = NodeType.DECIMAL;
                    break;
                case TokenType.FLOAT:
                    EvalType = NodeType.FLOAT;
                    break;
                case TokenType.RUPE:
                    EvalType = NodeType.RUPE;
                    break;
                default:
                    EvalType = NodeType.INVALID;
                    break;
            }
        }
    }
}
