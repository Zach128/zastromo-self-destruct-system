using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile
{
    public enum TokenType
    {
        EOF = 0,    // 'EOF character sequence'
        LRBRACK,    // '('
        RRBRACK,    // ')'
        LCBRACK,    // '{'
        RCBRACK,    // '}'
        LSBRACK,    // '['
        RSBRACK,    // ']'
        SEMICOLON,  // ';'
        COLON,      // ':'
        COMMA,      // ','
        SLASH,      // '/'
        NAME,       // 'ANYSTRING'
        INTEGER,    // '1234'
        DECIMAL,    // '12.5'
        FLOAT,      // '12.5f'
        RUPE        // '12.5rp'
    }
}
