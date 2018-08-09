using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.Parsers
{
    /// <summary>
    /// LL(k) parser for ZUL implementation.
    /// </summary>
    public abstract class Parser
    {

        Lexer input;
        Token[] lookahead;
        readonly int k;     //Defines size of lookahead buffer.
        int p = 0;          //Circular index for lookahead array.

        public Parser(Lexer input, int k)
        {
            this.input = input;
            this.k = k;
            lookahead = new Token[k];
            for (int i = 1; i <= k; i++) Consume();
        }

        public void Consume()
        {
            lookahead[p] = input.NextToken();
            p = (p + 1) % k;
        }

        public Token LT(int i)
        {
            return lookahead[(p + i - 1) % k]; //Circular accessing of lookahead buffer
        }

        /// <summary>
        /// Returns the type of the token stored in <code>lookahead</code> at index i.
        /// </summary>
        /// <param name="i">The index of the token to look at.</param>
        /// <returns>The token type of the requested token.</returns>
        public TokenType LA(int i)
        {
            return LT(i).type;
        }

        public void Match(TokenType x)
        {
            if (LA(1) == x) Consume();
            else throw new MatchNotFoundException("Expecting " + x.ToString() + "; found " + LT(1));
        }

        public void DumpAsLines()
        {
            string line = string.Empty;
            do
            {
                line += LT(1).text;
                if (LA(1) == TokenType.SEMICOLON)
                {
                    Console.WriteLine(line);
                    line = "";
                }
                Consume();
            } while (LT(1).type != TokenType.EOF);
        }

        public abstract void Parse();

    }
}
