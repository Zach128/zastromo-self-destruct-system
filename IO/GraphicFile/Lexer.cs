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

        //Move ahead one character in the input string
        public void Consume()
        {
            p++;
            if (p >= input.Length) ReachEOF = true;
            else C = input[p];
        }

        public void Match(char x)
        {
            if (C == x) Consume();
            else throw new MatchNotFoundException("Expecting " + x + "; found " + C);
        }

        public abstract Token NextToken();

    }
}
