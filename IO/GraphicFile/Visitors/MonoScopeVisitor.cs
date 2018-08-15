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
    class MonoScopeVisitor : IZULVisitor, ISymbolProvider
    {

        MonolithicSymbolTable symbolTable;

        Stack<ExprNode> nodeStackTrace;

        public MonoScopeVisitor()
        {
            symbolTable = new MonolithicSymbolTable();
            nodeStackTrace = new Stack<ExprNode>();
        }

        public void Visit(OldArrayNode node)
        {
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(ArrNode node)
        {
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(ArrRetNode node)
        {
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(AssignNode node)
        {
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(DeclNode node)
        {
            if (node[DeclNode.DEF] is NameNode)
            {
                string name = ((NameNode)node[DeclNode.DEF]).GetName();
                NodeType declType = ((ExprNode)node[DeclNode.TYPE_DEF]).EvalType;
                IType symType = ResolveVarType(node[DeclNode.TYPE_DEF].GetTokenName());
                DefineLocalSymbol(name, symType);
            }
            VisitChildren(node);
        }

        public void Visit(FuncNode node)
        {
            if (node[FuncNode.FUNC_NAME] is NameNode)
            {
                string name = ((NameNode)node[FuncNode.FUNC_NAME]).GetName();
            }
            VisitChildren(node);
        }

        public void Visit(NameNode node)
        {
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(NumNode node)
        {
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(RootNode node)
        {
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void DefineLocalSymbol(string name, IType type)
        {
            VariableSymbol variableSymbol = new VariableSymbol(name, type);
            symbolTable.Define(variableSymbol);
            Console.WriteLine("Defined new variable " + name);
        }

        public IType ResolveVarType(string name)
        {
            IType type = (IType) SymbolTable.PrimitveSymbols.Resolve(name);
            if (type == null) type = (IType) SymbolTable.ClassSymbols.Resolve(name); ;
            return type;
        }

        private void PushToTrace(ExprNode node)
        {
            nodeStackTrace.Push(node);
        }

        private ExprNode PopFromTrace()
        {
            return nodeStackTrace.Pop();
        }

        private int GetTraceLevel()
        {
            return nodeStackTrace.Count();
        }

        public SymbolTable GetSymbolTable()
        {
            return symbolTable;
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
