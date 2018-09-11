using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile;
using TestWinBackGrnd.IO.GraphicFile.Exceptions;
using TestWinBackGrnd.IO.GraphicFile.Interpreters;
using TestWinBackGrnd.IO.GraphicFile.Parsers;
using TestWinBackGrnd.IO.GraphicFile.Symbols;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd
{
    public class ZULMachine
    {

        private Lexer lexer;
        private GraphicsParser parser;
        private MonoScopeVisitor symbolReader;

        public Graphics Gfx { get; private set; }
        public bool IsRunning { get; private set; }

        public ZULMachine(string script)
        {
            LoadScript(script);
            symbolReader = new MonoScopeVisitor();
            IsRunning = false;
        }

        public bool LoadGraphicsOut(Graphics g)
        {
            if (!IsRunning)
            {
                Gfx = g;
                return true;
            }
            else return false;
        }

        public void LoadScript(string script)
        {
            lexer = new GraphicsLexer(script);
            parser = new GraphicsParser(lexer);
        }

        public void Execute()
        {
            if (!IsRunning)
            {

                if (Gfx == null)
                    throw new NullReferenceException("Graphics object is null.");
                else if (lexer == null)
                    throw new NullReferenceException("Input lexer is null.");
                else if (symbolReader == null)
                    symbolReader = new MonoScopeVisitor();
                IsRunning = true;

                parser.Parse();
                parser.GetTree().Visit(symbolReader);
                SymbolTable symbolTable = symbolReader.GetSymbolTable();
                ZULInterpreter interpreter = new ZULInterpreter(symbolTable, Gfx);

                try
                {
                    parser.GetTree().Visit(interpreter);
                } catch (UnsupportedVersionException e)
                {
                    Console.Write(e);
                    Console.Beep(500, 500);
                    System.Threading.Thread.Sleep(250);
                    Console.Beep(500, 500);
                } catch (Exception e)
                {
                    Console.Write(e);
                }

                IsRunning = false;

            }
            else return;
        }

    }
}
