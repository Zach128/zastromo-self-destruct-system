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
            Console.WriteLine(symbolTable.ArrayIndex(nameNode.GetName()) + " returned from " + nameNode.GetName() + " array");
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
            PushToTrace(node);
            VisitChildren(node);
            PopFromTrace();
        }

        public void Visit(NameNode node)
        {

            //Check if the previous node was an array access or function node. If so, do nothing
            ExprNode lastNode = nodeStackTrace.Peek();
            if (lastNode.EvalType == NodeType.ARRRET || lastNode.EvalType == NodeType.FUNC)
            {
            }
            else {
                
                //Output the symbol referenced by the node
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

        /// <summary>
        /// Define a new symbol in the symbol table.
        /// </summary>
        /// <param name="name">The name of the new symbol.</param>
        /// <param name="type">The return type of the symbol.</param>
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

        /// <summary>
        /// Wrapper method for SymbolTable.Resolve.
        /// Resolves a variable-type symbol from the symbol table.
        /// </summary>
        /// <param name="name">The name of the symbol to be resolved.</param>
        /// <returns>The returned symbol from SymbolTable.Resolve</returns>
        public Symbol RefLocalSymbol(string name)
        {
            Symbol symbol = symbolTable.Resolve(name);
            return symbol;
        }

        /// <summary>
        /// Wrapper method for SymbolTable.Resolve.
        /// Resolves a function symbol from the symbol table.
        /// </summary>
        /// <param name="name">The name of the symbol to resolve.</param>
        /// <returns>The function symbol returned, abstracted as a Symbol object.</returns>
        public Symbol RefFunction(string name)
        {
            Symbol symbol = symbolTable.Resolve(name);
            if(symbol != null) Console.WriteLine("Found function symbol: " + symbol);
            return symbol;
        }

        /// <summary>
        /// Resolve a type symbol from the symbol table.
        /// </summary>
        /// <param name="name">The name of the type to resolve.</param>
        /// <returns>A symbol of type IType depending on what was found in the symbol table.</returns>
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
        /// <param name="node">The parent node to visit.</param>
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
