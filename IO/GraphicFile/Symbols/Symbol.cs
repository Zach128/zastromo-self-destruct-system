using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols
{
    public class Symbol
    {
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

        public override string ToString()
        {
            if (type != null) return "<" + Name + ": " + type + ">";
            return Name;
        }

    }
}
