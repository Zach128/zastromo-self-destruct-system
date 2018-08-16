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

        public ArrayType(string name, IType type, IType elementType) : base(name, type)
        {
            ElementType = elementType;
            SymbolType = ARRAY;
        }
        public ArrayType(string name, IType type) : base(name, type) {
            ElementType = type;
            SymbolType = ARRAY;
        }

    }
}
