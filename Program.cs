using SelfDestructCommons;
using SelfDestructCommons.Model.GraphicsMessages;
using System;
using System.Collections.Generic;
using TestWinBackGrnd.IO.GraphicFile;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Parsers;
using TestWinBackGrnd.IO.GraphicFile.Symbols;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.Visitors;
using TestWinBackGrnd.Properties;
using WinGraphicsController.view;

namespace WinGraphicsController
{
    class Program : SDPipeClient
    {

        private static readonly Lazy<Program> lazy =
        new Lazy<Program>(() => new Program());

        public static Program Instance { get { return lazy.Value; } }

        private Program() : base("GRAPHICS")
        {
        }

        public bool exitAtNextCycle = false;

        WinBackgroundOverride winOverride;

        static void Main(string[] args)
        {

            Console.Write("Loading background override application\n");
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Instance.winOverride = new WinBackgroundOverride();

            //Testing

            //string testDataArrayDec = "arr octPoints : {point(37.5rp, 87.5rp),point(62.5rp, 87.5rp),point(87.5rp, 62.5rp),point(87.5rp, 37.5rp),point(62.5rp, 12.5rp),point(37.5rp, 12.5rp),point(12.5rp, 37.5rp),point(12.5rp, 62.5rp)};";
            //string testDataLineDec = "line line1: line(linesplt(octPoints[0], octPoints[7]), linesplt(octPoints[3], octPoints[4]));";
            //string testDataAssign = "basePen : pen(ORANGE, 10f);";

            GraphicsLexer lexer = new GraphicsLexer(Resources.zastromo_warning_underlay);
            List<Token> tokens = (List<Token>) lexer.GetTokens();
            Token t = tokens[tokens.Count - 1];
            tokens.Remove(t);
            int lineCount = 1;
            int lineTokens = 1;

            while (t.type != TokenType.EOF)
            {
                Console.WriteLine("Token " + lineCount + "." + lineTokens + "\t" + t);
                
                //Increment token/line counts unless line terminator(';') reached.
                if (t.type == TokenType.SEMICOLON)
                {
                    lineCount++;
                    lineTokens = 1;
                }
                else lineTokens++;

                t = tokens[tokens.Count - 1];
                tokens.Remove(t);
            }

            GraphicsLexer parsersLexer = new GraphicsLexer(Resources.zastromo_warning_underlay);
            GraphicsParser parser = new GraphicsParser(parsersLexer);
            parser.Parse();
            ExprNode ast = parser.GetTree();
            PrintTypesVisitor visitor = new PrintTypesVisitor();
            MonoScopeVisitor symbolVisitor = new MonoScopeVisitor();
            ast.Visit(visitor);
            ast.Visit(symbolVisitor);
            SymbolTable symbols = symbolVisitor.GetSymbolTable();

            Console.WriteLine("\nToken printing complete. Press any key to continue...");
            Console.ReadKey();

            /*
            Instance.StartClient();
            Console.Write("Client started.\n");

            //Remain in the loop
            while (!Instance.exitAtNextCycle)
            {
                Console.Write("Waiting for new connection...\n");
                Instance.WaitForConnection();
                Instance.WaitForDisconnection();
            }
            //*/
        }

        public override void ClientDisconnected()
        {
            Console.WriteLine("Server disconnected\n");
        }

        public override BKG_RESPONSE ProcessMessage(BackgroundCtrlMsg message)
        {
            Console.WriteLine("Received message of type " + message.BkgAction.ToString());

            //Parse the input message
            switch (message.BkgAction)
            {
                case BKG_ACTION.FILL_COLOR:

                    if (message is BkgFillColor)
                    {
                        winOverride.FillWithColor(
                            ((BkgFillColor)message).FillColor);
                        return BKG_RESPONSE.MSG_OK;
                    } else
                    {
                        return BKG_RESPONSE.ERR_ARGS;
                    }
                case BKG_ACTION.SHW_WRN:
                    try
                    {
                        winOverride.DrawWarningOnBkg();
                        return BKG_RESPONSE.MSG_OK;
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.StackTrace);
                        return BKG_RESPONSE.CTRL_FAIL;
                    }
                case BKG_ACTION.CLEAR_BKG:
                    winOverride.ClearBackground();
                    return BKG_RESPONSE.MSG_OK;
                case BKG_ACTION.EMG_SHDN:
                    Instance.exitAtNextCycle = true;
                    Environment.Exit(0);
                    return BKG_RESPONSE.NO_RESPONSE;
                default:
                    return BKG_RESPONSE.ERR_FMT;
            }

        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("Shutting down...\n");
            Instance.StopClient();
        }

    }
}
