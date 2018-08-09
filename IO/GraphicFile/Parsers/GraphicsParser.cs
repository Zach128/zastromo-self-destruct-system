using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.Parsers
{
    class GraphicsParser : Parser
    {
        public GraphicsParser(Lexer input, int k) : base(input, k)
        {
        }

        public override void Parse()
        {
            while(LA(1) != TokenType.EOF)
            {
                Statement();
            }
        }

        #region Grammar Parsing methods

        public void Statement()
        {
            if (LA(1) == TokenType.NAME && LA(2) == TokenType.NAME)
            {
                //Console.WriteLine("Found declaration");
                Declaration();
            }
            else if (LA(1) == TokenType.NAME && LA(2) == TokenType.COLON)
            {
                Assignment();
            }
            else if (LA(1) == TokenType.NAME && LA(2) == TokenType.LRBRACK)
            {
                Method();
            }
            else throw new MatchNotFoundException("Error, failed to find valid statement; found " + LT(1) + ", " + LT(2));
            Match(TokenType.SEMICOLON);
        }

        #endregion

        #region Variable matchers

        public void Numeral(string errorMsg = "Error: Expected int, float or rupe;")
        {

            TokenType type = LA(1);

            if (type == TokenType.INTEGER) { Match(TokenType.INTEGER); }
            else if (type == TokenType.FLOAT) { Match(TokenType.FLOAT); }
            else if (type == TokenType.RUPE) { Match(TokenType.RUPE); }
            else if (type == TokenType.DECIMAL) { Match(TokenType.DECIMAL); }
            else throw new MatchNotFoundException(errorMsg + " Found " + LT(1));

        }

        #endregion

        #region Declaration matchers

        public void Declaration()
        {
            Token token = LT(1);
            if(token.type == TokenType.NAME && LA(2) == TokenType.NAME)
            {
                Match(TokenType.NAME);
                Match(TokenType.NAME);
                Match(TokenType.COLON);
                if (LA(1) == TokenType.LCBRACK) { Array(); }
                else if (LA(1) == TokenType.NAME && LA(2) == TokenType.LRBRACK) { Method(); }
            }
        }

        #endregion

        #region Assignment matchers

        public void Assignment()
        {
            Match(TokenType.NAME);
            Match(TokenType.COLON);
            Parameter();
            //Console.WriteLine("Found Assignment");
        }

        #endregion

        #region Constructor matchers

        /*
        public void PointConstructor()
        {
            BasicConstructor(2);
        }

        public void LineConstructor()
        {
            BasicConstructor(2);
            Console.WriteLine("Found line constructor");
        }

        public void BasicConstructor(int parameterChecks = 0)
        {
            Match(TokenType.NAME);
            Match(TokenType.LRBRACK);

            if (parameterChecks > 0)
            {
                Parameter();
            }
            for(int i = 1; i < parameterChecks; i++)
            {
                Match(TokenType.COMMA);
                Parameter();
            }

            Match(TokenType.RRBRACK);
        }
        //*/
        #endregion

        #region Method matchers
        
        public void Method()
        {
            Match(TokenType.NAME);
            Match(TokenType.LRBRACK);
            if(LA(1) != TokenType.RRBRACK)
            {
                Parameter();
                //Console.WriteLine("Found param");
                while (LA(1) == TokenType.COMMA)
                {
                    Match(TokenType.COMMA);
                    Parameter();
                    //Console.WriteLine("Found param");
                }
            }
            Match(TokenType.RRBRACK);
            //Console.WriteLine("Found method");
        }

        public void Parameter()
        {
            if (LA(1) == TokenType.NAME && LA(2) == TokenType.LRBRACK) { Method(); }
            else if (LA(1) == TokenType.NAME && LA(2) == TokenType.LSBRACK) { ArrayAccess(); }
            else if (LA(1) == TokenType.NAME) { Match(TokenType.NAME); }
            else { Numeral("Error in Point constructor: Expected int, float or rupe;"); }
        }

        #endregion

        #region Array matchers

        public void Array()
        {
            Match(TokenType.LCBRACK);
            Elements();
            Match(TokenType.RCBRACK);
        }

        public void Elements()
        {
            Element();
            while (LA(1) == TokenType.COMMA)
            {
                Match(TokenType.COMMA);
                Element();
            }
        }
        
        public void Element()
        {
            TokenType type = LA(1);
            if (type == TokenType.NAME && LA(2) == TokenType.LRBRACK) { Method(); }
            else if (type == TokenType.LCBRACK) { Array(); }
            else if (type == TokenType.NAME && LA(2) == TokenType.LRBRACK)
            {
                Match(TokenType.NAME);
            }
            else throw new MatchNotFoundException("Error in Array parsing: Expected name, list or constructor; found " + LT(1));
        }

        public void ArrayAccess()
        {
            Match(TokenType.NAME);
            Match(TokenType.LSBRACK);
            Match(TokenType.INTEGER);
            Match(TokenType.RSBRACK);
            //Console.WriteLine("Found array access");
        }

        #endregion

    }
}
