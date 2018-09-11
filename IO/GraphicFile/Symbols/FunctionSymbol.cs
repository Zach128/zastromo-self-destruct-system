using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class FunctionSymbol : Symbol, IScope
    {

        protected readonly SymbolTable symbols;
        public readonly int ArgCount;
        protected readonly List<object> args;
        protected readonly IScope enclosingScope;

        public FunctionSymbol(string name, IType returnType, IScope enclosingScope) : base(name, returnType)
        {
            symbols = new SymbolTable(this);
            SymbolType = FUNCTION_REF;
            args = new List<object>(0);
            ArgCount = 0;
        }

        public FunctionSymbol(string name, IType returnType, int argCount, IScope enclosingScope) : base(name, returnType)
        {
            symbols = new SymbolTable(this);
            SymbolType = FUNCTION_REF;
            ArgCount = argCount;
            args = new List<object>(argCount);
        }

        public FunctionSymbol(string name, IType returnType, List<object> args, IScope enclosingScope) : base(name, returnType)
        {
            symbols = new SymbolTable(this);
            SymbolType = FUNCTION_REF;
            this.args = args;
        }

        public void PushArg(object arg) { args.Add(arg); }
        public bool PopArg(object arg) { return args.Remove(arg); }
        public object ArgAt(int index)
        {
            if (index >= 0 && index < args.Count) return args[index];
            else throw new IndexOutOfRangeException();
        }

        public string ScopeName { get; }

        public IScope EnclosingScope { get; }

        public void Define(Symbol symbol)
        {
            symbols.Define(symbol);
        }

        public Symbol Resolve(string name)
        {
            Symbol symbol = symbols.Resolve(name);
            if (symbol != null) return symbol;
            if (EnclosingScope != null) return EnclosingScope.Resolve(name);
            return null;
        }

        public void ClearArgs()
        {
            args.Clear();
        }
    }
}
