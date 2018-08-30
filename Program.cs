﻿using SelfDestructCommons;
using SelfDestructCommons.Model.GraphicsMessages;
using System;
using System.Collections.Generic;
using TestWinBackGrnd.IO.GraphicFile;
using TestWinBackGrnd.IO.GraphicFile.Interpreters;
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

            Instance.winOverride.NewDrawWarning();
            Console.WriteLine("\nExecution complete.");
            Console.ReadKey();

            /*
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

            Console.WriteLine("\nToken printing complete. Press any key to continue...");
            Console.ReadKey();
            //*/

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
                        winOverride.NewDrawWarning();
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
