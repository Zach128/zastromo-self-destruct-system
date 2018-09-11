using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile
{
    public class GraphicsLexer : Lexer
    {

        public GraphicsLexer(string input) : base(input)
        {
        }
        
        /// <summary>
        /// Process and return a list of tokens based on the value of <code>input</code>.
        /// </summary>
        /// <returns>A collection of tokens from the processed string.</returns>
        public IList<Token> GetTokens()
        {
            Stack<Token> tokens = new Stack<Token>();

            do
            {
                tokens.Push(NextToken());
            } while (tokens.Peek().type != TokenType.EOF);

            return tokens.ToList();
        }

        //Process and return the next token
        public override Token NextToken()
        {
            while (ReachEOF != true)
            {
                switch (C)
                {
                    case ' ': case '\t': case '\n': case '\r': WS(); continue;
                    case '/': COMMENT(); continue;
                    case ',': Consume(); return new Token(TokenType.COMMA, ",");
                    case '(': Consume(); return new Token(TokenType.LRBRACK, "(");
                    case ')': Consume(); return new Token(TokenType.RRBRACK, ")");
                    case '[': Consume(); return new Token(TokenType.LSBRACK, "[");
                    case ']': Consume(); return new Token(TokenType.RSBRACK, "]");
                    case '{': Consume(); return new Token(TokenType.LCBRACK, "{");
                    case '}': Consume(); return new Token(TokenType.RCBRACK, "}");
                    case ';': Consume(); return new Token(TokenType.SEMICOLON, ";");
                    case ':': Consume(); return new Token(TokenType.COLON, ":");
                    default:
                        if (IsLetter() || C == '_') return NAME();
                        else if (IsNumber()) return NUMBER();
                        throw new MatchNotFoundException("Invalid character: " + C);
                }
            }
            return new Token(TokenType.EOF, "<EOF>");
        }

        private bool IsLetter() => C >= 'a' && C <= 'z' || C >= 'A' && C <= 'Z';
        private bool IsNumber() => C >= '0' && C <= '9';
        private bool IsOtherChar() => C == '-' || C == '_';
        private bool IsWhiteSpace() => C == ' ' || C == '\n' || C == '\t' || C == '\r';

        private Token NUMBER()
        {
            StringBuilder buffer = new StringBuilder();
            //Get all digits in the number
            do
            {
                buffer.Append(C);
                Consume();
            } while (IsNumber() || C == '.');

            bool isDecimal = buffer.ToString().Contains('.');

            //If there's no characters after it
            if (!IsLetter() && isDecimal) return new Token(TokenType.DECIMAL, buffer.ToString());

            //Check if it's a float
            if (C == 'f') {
                buffer.Append(C);
                Consume();
                return new Token(TokenType.FLOAT, buffer.ToString());
            }
            //Check if it's a RuPe
            else if (C == 'r')
            {
                //If its followed by an r, append it and check for a p
                buffer.Append(C);
                Consume();
                if (C == 'p') {
                    buffer.Append(C);
                    Consume();
                    return new Token(TokenType.RUPE, buffer.ToString());
                }
                else throw new FormatException("Invalid number format '" + buffer.ToString() + "'; expected" + buffer.Append('p').ToString());
            }
            return new Token(TokenType.INTEGER, buffer.ToString());
        }

        //Eliminate comment from buffer
        private void COMMENT()
        {
            Consume();

            //In-line comments
            if (C == '/')
                do Consume(); while (C != '\n');
            else if (C == '*')
            {
                do
                {
                    Consume();
                    do Consume(); while (C != '*');
                    Consume();
                } while (C != '/');
                Consume();
            }
            else throw new FormatException("Invalid comment format; expected '*' or '/'. Found '" + C + "'");
        }

        //Get string name from buffer
        private Token NAME()
        {
            StringBuilder buffer = new StringBuilder();
            do
            {
                buffer.Append(C);
                Consume();
            } while (IsLetter() || IsNumber() || IsOtherChar());
            return new Token(TokenType.NAME, buffer.ToString());
        }

        //Eliminate whitespace from buffer
        private void WS()
        {
            while (IsWhiteSpace() && !ReachEOF) Consume() ;
        }
    }
}
