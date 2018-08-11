using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;

namespace TestWinBackGrnd.IO.GraphicFile.Visitors
{
    public class PrintTreeVisitor : IZULVisitor
    {
        public void Visit(ArrayNode node)
        {
            ToStringTree(node);
        }

        public void Visit(ArrRetNode node)
        {
            ToStringTree(node);
        }

        public void Visit(AssignNode node)
        {
            ToStringTree(node);
        }

        public void Visit(DeclNode node)
        {
            ToStringTree(node);
        }

        public void Visit(FuncNode node)
        {
            ToStringTree(node);
        }

        public void Visit(NameNode node)
        {
            ToStringTree(node);
        }

        public void Visit(NumNode node)
        {
            ToStringTree(node);
        }

        /// <summary>
        /// Simple recursive printing of nodes, token definitions and children.
        /// </summary>
        /// <param name="node"></param>
        public void ToStringTree(ExprNode node)
        {
            if (node.ChildCount() == 0) { Console.WriteLine(node.ToString()); }
            StringBuilder buffer = new StringBuilder();
            if (!node.IsNil())
            {
                buffer.Append("(");
                buffer.Append(node.ToString());
                buffer.Append(" ");
            }
            for (int i = 0; i < node.ChildCount(); i++)
            {
                AST t = (AST) node.Child(i);
                if (i > 0) buffer.Append(", ");
                buffer.Append(t.ToStringTree());
            }
            if (!node.IsNil()) buffer.Append(")");

            Console.WriteLine(buffer.ToString());

        }

    }
}
