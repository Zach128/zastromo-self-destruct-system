using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public class AssignNode : ExprNode
    {

        public const int SUBJECT = 0;
        public const int NEW_VALUE = 1;

        public AssignNode(ExprNode subject, ExprNode newAssign) : base(new Token(TokenType.COLON, ":"))
        {
            EvalType = NodeType.ASSIGN;
            AddChild(subject);
            AddChild(newAssign);
        }

        public AssignNode(Token assignToken, ExprNode subject, ExprNode newAssign) : base(assignToken)
        {
            EvalType = NodeType.ASSIGN;
            AddChild(subject);
            AddChild(newAssign);
        }

        public AST this[int index]
        {
            get
            {
                if (index == SUBJECT)
                { return Child(0); }
                else if (index == NEW_VALUE)
                { return Child(1); }
                else return null;
            }
        }

    }
}
