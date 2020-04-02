using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA2.AST;
using SPA2.Enums;
using SPA2.Interfaces;
using SPA2.VarTable;
using SPA2.QueryProcessor;

namespace SPA2.AST
{
    class Program
    {
        static void Main(string[] args)
        {
          //  string query = "stmt s;select s";
          //  string query = "stmt s1, s2; assign a, a1; while w; procedure p;Select s1 such that Follows (s1, s2) and Parent(w, s1);";
            string query = "stmt s1, s2; assign a, a1; while w; procedure p;Select s1 such that Follows (s1, s2) and Parent(w, s1) with s1.stmt#= 5;";
            QueryProcessor.QueryProcessor.ProcessQuery(query);
            Console.ReadLine();
        }
    }
}
