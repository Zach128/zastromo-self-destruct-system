using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes
{
    public interface IScope
    {
        string ScopeName
        {
            get;
        }             //Get the name of this scope
        IScope EnclosingScope
        {
            get;
        }        //Get the name of the scope enclosing this scope
        void Define(Symbol symbol);     //Define a symbol within this scope
        Symbol Resolve(string name);    //Find a symbol with the given name in this semantic context
    }
}
