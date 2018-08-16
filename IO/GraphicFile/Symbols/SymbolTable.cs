using System;
using System.Collections.Generic;
using TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class SymbolTable
    {

        //public static readonly SymbolTable PrimitveSymbols = new SymbolTable();
        //public static readonly SymbolTable ClassSymbols = new SymbolTable();

        private readonly Dictionary<string, Symbol> symbols;

        static SymbolTable()
        {
            //PrimitveSymbols = new SymbolTable();
            //PrimitveSymbols.DefineSystemSymbolTypes();
            //ClassSymbols = new SymbolTable();
            //ClassSymbols.DefineSystemClassTypes();
        }

        public SymbolTable(IScope enclosingScope)
        {
            symbols = new Dictionary<string, Symbol>();
        }

        public void InitTypes(IScope enclosingScope)
        {
            if (enclosingScope != null)
            {
                DefineSystemClassTypes();
                DefineSystemSymbolTypes();
                DefineSystemFunctions(enclosingScope);
                DefineSystemVariables();
            }
        }

        public void Define(Symbol symbol) { symbols[symbol.Name] = symbol; }
        public Symbol Resolve(string name)
        {
            Symbol symbol;
            symbols.TryGetValue(name, out symbol);
            return symbol;
        }

        public IType ResolveType(string name)
        {
            Symbol symbol;
            symbols.TryGetValue(name, out symbol);
            string type = symbol.GetType().ToString();
            if (symbol is ClassTypeSymbol) return (ClassTypeSymbol)symbol;
            else if (symbol is SystemTypeSymbol) return (SystemTypeSymbol)symbol;
            else if (symbol is FunctionSymbol) return symbol.Type;
            else return null;
        }

        public IType ArrayIndex(ZULAst id, ZULAst index)
        {
            Symbol s = id.scope.Resolve(id.symbol.Name);
            VariableSymbol vs = (VariableSymbol)s;
            id.symbol = vs;
            return vs.Type;
        }

        public IType Call(ZULAst id, List<ExprNode> args)
        {
            Symbol s = id.scope.Resolve(id.symbol.Name);
            FunctionSymbol fs = (FunctionSymbol)s;
            id.symbol = fs;
            return fs.Type;
        }

        public override string ToString()
        {
            return symbols.ToString();
        }

        public void DefineSystemSymbolTypes()
        {
            Define(new SystemTypeSymbol("int"));
            Define(new SystemTypeSymbol("decimal"));
            Define(new SystemTypeSymbol("float"));
            Define(new SystemTypeSymbol("rupe"));
            Define(new SystemTypeSymbol("arr"));
            Define(new SystemTypeSymbol("void"));
            Define(new SystemTypeSymbol("string"));
        }

        public void DefineSystemClassTypes()
        {
            Define(new ClassTypeSymbol("line"));
            Define(new ClassTypeSymbol("point"));
            Define(new ClassTypeSymbol("pen"));
        }

        private void DefineSystemFunctions(IScope enclosingScope)
        {
            Define(new FunctionSymbol("line", ResolveType("line"), enclosingScope));
            Define(new FunctionSymbol("linesplt", ResolveType("point"), enclosingScope));
            Define(new FunctionSymbol("point", ResolveType("point"), enclosingScope));
            Define(new FunctionSymbol("pen", ResolveType("pen"), enclosingScope));
            Define(new FunctionSymbol("draw", ResolveType("void"), enclosingScope));
            Define(new FunctionSymbol("drawPoly", ResolveType("void"), enclosingScope));
        }

        private void DefineSystemVariables()
        {
            Define(new VariableSymbol("VER", ResolveType("decimal")));
            Define(new VariableSymbol("renderMode", ResolveType("string")));
            Define(new VariableSymbol("basePen", ResolveType("pen")));
        }

    }
}
