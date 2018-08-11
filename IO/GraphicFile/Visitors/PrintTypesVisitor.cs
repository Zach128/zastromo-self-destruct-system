using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;

namespace TestWinBackGrnd.IO.GraphicFile.Visitors
{
    public class PrintTypesVisitor : IZULVisitor
    {

        string buffer;
        int leadingWSCount = 0;

        public PrintTypesVisitor()
        {
            buffer = "";
            leadingWSCount = 0;
        }

        public void Visit(ArrayNode node)
        {
            buffer = "Array named " + node[ArrayNode.ARR_NAME] + " with " + (node.ChildCount() - 1) + " elements.";
            PrintWithWS();
            VisitChildren(node);
        }

        public void Visit(ArrRetNode node)
        {
            buffer = "Array " + node[ArrRetNode.ARR_NAME] + " accessed at index " + node[ArrRetNode.INDEX];
            PrintWithWS();
            VisitChildren(node);
        }

        public void Visit(AssignNode node)
        {
            buffer = "Assignment to " + node[AssignNode.SUBJECT];
            PrintWithWS();
            VisitChildren(node);
        }

        public void Visit(DeclNode node)
        {
            buffer = "Declaration of a " + node[DeclNode.TYPE_DEF];
            PrintWithWS();
            VisitChildren(node);
        }

        public void Visit(FuncNode node)
        {
            buffer = "Function named " + node[FuncNode.FUNC_NAME];
            PrintWithWS();
            VisitChildren(node);
        }

        public void Visit(NameNode node)
        {
            buffer = "Name found; " + node.GetName();
            PrintWithWS();
            VisitChildren(node);
        }

        public void Visit(NumNode node)
        {
            buffer = "Number of type " + node.GetNodeTokenType().ToString() + " (" + node.getValue() + ")";
            PrintWithWS();
            VisitChildren(node);
        }

        /// <summary>
        /// Visits the children of the provided node. Adds indentation to print messages based on level in the tree.
        /// </summary>
        /// <param name="node"></param>
        private void VisitChildren(ExprNode node)
        {
            if (node.IsChildrenNull()) return;
            int childrenCount = node.ChildCount();
            if (childrenCount > 0) { leadingWSCount++; }
            for(int i = 0; i < childrenCount; i++)
            {
                node.Child(i).Visit(this);
            }
            if (childrenCount == 0) { leadingWSCount--; }
        }

        /// <summary>
        /// Prints the contents of the <C>buffer</C> to the Console with leading whitespace based on the value of <c>leadingWSCount</c>.
        /// </summary>
        public void PrintWithWS()
        {
            int padding = leadingWSCount + buffer.Length;
            Console.WriteLine(buffer.PadLeft(padding));
        }

    }
}
