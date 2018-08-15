using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class SystemTypeSymbol : Symbol, IType
    {

        public SystemTypeSymbol(string name) : base(name) { }

        public string GetName()
        {
            return name;
        }
    }
}
