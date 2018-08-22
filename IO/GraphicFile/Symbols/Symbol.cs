using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class Symbol
    {

        public const int NA = 0;
        public const int PRIMITIVE = 1;
        public const int OBJECT = 2;
        public const int FUNCTION_REF = 3;
        public const int PRIMITIVE_T = 4;
        public const int ARRAY = 5;
        public const int CONSTANT = 6;

        protected readonly string name;
        private readonly IType type;

        public Symbol(string name)
        {
            this.name = name;
            
        }

        public Symbol(string name, IType type)
        {
            this.name = name;
            this.type = type;
            
        }

        public string Name => name;
        public IType Type => type;
        public int SymbolType = NA;

        public override string ToString()
        {
            if (type != null) return "<" + Name + ": " + type + ">";
            return Name;
        }

    }
}
