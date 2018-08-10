using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees
{
    public enum NodeType
    {
        INVALID, //Invalid result
        INTEGER, //Integer result
        FLOAT, //Float result
        DECIMAL, //Decimal result
        RUPE, //RuPe result
        ARR, //Array result
        ARRRET, //Array return result
        STRING,
        NAME,
        ASSIGN,
        DECL,
        FUNC
    }
}
