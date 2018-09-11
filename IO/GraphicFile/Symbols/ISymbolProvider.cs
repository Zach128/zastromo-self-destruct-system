using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public interface ISymbolProvider
    {
        SymbolTable GetSymbolTable();
    }
}
