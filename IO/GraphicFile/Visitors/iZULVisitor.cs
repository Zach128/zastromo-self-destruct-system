using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;

namespace TestWinBackGrnd.IO.GraphicFile.Visitors
{
    public interface IZULVisitor
    {
        
        void Visit(OldArrayNode node);
        void Visit(ArrNode node);
        void Visit(ArrRetNode node);
        void Visit(AssignNode node);
        void Visit(DeclNode node);
        void Visit(FuncNode node);
        void Visit(NameNode node);
        void Visit(NumNode node);
        void Visit(RootNode node);

        //void Visit(ExprNode node);

    }
}
