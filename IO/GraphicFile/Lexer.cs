using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile
{
    public abstract class Lexer
    {
        public const int EOF_TYPE = 0;
        string input;   //Input string
        int p = 0;      //Character pointer for string
        public char C { get; protected set; }         //

        public bool ReachEOF { get; private set; } = false;

        public Lexer(string input)
        {
            this.input = input ?? throw new ArgumentNullException(nameof(input));
            C = input[p];
        }

        /// <summary>
        /// Move ahead one character in the input string.
        /// </summary>
        public void Consume()
        {
            p++;
            if (p >= input.Length) ReachEOF = true;
            else C = input[p];
        }

        /// <summary>
        /// Match the expected character to the current lookahead and consume it.
        /// </summary>
        /// <param name="x"></param>
        public void Match(char x)
        {
            if (C == x) Consume();
            else throw new MatchNotFoundException("Expecting " + x + "; found " + C);
        }

        public void Reset()
        {
            p = 0;
            C = input[p];
        }

        public abstract Token NextToken();

    }
}
