using System;
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
            TNODE node = ast.CreateTNode(EntityTypeEnum.Procedure);
            TNODE node1 = ast.CreateTNode(EntityTypeEnum.Call);
            TNODE node2 = ast.CreateTNode(EntityTypeEnum.Program);
            TNODE node3 = ast.CreateTNode(EntityTypeEnum.If);
            ast.SetRoot(node);
            ast.SetFollows(node3, node2);
            ast.SetFollows(node2, node);
            ast.SetFollows(node1, node);
            
        }
    }
}
