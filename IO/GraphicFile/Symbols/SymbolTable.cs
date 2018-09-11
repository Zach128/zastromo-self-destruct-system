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

        /// <summary>
        /// Initialise the system symbols in the symbol table.
        /// System symbols will include classes, symbols, functions, system variables, and enums.
        /// </summary>
        /// <param name="enclosingScope">The enclosing scope of the symbol table. Necessary for defining system functions.</param>
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

        /// <summary>
        /// Define a symbol in the symbol table
        /// </summary>
        /// <param name="symbol">The symbol to be added/updated.</param>
        public void Define(Symbol symbol) { symbols[symbol.Name] = symbol; }
        /// <summary>
        /// Define an enum symbol in the symbol table
        /// </summary>
        /// <param name="symbol">The enum to be added/updated.</param>
        public void DefineEnum(Symbol symbol) { enumSymbols[symbol.Name] = symbol; }

        /// <summary>
        /// Attempt to find a symbol by name and return it.
        /// </summary>
        /// <param name="name">The name of the symbol to resolve.</param>
        /// <returns>The symbol of the given name. Null if no symbol was found.</returns>
        public Symbol Resolve(string name)
        {
            Symbol symbol;
            symbols.TryGetValue(name, out symbol);
            if (symbol == null) enumSymbols.TryGetValue(name, out symbol);
            return symbol;
        }

        /// <summary>
        /// Attempt to find an enum symbol by name and return it.
        /// </summary>
        /// <param name="name">The name of the symbol to resolve.</param>
        /// <returns>The symbol of the given name. Null if no symbol was found.</returns>
        public ConstantSymbol ResolveEnum(string name)
        {
            Symbol symbol;
            enumSymbols.TryGetValue(name, out symbol);
            if (symbol is ConstantSymbol) return (ConstantSymbol) symbol;
            else return null;
        }

        /// <summary>
        /// Attempt to find a symbol by name and return its return type.
        /// </summary>
        /// <param name="name">The name of the symbol to resolve.</param>
        /// <returns>The return type of the resolved symbol; null if no symbol is provided.</returns>
        public IType ResolveType(string name)
        {
            Symbol symbol;
            symbols.TryGetValue(name, out symbol);

            //Check the type of the symbol and return the appropriate type
            if (symbol is ClassTypeSymbol) return (ClassTypeSymbol)symbol;
            else if (symbol is SystemTypeSymbol) return (SystemTypeSymbol)symbol;
            else if (symbol is FunctionSymbol || symbol is ArrayType) return symbol.Type;
            else return null;
        }

        /// <summary>
        /// Resolve an array symbol and return the type of the arrays elements.
        /// </summary>
        /// <param name="id">A syntax tree node containing the symbol desired and scope to resolve it from.</param>
        /// <returns></returns>
        [Obsolete("This method is obsolete and pending removal. Replaced by ArrayIndex(string name).", true)]
        public IType ArrayIndex(ZULAst id)
        {
            Symbol s = id.scope.Resolve(id.symbol.Name);
            ArrayType vs = (ArrayType)s;
            id.symbol = vs;
            return vs.ElementType;
        }

        /// <summary>
        /// Resolve an array symbol and return the type of the arrays elements.
        /// </summary>
        /// <param name="name">The name of the array symbol to resolve.</param>
        /// <returns>The type of the elements in the array.</returns>
        public IType ArrayIndex(string name)
        {
            Symbol s = Resolve(name);
            ArrayType vs = (ArrayType)s;
            return vs.ElementType;
        }

        /// <summary>
        /// Resolve a function using the given symbol id and list of arguments for validation.
        /// </summary>
        /// <param name="id">A syntax tree node containing the </param>
        /// <param name="args"></param>
        /// <returns></returns>
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
            return symbols.ToString() + ", " + enumSymbols.ToString();
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
            Define(new VariableSymbol("VIEW_WIDTH", ResolveType("int")));
            Define(new VariableSymbol("VER_HEIGHT", ResolveType("int")));
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
