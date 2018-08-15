using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Symbols;
using TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;

namespace TestWinBackGrnd.IO.GraphicFile.Visitors
{
    class SymbolScopeVisitor : IZULVisitor
    {

        private IScope _save;
        private IScope currentScope;

        public SymbolScopeVisitor()
        {

            currentScope = new GlobalScope();
        }

        public void Visit(OldArrayNode node)
        {
            VisitChildren(node);
        }

        public void Visit(ArrNode node)
        {
            EnterArrayBlock("array");
            VisitChildren(node);
            ExitBlock();
        }

        public void Visit(ArrRetNode node)
        {
            VisitChildren(node);
        }

        public void Visit(AssignNode node)
        {
            VisitChildren(node);
        }

        public void Visit(DeclNode node)
        {
            if (node[DeclNode.DEF] is NameNode)
            {
                string name = ((NameNode) node[DeclNode.DEF]).GetName();
                DefineLocalSymbol(name, new SystemTypeSymbol(name));
            }
            VisitChildren(node);
        }

        public void Visit(FuncNode node)
        {
            if(node[FuncNode.FUNC_NAME] is NameNode)
            {
                string name = ((NameNode)node[FuncNode.FUNC_NAME]).GetName();
                EnterFunction(name, new SystemTypeSymbol(name));
            }
            VisitChildren(node);
            ExitFunction();
        }

        public void Visit(NameNode node)
        {
            VisitChildren(node);
        }

        public void Visit(NumNode node)
        {
            VisitChildren(node);
        }

        public void Visit(RootNode node)
        {
            VisitChildren(node);
        }

        public void EnterBlock(string scopeName)
        {
            currentScope = new LocalScope(scopeName, currentScope);
        }

        public void EnterArrayBlock(string arrayName)
        {
            currentScope = new ArrayScope(arrayName, currentScope);
        }

        public void ExitBlock()
        {
            if(!(currentScope is GlobalScope)) currentScope = currentScope.EnclosingScope;
        }

        public void EnterFunction(string name, IType returnType)
        {
            FunctionSymbol functionSymbol = new FunctionSymbol(name, returnType, currentScope);
            currentScope.Define(functionSymbol);
            currentScope = functionSymbol;
            Console.WriteLine("Defined function call " + name);
        }

        public void ExitFunction()
        {
            Console.WriteLine("Exited current scope " + currentScope.ScopeName);
            if(!(currentScope is GlobalScope)) currentScope = currentScope.EnclosingScope;
        }

        public void DefineLocalSymbol(string name, IType type)
        {
            VariableSymbol variableSymbol = new VariableSymbol(name, type);
            currentScope.Define(variableSymbol);
            Console.WriteLine("Defined new variable " + name);
        }

        /// <summary>
        /// Visits the children of the provided node. Adds indentation to print messages based on level in the tree.
        /// </summary>
        /// <param name="node"></param>
        private void VisitChildren(ExprNode node)
        {
            if (node.IsChildrenNull()) return;
            int childrenCount = node.ChildCount();
            for (int i = 0; i < childrenCount; i++)
            {
                node.Child(i).Visit(this);
            }
        }

    }
}
