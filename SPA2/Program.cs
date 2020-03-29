using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA2.AST;
using SPA2.Enums;
using SPA2.Interfaces;
using SPA2.VarTable;

namespace SPA2.AST
{
    class Program
    {
        static void Main(string[] args)
        {
            String query = "stmt s1, s2; assign a; while w;\nSelect s1 such that Follows (s1, s2) with s1.stmt#= 5;";
            QueryProcessor.processQuery(query);

        }
    }
}
