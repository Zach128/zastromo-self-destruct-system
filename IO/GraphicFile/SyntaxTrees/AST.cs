using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public abstract class AST
    {
        protected Token token;
        private List<AST> children;
        public AST() { }
        public AST (Token token) { this.token = token; }
        public AST(TokenType type) { this.token = new Token(type); }

        public TokenType GetNodeTokenType() { return token.type; }
        public bool IsNil() { return token == null; }

        public void AddChild (AST t)
        {
            if (children == null) children = new List<AST>();
            children.Add(t);
        }
        public int ChildCount() { return children.Count; }

        public override string ToString()
        { return token != null ? token.ToString() : "nil"; }

        public bool IsChildrenNull() { return children == null; }

        public string ToStringTree()
        {
            if(children == null || children.Count == 0) { return this.ToString(); }
            StringBuilder buffer = new StringBuilder();
            if(!IsNil())
            {
                buffer.Append("(");
                buffer.Append(this.ToString());
                buffer.Append(" ");
            }
            for(int i = 0; i < children.Count; i++)
            {
                AST t = (AST)children[i];
                if (i > 0) buffer.Append(", ");
                buffer.Append(t.ToStringTree());
            }
            if (!IsNil()) buffer.Append(")");
            return buffer.ToString();
        }

        public AST Child(int index)
        {
            return children[index];
        }
    }
}
