using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class SystemType : Symbol, IType
    {

        public SystemType(string name) : base(name) { }

        public string GetName()
        {
            return name;
        }
    }
}
