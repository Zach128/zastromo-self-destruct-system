using System.Collections.Generic;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class SymbolTable
    {

        public static readonly SymbolTable PrimitveSymbols = new SymbolTable();
        public static readonly SymbolTable ClassSymbols = new SymbolTable();

        private readonly Dictionary<string, Symbol> symbols;

        static SymbolTable()
        {
            PrimitveSymbols = new SymbolTable();
            PrimitveSymbols.DefineSystemSymbolTypes();
            ClassSymbols = new SymbolTable();
            ClassSymbols.DefineSystemClassTypes();
        }

        public SymbolTable()
        {
            symbols = new Dictionary<string, Symbol>();
        }

        public void Define(Symbol symbol) { symbols[symbol.Name] = symbol; }
        public Symbol Resolve(string name)
        {
            Symbol symbol;
            symbols.TryGetValue(name, out symbol);
            return symbol;
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
        }

        public void DefineSystemClassTypes()
        {
            Define(new ClassTypeSymbol("line"));
            Define(new ClassTypeSymbol("point"));
            Define(new ClassTypeSymbol("pen"));
        }

    }
}
