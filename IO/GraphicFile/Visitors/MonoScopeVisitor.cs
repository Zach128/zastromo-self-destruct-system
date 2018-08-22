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

            NameNode nameNode = (NameNode) node[ArrRetNode.ARR_NAME];
            Symbol indexSymbol = new Symbol("index", ResolveVarType("int"));
            ZULAst id = new ZULAst(RefLocalSymbol(nameNode.GetName()), symbolTable);
            ZULAst index = new ZULAst(indexSymbol, symbolTable, node[ArrRetNode.INDEX].GetToken());

            Console.WriteLine(symbolTable.ArrayIndex(id, index) + " returned from " + nameNode.GetName() + " array");
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
                ZULAst funcSymbol = new ZULAst(RefFunction(name), symbolTable);
                symbolTable.Call(funcSymbol, node.ArgsToList());
            }
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(NameNode node)
        {

            ExprNode lastNode = nodeStackTrace.Peek();
            if (lastNode.EvalType == NodeType.ARRRET || lastNode.EvalType == NodeType.FUNC)
            {
            }
            else {
                
                Symbol s = RefLocalSymbol(node.GetName());
                if(s != null && (lastNode.EvalType == NodeType.ASSIGN || lastNode.EvalType == NodeType.FUNC))
                    Console.WriteLine("Name reference to " + s.Name + " of type " + s.Type);
                
            }

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
            Symbol symbol;
            if (type.GetName() == "arr")
                symbol = new ArrayType(name, type);
            else
                symbol = new VariableSymbol(name, type);
            symbolTable.Define(symbol);
            Console.WriteLine("Defined new variable " + name);
        }

        public Symbol RefLocalSymbol(string name)
        {
            Symbol symbol = symbolTable.Resolve(name);
            return symbol;
        }

        public Symbol RefFunction(string name)
        {
            Symbol symbol = symbolTable.Resolve(name);
            if(symbol != null) Console.WriteLine("Found function symbol: " + symbol);
            return symbol;
        }

        public IType ResolveVarType(string name)
        {
            IType type = symbolTable.ResolveType(name);
            
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
