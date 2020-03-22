﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA2.AST;
using SPA2.Enums;

namespace SPA2.AST
{
    class Program
    {
        static void Main(string[] args)
        {
            AST ast = new AST();
            ast.SetRoot(ast.CreateTNode(EntityTypeEnum.Procedure));
        }
    }
}
