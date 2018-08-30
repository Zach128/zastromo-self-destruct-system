using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;

namespace TestWinBackGrnd.IO.GraphicFile.Parsers
{
    /// <summary>
    /// Uses a Lexer to parse its token stream and construct a syntax tree representation.
    /// </summary>
    class GraphicsParser : Parser, ITreeBuilder
    {

        public RootNode root;
        private ExprNode currentNode;

        private string AssignedName = "";


        public GraphicsParser(Lexer input, int k = 3) : base(input, k)
        {
            root = new RootNode();
            currentNode = root;
        }

        /// <summary>
        /// Begin parsing the Tokens present in the input Lexer.
        /// </summary>
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

        /// <summary>
        /// Parse a line of tokens.
        /// </summary>
        public void Statement()
        {
            if (LA(1) == TokenType.NAME && LA(2) == TokenType.NAME)
            {
                Declaration();
            }
            else if (LA(1) == TokenType.NAME && LA(2) == TokenType.COLON)
            {
                AssignedName = LT(1).text;
                Assignment();
                AssignedName = "";
            }
            else if (LA(1) == TokenType.NAME && LA(2) == TokenType.LRBRACK)
            {
                Method();
            }
            else throw new MatchNotFoundException("Error, failed to find valid statement; found " + LT(1) + ", " + LT(2));

            Match(TokenType.SEMICOLON);
        }

        /// <summary>
        /// Parse a number.
        /// </summary>
        /// <param name="errorMsg"></param>
        public void Numeral ()
        {

            TokenType type = LA(1);

            if (type == TokenType.INTEGER) { Match(TokenType.INTEGER); }
            else if (type == TokenType.FLOAT) { Match(TokenType.FLOAT); }
            else if (type == TokenType.RUPE) { Match(TokenType.RUPE); }
            else if (type == TokenType.DECIMAL) { Match(TokenType.DECIMAL); }
            else throw new MatchNotFoundException("Error: Expected int, float or rupe; found " + LT(1));

        }

        /// <summary>
        /// Parse a declaration.
        /// </summary>
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
                    AssignedName = LT(1).text;
                    Assignment();
                    AssignedName = "";
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

        /// <summary>
        /// Parse an assignment.
        /// </summary>
        public void Assignment()
        {

            AssignNode assignNode = new AssignNode(new NameNode(LT(1)));

            currentNode.AddChild(assignNode);
            ExprNode _save = currentNode;
            currentNode = assignNode;

            Token assignedToken = LT(1);

            Match(TokenType.NAME);
            Match(TokenType.COLON);
            Parameter(assignedToken);

            currentNode = root;
        }

        /// <summary>
        /// Parse a method call.
        /// </summary>
        public void Method()
        {

            ExprNode _save;
            ExprNode newNode = new FuncNode(new NameNode(LT(1)));
            currentNode.AddChild(newNode);
            _save = currentNode;
            currentNode = newNode;

            Token assignedToken = LT(1);

            Match(TokenType.NAME);
            Match(TokenType.LRBRACK);
            if(LA(1) != TokenType.RRBRACK)
            {
                Parameter(assignedToken);
                while (LA(1) == TokenType.COMMA)
                {
                    Match(TokenType.COMMA);
                    Parameter(assignedToken);
                }
            }
            Match(TokenType.RRBRACK);

            currentNode = _save;

        }

        /// <summary>
        /// Parse a parameter/value-returning expression.
        /// </summary>
        /// <param name="destination"></param>
        public void Parameter(Token destination)
        {
            ExprNode newNode;
            ExprNode _save;

            if (LA(1) == TokenType.NAME && LA(2) == TokenType.LRBRACK)
            {
                _save = currentNode;
                Method();
            }
            else if (LA(1) == TokenType.NAME && LA(2) == TokenType.LSBRACK)
            {
                newNode = new ArrRetNode(LT(1), new NameNode(LT(1)), new NumNode(LT(3)));
                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
                ArrayAccess();
            }
            else if (LA(1) == TokenType.LCBRACK)
            {
                if(destination != null && destination.type == TokenType.NAME) newNode = new ArrNode(destination);
                else newNode = new ArrNode(LT(1));

                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
                Array();
            }
            else if (LA(1) == TokenType.NAME)
            {
                newNode = new NameNode(LT(1)); //Add child name node
                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
                Match(TokenType.NAME);
            }
            else
            {

                TokenType type = LA(1);
                Token token = LT(1);

                Numeral();

                newNode = new NumNode(token);
                currentNode.AddChild(newNode);
                _save = currentNode;
                currentNode = newNode;
            }

            currentNode = _save;
        }
        
        /// <summary>
        /// Parse a parameter/value-returning expression.
        /// </summary>
        public void Parameter()
        {
            Parameter(null);
        }

        /// <summary>
        /// Parse an array initialiser.
        /// </summary>
        public void Array()
        {
            Match(TokenType.LCBRACK);
            Elements();
            Match(TokenType.RCBRACK);
        }

        /// <summary>
        /// Parse an array of comma-separated elements.
        /// </summary>
        public void Elements()
        {

            Element();
            while (LA(1) == TokenType.COMMA)
            {
                Match(TokenType.COMMA);
                Element();
            }
        }
        
        /// <summary>
        /// Wrapper method for the method Parameter
        /// </summary>
        public void Element()
        {
            Parameter();
        }

        /// <summary>
        /// Parse an array access.
        /// </summary>
        public void ArrayAccess()
        {
            Match(TokenType.NAME);
            Match(TokenType.LSBRACK);
            Match(TokenType.INTEGER);
            Match(TokenType.RSBRACK);
        }

        /// <summary>
        /// Match the next token to an expected token and move forward the stream if consumed.
        /// </summary>
        /// <param name="type">The token type expected to be encountered.</param>
        public override void Match(TokenType type)
        {
            if (LA(1) == type) Consume();
            else throw new MatchNotFoundException("Expecting " + type.ToString() + "; found " + LT(1));
        }
        
    }
}
