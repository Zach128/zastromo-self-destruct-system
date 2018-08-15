using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;

namespace TestWinBackGrnd.IO.GraphicFile.Parsers
{
    class GraphicsParser : Parser, ITreeBuilder
    {

        public RootNode root;
        private ExprNode currentNode;

        public GraphicsParser(Lexer input, int k = 3) : base(input, k)
        {
            root = new RootNode();
            currentNode = root;
        }

        public override void Parse()
        {
            while(LA(1) != TokenType.EOF)
            {
                ExprNode _save = currentNode;
                currentNode = root;
                Statement();
                currentNode = _save;
            }
        }

        public ExprNode GetTree()
        {
            return root;
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

        public void Numeral (string errorMsg = "Error: Expected int, float or rupe;")
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
            if (token.type == TokenType.NAME && LA(2) == TokenType.NAME)
            {

                DeclNode decl = new DeclNode(new NameNode(token), new NameNode(LT(2)));
                currentNode.AddChild(decl);

                ExprNode _save = currentNode;
                currentNode = decl;

                Match(TokenType.NAME);

                if (LA(1) == TokenType.NAME && LA(2) == TokenType.COLON)
                {
                    Assignment();
                }
                else {
                    Match(TokenType.SEMICOLON);
                }
                currentNode = _save;
                //if (LA(1) == TokenType.LCBRACK) { Array(); }
                //else if (LA(1) == TokenType.NAME && LA(2) == TokenType.LRBRACK) { Method(); }
            }
            else throw new MatchNotFoundException("Expected TYPE and NAME; found " + LT(1).text + ", and " + LT(2).text);
        }

        #endregion

        #region Assignment matchers

        public void Assignment()
        {

            AssignNode assignNode = new AssignNode(new NameNode(LT(1)));

            currentNode.AddChild(assignNode);
            ExprNode _save = currentNode;
            currentNode = assignNode;

            Match(TokenType.NAME);
            Match(TokenType.COLON);
            Parameter();

            currentNode = root;

            //Console.WriteLine("Found Assignment");
        }

        #endregion

        #region Method matchers
        
        public void Method()
        {

            ExprNode _save;
            ExprNode newNode = new FuncNode(new NameNode(LT(1)));
            currentNode.AddChild(newNode);
            _save = currentNode;
            currentNode = newNode;

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

            currentNode = _save;

        }

        public void Parameter()
        {
            ExprNode newNode;
            ExprNode _save;

            if (LA(1) == TokenType.NAME && LA(2) == TokenType.LRBRACK) {
                _save = currentNode;
                Method();
            }
            else if (LA(1) == TokenType.NAME && LA(2) == TokenType.LSBRACK) {
                newNode = new ArrRetNode(LT(1), new NameNode(LT(1)), new NumNode(LT(3)));
                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
                ArrayAccess();
            }
            else if (LA(1) == TokenType.LCBRACK) {
                newNode = new ArrNode(LT(1));
                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
                Array();
            }
            else if (LA(1) == TokenType.NAME) {
                newNode = new NameNode(LT(1)); //Add child name node
                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
                Match(TokenType.NAME);
            }
            else {

                TokenType type = LA(1);
                Token token = LT(1);

                if (type == TokenType.INTEGER) { Match(TokenType.INTEGER); }
                else if (type == TokenType.FLOAT) { Match(TokenType.FLOAT); }
                else if (type == TokenType.RUPE) { Match(TokenType.RUPE); }
                else if (type == TokenType.DECIMAL) { Match(TokenType.DECIMAL); }
                else throw new MatchNotFoundException("Error in parameter parsing: Expected int, float or rupe; found " + LT(1));

                newNode = new NumNode(token);
                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
            }

            currentNode = _save;

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
            Parameter();
        }

        public void ArrayAccess()
        {
            Match(TokenType.NAME);
            Match(TokenType.LSBRACK);
            Match(TokenType.INTEGER);
            Match(TokenType.RSBRACK);
            //Console.WriteLine("Found array access");
        }

        public override void Match(TokenType type)
        {
            if (LA(1) == type) Consume();
            else throw new MatchNotFoundException("Expecting " + type.ToString() + "; found " + LT(1));
        }

        #endregion

    }
}
