using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class ArrayType : Symbol
    {

        public IType ElementType;
        public readonly List<object> Elements;

        public ArrayType(string name, IType type, IType elementType, List<object> elms) : base(name, type)
        {
            ElementType = elementType;
            Elements = elms;
            SymbolType = ARRAY;
        }

        public ArrayType(string name, IType type, IType elementType) : base(name, type)
        {
            ElementType = elementType;
            Elements = new List<object>();
            SymbolType = ARRAY;
        }

        public ArrayType(string name, IType type) : base(name, type) {
            ElementType = type;
            Elements = new List<object>();
            SymbolType = ARRAY;
        }

    }
}
