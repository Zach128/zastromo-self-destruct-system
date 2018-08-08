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
        INTEGER,    // '12'
        DECIMAL,    // '12.5'
        FLOAT,      // '12.5f'
        RUPE        // '12.5rp'
    }
}