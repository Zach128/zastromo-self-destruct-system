﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Models;

namespace TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes
{
    public class NameNode : ExprNode
    {
        public NameNode(Token token) : base(token)
        { EvalType = NodeType.NAME; }
    }
}
