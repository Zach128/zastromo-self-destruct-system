using System;
using System.Collections.Generic;
using TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class SymbolTable
    {

        private readonly Dictionary<string, Symbol> symbols;
        private readonly Dictionary<string, Symbol> enumSymbols;

        public SymbolTable(IScope enclosingScope)
        {
            symbols = new Dictionary<string, Symbol>();
            enumSymbols = new Dictionary<string, Symbol>();
        }

        public void InitTypes(IScope enclosingScope)
        {
            if (enclosingScope != null)
            {
                DefineSystemClassTypes();
                DefineSystemSymbolTypes();
                DefineSystemFunctions(enclosingScope);
                DefineSystemVariables();
                DefineEnums();
            }
        }

        public void Define(Symbol symbol) { symbols[symbol.Name] = symbol; }
        public void DefineEnum(Symbol symbol) { enumSymbols[symbol.Name] = symbol; }

        public Symbol Resolve(string name)
        {
            Symbol symbol;
            symbols.TryGetValue(name, out symbol);
            if (symbol == null) enumSymbols.TryGetValue(name, out symbol);
            return symbol;
        }

        public ConstantSymbol ResolveEnum(string name)
        {
            Symbol symbol;
            enumSymbols.TryGetValue(name, out symbol);
            if (symbol is ConstantSymbol) return (ConstantSymbol) symbol;
            else return null;
        }

        public IType ResolveType(string name)
        {
            Symbol symbol;
            symbols.TryGetValue(name, out symbol);
            string type = symbol.GetType().ToString();
            if (symbol is ClassTypeSymbol) return (ClassTypeSymbol)symbol;
            else if (symbol is SystemTypeSymbol) return (SystemTypeSymbol)symbol;
            else if (symbol is FunctionSymbol) return symbol.Type;
            else if (symbol is ArrayType) return symbol.Type;
            else return null;
        }

        public IType ArrayIndex(ZULAst id, ZULAst index)
        {
            Symbol s = id.scope.Resolve(id.symbol.Name);
            ArrayType vs = (ArrayType)s;
            id.symbol = vs;
            return vs.ElementType;
        }

        public FunctionSymbol Call(ZULAst id, List<ExprNode> args)
        {
            FunctionSymbol fs = (FunctionSymbol)id.scope.Resolve(id.symbol.Name);
            id.symbol = fs;
            if(fs.ArgCount == args.Count)
            {
                return fs;
            }
            return null;
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
            Define(new FunctionSymbol("line", ResolveType("line"), 2, enclosingScope));
            Define(new FunctionSymbol("linesplt", ResolveType("point"), 2, enclosingScope));
            Define(new FunctionSymbol("point", ResolveType("point"), 2, enclosingScope));
            Define(new FunctionSymbol("pen", ResolveType("pen"), 2, enclosingScope));
            Define(new FunctionSymbol("draw", ResolveType("void"), 1, enclosingScope));
            Define(new FunctionSymbol("drawPoly", ResolveType("void"), 1, enclosingScope));
        }

        private void DefineSystemVariables()
        {
            Define(new VariableSymbol("VER", ResolveType("decimal")));
            Define(new VariableSymbol("renderMode", ResolveType("string")));
            Define(new VariableSymbol("basePen", ResolveType("pen")));
        }

        private void DefineEnums()
        {
            DefineEnum(new ConstantSymbol("ORANGE", ResolveType("string"), "#ff6c00"));
            DefineEnum(new ConstantSymbol("ORANGE_RED", ResolveType("string"), "#ff3300"));
        }

    }
}
